Shader "Custom/FloorMat" {
	Properties{

		_Step("Step", Float) = 1.0
		_Distance("Distance",Float) = 1
		//_WaveSpeed("Amplitude", Float) = 1
		_Amount("Amount", Range(0.0, 1.0)) = 1.0
		_Amplitude("Amplitude", Float) = 1
		_Frequency("Frequency", Float) = 1
		_Damping("Damping", Float) = 1

		//_VertexOffset("Offset", Float) = 1


		_EdgeLength("Edge length", Range(2,50)) = 15
		//_Phong("Phong Strengh", Range(0,1)) = 0.5
		//_Amount("Extrusion Amount", Range(-1,1)) = 0.5
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		[NoScaleOffset] _FlowMap("Flow (RG, A noise)", 2D) = "black" {}
		[NoScaleOffset] _DerivHeightMap("Deriv (AG) Height (B)", 2D) = "black" {}
		_UJump("U jump per phase", Range(-0.25, 0.25)) = 0.25
		_VJump("V jump per phase", Range(-0.25, 0.25)) = 0.25
		_Tiling("Tiling", Float) = 1
		_Speed("Speed", Float) = 1
		_FlowStrength("Flow Strength", Float) = 1
		_FlowOffset("Flow Offset", Float) = 0
		_HeightScale("Height Scale, Constant", Float) = 0.25
		_HeightScaleModulated("Height Scale, Modulated", Float) = 0.75
		_WaterFogColor("Water Fog Color", Color) = (0, 0, 0, 0)
		_WaterFogDensity("Water Fog Density", Range(0, 2)) = 0.1
		_RefractionStrength("Refraction Strength", Range(0, 1)) = 0.25
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}
		SubShader{
			Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "DisableBatching" = "true" }
			LOD 200

			GrabPass { "_WaterBackground" }

			CGPROGRAM
			//#pragma surface surf Standard alpha finalcolor:ResetAlpha vertex:vert tessellate:tessEdge
			#pragma surface surf BlinnPhong alpha finalcolor:ResetAlpha vertex:vert tessellate:tessEdge
			#pragma target 4.6

			#include "Tessellation.cginc"
			#include "Flow.cginc"
			#include "LookingThroughWater.cginc"
			//#include "MyTessellation.cginc"

			sampler2D _MainTex, _FlowMap, _DerivHeightMap;
			float _UJump, _VJump, _Tiling, _Speed, _FlowStrength, _FlowOffset;
			float _HeightScale, _HeightScaleModulated;

			//float _Phong;
			float _EdgeLength;

			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 texcoord : TEXCOORD0;
			};

			struct Input {
				float2 uv_MainTex;
				float4 screenPos;
			};


			float4 tessEdge(appdata_full v0, appdata_full v1, appdata_full v2)
			{
				return UnityEdgeLengthBasedTess(v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
			}

			//static half _Frequencyx = 10;
			//static half _Frequencyy = 5;
			//static half _Frequencyz = 1;

			//static half _Amplitude = 1;
					const float pi = 3.141592653589793238462;

			float _Amplitude;
			float _Step;
			float _WaveSpeed;
			float _Distance;
			float _Amount;
			float _Frequency;
			float _VertexOffset;
			float _Damping; 


			//float _Amount;
			
			void vert(inout appdata_full v) {
				//float3 worldPos = mul(_object2World, v.vertex).xyz;
				//v.vertex.xyz += v.normal *  sin(v.vertex.x * _Frequency + _Time.y) * _Amplitude;
				
				//float _U = (v.vertex.x + 0.5) * _Step - 1;
				//float _V = (v.vertex.y + 0.5) * _Step - 1;

				float _U = v.vertex.x;
				float _V = v.vertex.z;
					

				float d = sqrt(_U * _U + _V * _V);

				float y = sin(_Frequency * d - _Time.y) * _Amplitude;

				v.vertex.y += y;
				v.vertex.y /= 1 + _Damping * d;


				//v.vertex.y += sin(_Time.y * _WaveSpeed + v.vertex.y * _Amplitude) * _Distance * _Amount;
				//v.vertex.y += sin(pi * (_Frequency * d - _Time.y + v.vertex.z)) * _Amplitude;
				//v.vertex.y += sin(pi * (4 * d - _Time.y) + v.vertex.y) * _Amplitude;
				//v.vertex.y /= 1 + 10 * d;
				//p.y = Mathf.Sin(pi * (4f * d - t));
				//p.y /= 1f + 10f * d;


				//v.vertex.xyz += v.normal *  sin(v.vertex.x * _Frequencyx + _Time.y) * _Amplitude;
				//v.vertex.xyz += v.normal *  sin(v.vertex.y * _Frequencyy + _Time.y) * _Amplitude;
				//v.vertex.xyz += v.normal *  sin(v.vertex.z * _Frequencyz + _Time.y) * _Amplitude;

			}

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;

			float3 UnpackDerivativeHeight(float4 textureData) {
				float3 dh = textureData.agb;
				dh.xy = dh.xy * 2 - 1;
				return dh;
			}

			void surf(Input IN, inout SurfaceOutput o) {
				float3 flow = tex2D(_FlowMap, IN.uv_MainTex).rgb;
				flow.xy = flow.xy * 2 - 1;
				flow *= _FlowStrength;
				float noise = tex2D(_FlowMap, IN.uv_MainTex).a;
				float time = _Time.y * _Speed + noise;
				float2 jump = float2(_UJump, _VJump);

				float3 uvwA = FlowUVW(
					IN.uv_MainTex, flow.xy, jump,
					_FlowOffset, _Tiling, time, false
				);
				float3 uvwB = FlowUVW(
					IN.uv_MainTex, flow.xy, jump,
					_FlowOffset, _Tiling, time, true
				);

				float finalHeightScale =
					flow.z * _HeightScaleModulated + _HeightScale;

				float3 dhA =
					UnpackDerivativeHeight(tex2D(_DerivHeightMap, uvwA.xy)) *
					(uvwA.z * finalHeightScale);
				float3 dhB =
					UnpackDerivativeHeight(tex2D(_DerivHeightMap, uvwB.xy)) *
					(uvwB.z * finalHeightScale);
				o.Normal = normalize(float3(-(dhA.xy + dhB.xy), 1));

				fixed4 texA = tex2D(_MainTex, uvwA.xy) * uvwA.z;
				fixed4 texB = tex2D(_MainTex, uvwB.xy) * uvwB.z;

				fixed4 c = (texA + texB) * _Color;
				o.Albedo = c.rgb;
				o.Specular = _Metallic;
				o.Gloss = _Glossiness;
				//o.Metallic = _Metallic;
				//o.Smoothness = _Glossiness;
				o.Alpha = c.a;

				o.Emission = ColorBelowWater(IN.screenPos, o.Normal) * (1 - c.a);
			}

			void ResetAlpha(Input IN, SurfaceOutput o, inout fixed4 color) {
				color.a = 1;
			}

			ENDCG
		}
}

