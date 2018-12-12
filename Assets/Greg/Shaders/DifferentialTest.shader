// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/DifferentialTest" {
	Properties {
		_ElementFreq ("Element Frequency", Float) = 1
		_ElementRotOrigin("Element Rotation Origin", Vector) = (0,0,0,1)
		_RingFreq ("Ring Frequency", Float) = 1
		_ObjectFreq("Object Frequency", Float) = 1
		_SystemFreq("System Frequency", Float) = 1

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
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _ElementFreq;
		float4 _ElementRotOrigin;

		/*
		struct appdata {
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float2 texcoord : TEXCOORD0;
		};
		*/

		float4 RotateAroundYInDegrees(float4 vertex, float degrees)
		{
			float alpha = degrees * UNITY_PI / 180.0;
			float sina, cosa;
			sincos(alpha, sina, cosa);
			float2x2 m = float2x2(cosa, -sina, sina, cosa);
			return float4(mul(m, vertex.xz), vertex.yw).xzyw;
		}

		float3x3 YRotationMatrix(float degrees)
		{
			float alpha = degrees * UNITY_PI / 180.0;
			float sina, cosa;
			sincos(alpha, sina, cosa);
			return float3x3(
				cosa, 0, -sina,
				0, 1, 0,
				sina, 0, cosa);
		}

		float4 testRotateAroundYInDegrees(float4 vertex, float degrees)
		{
			float alpha = degrees * UNITY_PI / 180.0;
			float sina, cosa;
			sincos(alpha, sina, cosa);
			float2x2 m = float2x2(cosa, -sina, sina, cosa);
			return float4(mul(m, vertex.yz), vertex.xw).xzyw;
		}

		void vert(inout appdata_full v) {
			float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
			//float4 pos = v.vertex;
			//float4 pivot = pos;
			//float3 pivot = v.texcoord1.yzw; //Vertex Stream Center

		   //Move to root
			//pos.xyz -= pos.xyz;

			//Rotate
			//pos.xyz = mul(YRotationMatrix(180 * _Time.y), pos.xyz);

			//Move it back
			//pos.xyz += pivot;
			//v.vertex.xyz = pos.xyz;
			//v.vertex = testRotateAroundYInDegrees(worldPos, 10 * _Time.y);

			//float3 worldPos = mul(_object2World, v.vertex).xyz; //get worldPos
			//v.vertex.xyz is localPos
			//v.vertex.x += sin(v.ver)
			float rotatedPos = RotateAroundYInDegrees(worldPos, 50 * _Time.y);
			v.vertex = v.vertex + rotatedPos;

			//v.vertex.z += sin(v.vertex.xyz * _ElementFreq + _Time.y) * 1;
			//v.vertex.xyz += v.normal *  sin(v.vertex.y * _ElementFreq + _Time.y) * 1;
			//v.vertex.xyz += v.normal *  sin(v.vertex.z * _ElementFreq + _Time.y) * 1;

			//v.vertex.xyz += v.normal *  sin(v.vertex.x * _ElementFreq + _Time.y) * 1;
			//float3 worldPos = mul(_object2World, v.vertex).xyz;
			//v.vertex.xyz += v.normal *  sin(v.vertex.x * _Frequency + _Time.y) * _Amplitude;
			//v.vertex.xyz += v.normal *  sin(v.vertex.x * _Frequencyx + _Time.y) * _Amplitude;
			//v.vertex.xyz += v.normal *  sin(v.vertex.y * _Frequencyy + _Time.y) * _Amplitude;
			//v.vertex.xyz += v.normal *  sin(v.vertex.z * _Frequencyz + _Time.y) * _Amplitude;

		}

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

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
