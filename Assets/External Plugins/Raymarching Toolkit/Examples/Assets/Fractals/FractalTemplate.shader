Shader "Hidden/Fractal Template"
{

SubShader
{

Tags {
	"RenderType" = "Opaque"
	"Queue" = "Geometry-1"
	"DisableBatching" = "True"
	"IgnoreProjector" = "True"
}

Cull Off
ZWrite On

Pass
{

CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#define USE_OPTIMIZED_NORMAL 1

#pragma multi_compile __ ENABLE_REFLECTIONS

float _Steps; // STUB
float ConservativeStepFactor; // STUB
uniform float _DrawDistance;
uniform sampler2D _SkyGradient;
uniform float _SkyGradientNoise;
uniform float _Glow;
uniform float4 _GlowColor;
uniform int _Reflections;
uniform int _ReflectionSteps;
uniform float4 _ReflectionColor;

//[[GLOBALS]]

//[[LIGHTUNIFORMS]]

#include "../../../Assets/Shaders/Raymarching.cginc"
#include "../../../Assets/Shaders/CustomIncludes.cginc"

//[[UNIFORMS_AND_FUNCTIONS]]

float2 map(float3 p) {
	//[[OBJECTDISPLACEMENTS]]
	float2 result = float2(1.0, 0.0);
	//[[OBJECTS]]
	return result;
}

fixed4 sampleGradient(sampler2D tex, float value) {
	return tex2Dlod(tex, float4(value,0,0,0));
}

float3 getLights(in float3 color, in float3 pos, in float3 normal) {
	LightInput input;
	input.pos = pos;
	input.color = color;
	input.normal = normal;

	float3 lightValue = float3(0, 0, 0);
	//[[LIGHTS]]
	return lightValue;
}

float2 traceFr(float3 o, float3 d, out float3 raypos, int maxSteps, out int numSteps, inout bool found) {
  float t_min = get_camera_near_plane();
  float t_max = _DrawDistance - get_camera_near_plane();
  float t = t_min;
  float candidate_error = INFINITY;
  float candidate_t = t_min;
  float candidate_mat = 0;
  float previousRadius = 0;
  float stepLength = 0;
  float functionSign = map(o).x < 0 ? -1 : +1;
  float pixelRadius = _ScreenParams.z - 1.0;
  raypos = o;
  float mat = 0;
  [loop]
  for (int i = 0; i < maxSteps; ++i) {
    raypos = d * t + o;
    float2 hit = map(raypos);
    float signedRadius = functionSign * hit.x;
    mat = hit.y;
    float radius = abs(signedRadius);

    bool sorFail = omega > 1 && (radius + previousRadius) < stepLength;
    if (sorFail) {
      stepLength -= omega * stepLength;
      omega = 1;
    } else {
      stepLength = signedRadius * omega;
    }

    previousRadius = radius;
    float error = radius / t;
    if (!sorFail && error < candidate_error) {
      candidate_t = t;
      candidate_error = error;
      candidate_mat = mat;
    }

    [branch]
    if (!sorFail && error < pixelRadius || t > t_max)
      break;

    t += stepLength * ConservativeStepFactor;
    numSteps++;
  }

  if ((t > t_max || candidate_error > pixelRadius))
    found = false;
  else
    found = true;
  return float2(candidate_t, candidate_mat);
}

float4 getColor(float3 p, float2 d, float3 rayDir, int numSteps, float3 bgColor)
{
	float t = d.x;

	// First, we sample the map() function around our hit point to find the normal.
	float3 n = calcNormal(p);

	// Then, we get the color of the world at that point, based on our material ids.
	float objectID = 0;
	float3 color = MaterialFunc(d.y, n, p, rayDir, objectID);
	float3 light = getLights(color, p, n);

	// The ambient color is applied.
	color *= _AmbientColor.xyz;

	// And lights are added.
	color += light;

	// Steps-based glow
	float s = numSteps / _Steps;
	s = clamp(s,0,1);
	color *= 1 + _Glow * s;
	color += _GlowColor * s;
	// color += _GlowColor * dot(1-s,2);

	// If enabled, darken with ambient occlusion.
	#if AO_ENABLED
	color *= ambientOcclusion(p, n);
	#endif

	// If fog is enabled, lerp towards the fog color based on the distance.
	#if FOG_ENABLED
	color = lerp(color, bgColor, 1.0-exp2(-FogDensity * t * t));
	#endif

	// If fading to the skybox is enabled, reduce the alpha value of the output pizel.
	#if FADE_TO_SKYBOX
	float alpha = lerp(1.0, 0, 1.0 - (_DrawDistance - t) / FadeToSkyboxDistance);
	#else
	float alpha = 1.0;
	#endif

	return fixed4(color, alpha);
}

fixed4 raymarch(float3 ro, float3 rd, float s, inout float3 raypos) {
	bool found = false;

	float2 d;
	float t = 0; // current distance traveled along ray
	float3 p = float3(0, 0, 0);

	// Sample the sky gradient
	float3 gradientray = rd;
	if (_SkyGradientNoise != 0)
		gradientray += rand(rd.xy) * 0.03 * _SkyGradientNoise;
	// get the angle of the gradient to fake a dome
	// TODO: make this value spherical
	float g = gradientray.y;
	g = 0.5 + g * 0.5;
	// g = (dot(gradientray, float3(0,1,0)) + 1.) / 2.;
	fixed4 ret = sampleGradient(_SkyGradient, g);

	#if FADE_TO_SKYBOX
	const float skyboxAlpha = 0;
	#else
	const float skyboxAlpha = 1;
	#endif

	#if FOG_ENABLED
	// ret = fixed4(FogColor, skyboxAlpha);
	#else
	// ret = fixed4(0,0,0,0);
	#endif

	int numSteps;
	d = traceFr(ro, rd, raypos, _Steps, numSteps, found);
	t = d.x;
	p = raypos;

	#if DEBUG_STEPS
	// float3 c = float3(1,0,0) * fmod(t, 0.1);
	float3 c = float3(1,0,0) * (1-(t / (float)numSteps));
	return fixed4(c, 1);
	#elif DEBUG_MATERIALS
	float3 c = float3(1,1,1) * (d.y / 20);
	return fixed4(c, 1);
	#endif


	[branch]
	if (found) 
	{
		#if ENABLE_REFLECTIONS
		ret = getColor(p, d, rd, numSteps, ret.rgb);
		float3 refp = p;
		float3 refraypos = raypos;
		float2 refd = d;
		for(int i = 0; i < _Reflections; ++i)
		{
			refraypos = calcNormal(refp);
			found = false;
			numSteps = 0;
			refd = traceFr(refp, refraypos, refp, _ReflectionSteps, numSteps, found);
			if (!found)
				break;
			float4 refcol = getColor(refp, refd, rd, numSteps, ret.rgb);
			refcol = float4(refcol.rgb * _ReflectionColor.rgb, refcol.a);
			// refcol.rgb *=  _ReflectionColor.rgb;
			ret = lerp(ret, refcol, 0.5 * _ReflectionColor.a);
		}
		#else
		ret = getColor(p, d, rd, numSteps, ret.rgb);
		#endif
	}

	raypos = p;
	return ret;
}


struct vertexFSTriangleInput
{
	float4 vertex : POSITION;
};


struct v2f
{
	float4 vertex : SV_POSITION;
	float4 screenPos : TEXCOORD0;
};

v2f vert(vertexFSTriangleInput input)
{
	v2f o;
	UNITY_INITIALIZE_OUTPUT(v2f, o);
	o.vertex = input.vertex;
	o.screenPos = input.vertex;
	return o;
}

struct FragOut
{
	fixed4 col : SV_Target;
	float depth : SV_Depth;
};

#include "../../../Assets/Shaders/camera.cginc"

FragOut frag (v2f i)
{
	FragOut o;

    float3 rayDir = GetCameraDirection(i.screenPos);
    float3 rayOrigin = GetCameraPosition() + GetCameraNearClip() * rayDir;

	float3 raypos = float3(0, 0, 0);
	o.col = raymarch(rayOrigin, rayDir, _DrawDistance, raypos);
	o.depth = compute_depth(mul(UNITY_MATRIX_VP, float4(raypos, 1.0)));
	#if !FOG_ENABLED
	clip(o.col.a < 0.001 ? -1.0 : 1.0);
	#endif
	return o;
}

ENDCG

}
}
}