/*
		//_WaveTime("WaveTime", Range(0, 1)) = 0.0
		//_WaveTime("WaveTime", Range(-100, 100)) = 0.0
		//_WaveTime("WaveTime", Range(-100, 100)) = 0.0
		_Step("Step", Range(0, 100)) = 1.0


		_Frequency("Frequency", Range(0, 100)) = 1.0
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 4.6

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)


		struct appdata {
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float2 texcoord : TEXCOORD0;
		};


		
		//static half _Frequencyx = 10;
		//static half _Frequencyy = 5;
		//static half _Frequencyz = 1;
		half _Frequency;
		half _Amplitude;
		half _Step;
		half _Speed;
		half _Distance;
		half _Amount;
		const float pi = 3.141592653589793238462;

		//half _Time;
		//half _U;
		//half _V;

		void vert(inout appdata_full v) {
			float _U = (v.vertex.x + 0.5) * _Step - 1;
			float _V = (v.vertex.z + 0.5) * _Step - 1;

			float d = sqrt(_U * _U + _V * _V);
			//Debug.Log(d);
			v.vertex.y += sin(_Time.y * _Speed + v.vertex.x * _Amplitude) * _Distance * _Amount;
			//v.vertex.xyz += v.normal * sin(pi * (4 * d - _Time.y)) * _Amplitude;
			//v.vertex.y /= 1 + 10 * d;

			//v.vertex.xyz +=

			//float3 worldPos = mul(_object2World, v.vertex).xyz;
			//v.vertex.xyz += v.normal *  sin(v.vertex.x * _Frequency + _Time.y) * _Amplitude;
			//v.vertex.xyz += v.normal *  sin(v.vertex.z * _Frequency + _Time.y) * _Amplitude;
			//v.vertex.xyz += v.normal *  sin(v.vertex.y * _Frequencyy + _Time.y) * _Amplitude;
			//v.vertex.xyz += v.normal *  sin(v.vertex.z * _Frequencyz + _Time.y) * _Amplitude;

		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
*/
