Shader "Hidden/Raymarcher Blit"
{
	SubShader
	{
		Tags { "RenderType"="Transparnet" "Queue" = "Geometry-1" }
		Cull Off
		ZWrite On
        Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#pragma multi_compile __ CONVERT_TO_GAMMA
			#pragma multi_compile __ HIGHLIGHT_OBJECTS

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv     : TEXCOORD0;
				uint   id     : SV_VertexID;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _RaymarchColor;
			sampler2D _RaymarchDepth;
			
			#if HIGHLIGHT_OBJECTS
			sampler2D ObjectIDTexture;
			float objectToHighlight;
			fixed4 objectHighlightColor;
			#endif

			float2 TransformTriangleVertexToUV(float2 vertex)
			{
				float2 uv = (vertex + 1.0) * 0.5;
				return uv;
			}

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = float4(v.vertex.xy, 0.0, 1.0);
				o.uv = TransformTriangleVertexToUV(v.vertex.xy);
			#if UNITY_UV_STARTS_AT_TOP
				o.uv = o.uv * float2(1.0, -1.0) + float2(0.0, 1.0);
			#endif
				return o;
			}

			struct FragOut
			{
				fixed4 color : SV_Target;
				float depth : SV_Depth;
			};
			
			float4 LinearToSrgb(float4 color) {
              // Approximation http://chilliant.blogspot.com/2012/08/srgb-approximations-for-hlsl.html
              float3 linearColor = color.rgb;
              float3 S1 = sqrt(linearColor);
              float3 S2 = sqrt(S1);
              float3 S3 = sqrt(S2);
              color.rgb = 0.662002687 * S1 + 0.684122060 * S2 - 0.323583601 * S3 - 0.0225411470 * linearColor;
              return color;
            }

			FragOut frag(v2f i)
			{
				FragOut result;
				
				fixed4 col = tex2D(_RaymarchColor, i.uv);
				
				#if CONVERT_TO_GAMMA
				col = LinearToSrgb(col);
				#endif
				
				result.color = col;
				
				#if HIGHLIGHT_OBJECTS
				#define HIGHLIGHT_EPSILON 0.001
				float objectID = tex2D(ObjectIDTexture, i.uv).r;
				if (abs(objectID - objectToHighlight) < HIGHLIGHT_EPSILON)
				{
				    result.color = lerp(result.color, fixed4(objectHighlightColor.rgb, 1.0), objectHighlightColor.a);
				}
				#endif
				
                result.depth = SAMPLE_DEPTH_TEXTURE(_RaymarchDepth, i.uv);
				return result;
			}
			ENDCG
		}
	}
}
