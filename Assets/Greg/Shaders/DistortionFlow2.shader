Shader "Custom/DistortionFlow2" {
	Properties {
		_EdgeLength("Edge length", Range(2,50)) = 15
		//_Phong("Phong Strengh", Range(0,1)) = 0.5
		_Amount ("Extrusion Amount", Range(-1,1)) = 0.5
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		[NoScaleOffset] _FlowMap ("Flow (RG, A noise)", 2D) = "black" {}
		[NoScaleOffset] _DerivHeightMap ("Deriv (AG) Height (B)", 2D) = "black" {}
		_UJump ("U jump per phase", Range(-0.25, 0.25)) = 0.25
		_VJump ("V jump per phase", Range(-0.25, 0.25)) = 0.25
		_Tiling ("Tiling", Float) = 1
		_Speed ("Speed", Float) = 1
		_FlowStrength ("Flow Strength", Float) = 1
		_FlowOffset ("Flow Offset", Float) = 0
		_HeightScale ("Height Scale, Constant", Float) = 0.25
		_HeightScaleModulated ("Height Scale, Modulated", Float) = 0.75
		_WaterFogColor ("Water Fog Color", Color) = (0, 0, 0, 0)
		_WaterFogDensity ("Water Fog Density", Range(0, 2)) = 0.1
		_RefractionStrength ("Refraction Strength", Range(0, 1)) = 0.25
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
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
		
		static half _Frequencyx = 10;
		static half _Frequencyy = 5;
		static half _Frequencyz = 1;

		static half _Amplitude = 1;

		float _Amount;
		
		void vert(inout appdata_full v) {
			//UNITY_INITIALIZE_OUTPUT(appdata_full, v);

			//float3 worldPos = mul(_object2World, v.vertex).xyz;
			//v.vertex.xyz += v.normal *  sin(v.vertex.x * _Frequency + _Time.y) * _Amplitude;
			v.vertex.xyz += v.normal *  sin(v.vertex.x * _Frequencyx + _Time.y) * _Amplitude;
			v.vertex.xyz += v.normal *  sin(v.vertex.y * _Frequencyy + _Time.y) * _Amplitude;
			v.vertex.xyz += v.normal *  sin(v.vertex.z * _Frequencyz + _Time.y) * _Amplitude;

		}

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		float3 UnpackDerivativeHeight (float4 textureData) {
			float3 dh = textureData.agb;
			dh.xy = dh.xy * 2 - 1;
			return dh;
		}

		void surf (Input IN, inout SurfaceOutput o) {
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
		
 		void ResetAlpha (Input IN, SurfaceOutput o, inout fixed4 color) {
			color.a = 1;
		}
		
		ENDCG
	}
}