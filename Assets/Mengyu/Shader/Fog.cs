// KinoFog Plus

// Modified by Mengyu Chen 2018

// Original Author: Keijiro Takahashi 2015
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using UnityEngine;

namespace Kino
{
    //[ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    //[AddComponentMenu("Kino Image Effects/Fog")]
    public class Fog : MonoBehaviour
    {

		#region Public Properties

        // Start distance
        [SerializeField]
        float _startDistance = 1;

        public float startDistance {
            get { return _startDistance; }
            set { _startDistance = value; }
        }

		// Use radial distance
        [SerializeField]
        bool _useRadialDistance;

        public bool useRadialDistance {
            get { return _useRadialDistance; }
            set { _useRadialDistance = value; }
        }

        // Fade-to-skybox flag
        [SerializeField]
        bool _fadeToSkybox;

        public bool fadeToSkybox {
            get { return _fadeToSkybox; }
            set { _fadeToSkybox = value; }
        }

		//Density
		[SerializeField]
		private float _fogdensity = 1;

		public float fogdensity {
			get { return _fogdensity;}
			set { _fogdensity = value; }
		}


        #endregion

        #region Private Properties

        [SerializeField] Shader _shader;

        Material _material;

        #endregion

        #region MonoBehaviour Functions

        void OnEnable()
        {
            GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
        }

        [ImageEffectOpaque]
        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (_material == null)
            {
                _material = new Material(_shader);
                _material.hideFlags = HideFlags.DontSave;
            }

            _startDistance = Mathf.Max(_startDistance, 0.0f);
			_fogdensity = Mathf.Max (_fogdensity, 0.0f);
            _material.SetFloat("_DistanceOffset", _startDistance);

            var mode = RenderSettings.fogMode;
            if (mode == FogMode.Linear)
            {
                var start = RenderSettings.fogStartDistance;
                var end = RenderSettings.fogEndDistance;
                var invDiff = 1.0f / Mathf.Max(end - start, 1.0e-6f);
                _material.SetFloat("_LinearGrad", -invDiff);
                _material.SetFloat("_LinearOffs", end * invDiff);
                _material.DisableKeyword("FOG_EXP");
                _material.DisableKeyword("FOG_EXP2");
            }
            else if (mode == FogMode.Exponential)
            {
                const float coeff = 1.4426950408f; // 1/ln(2)
                var density = RenderSettings.fogDensity;
                _material.SetFloat("_Density", coeff * density);
                _material.EnableKeyword("FOG_EXP");
                _material.DisableKeyword("FOG_EXP2");
            }
            else // FogMode.ExponentialSquared
            {
                const float coeff = 1.2011224087f; // 1/sqrt(ln(2))
                var density = RenderSettings.fogDensity;
				_material.SetFloat("_Density", coeff * 0.009f);
                _material.DisableKeyword("FOG_EXP");
                _material.EnableKeyword("FOG_EXP2");
            }

            if (_useRadialDistance)
                _material.EnableKeyword("RADIAL_DIST");
            else
                _material.DisableKeyword("RADIAL_DIST");

            if (_fadeToSkybox)
            {
                _material.EnableKeyword("USE_SKYBOX");
                // Transfer the skybox parameters.
                var skybox = RenderSettings.skybox;
                _material.SetTexture("_SkyCubemap", skybox.GetTexture("_Tex"));
                _material.SetColor("_SkyColor1", skybox.GetColor("_SkyColor1"));
				_material.SetColor("_SkyColor2", skybox.GetColor("_SkyColor2"));
				_material.SetColor("_SkyColor3", skybox.GetColor("_SkyColor3"));
				_material.SetColor("_SunColor", skybox.GetColor("_SunColor"));
				_material.SetVector("_SunVector", skybox.GetColor("_SunVector"));

				_material.SetFloat("_SkyExponent1", skybox.GetFloat("_SkyExponent1"));
				_material.SetFloat("_SkyExponent2", skybox.GetFloat("_SkyExponent2"));
				_material.SetFloat("_SkyIntensity", skybox.GetFloat("_SkyIntensity"));
				_material.SetFloat("_SunIntensity", skybox.GetFloat("_SunIntensity"));
				_material.SetFloat("_SunAlpha", skybox.GetFloat("_SunAlpha"));
				_material.SetFloat("_SunBeta", skybox.GetFloat("_SunBeta"));
				//const float coeff = 1.2011224087f; // 1/sqrt(ln(2))
				//                var density = RenderSettings.fogDensity;
				 //_material.SetFloat("_Density", coeff * fogdensity);

            }
            else
            {
                _material.DisableKeyword("USE_SKYBOX");
                //_material.SetColor("_FogColor", RenderSettings.fogColor);
            }

            // Calculate vectors towards frustum corners.
            var cam = GetComponent<Camera>();
            var camtr = cam.transform;
            var camNear = cam.nearClipPlane;
            var camFar = cam.farClipPlane;

            var tanHalfFov = Mathf.Tan(cam.fieldOfView * Mathf.Deg2Rad / 2);
            var toRight = camtr.right * camNear * tanHalfFov * cam.aspect;
            var toTop = camtr.up * camNear * tanHalfFov;

            var v_tl = camtr.forward * camNear - toRight + toTop;
            var v_tr = camtr.forward * camNear + toRight + toTop;
            var v_br = camtr.forward * camNear + toRight - toTop;
            var v_bl = camtr.forward * camNear - toRight - toTop;

            var v_s = v_tl.magnitude * camFar / camNear;

            // Draw screen quad.
            RenderTexture.active = destination;

            _material.SetTexture("_MainTex", source);
            _material.SetPass(0);

            GL.PushMatrix();
            GL.LoadOrtho();
            GL.Begin(GL.QUADS);

            GL.MultiTexCoord2(0, 0, 0);
            GL.MultiTexCoord(1, v_bl.normalized * v_s);
            GL.Vertex3(0, 0, 0.1f);

            GL.MultiTexCoord2(0, 1, 0);
            GL.MultiTexCoord(1, v_br.normalized * v_s);
            GL.Vertex3(1, 0, 0.1f);

            GL.MultiTexCoord2(0, 1, 1);
            GL.MultiTexCoord(1, v_tr.normalized * v_s);
            GL.Vertex3(1, 1, 0.1f);

            GL.MultiTexCoord2(0, 0, 1);
            GL.MultiTexCoord(1, v_tl.normalized * v_s);
            GL.Vertex3(0, 1, 0.1f);

            GL.End();
            GL.PopMatrix();
        }

        #endregion
    }
}
