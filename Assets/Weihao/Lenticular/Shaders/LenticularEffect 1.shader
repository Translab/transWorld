// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/LenticularEffectVertical"
{
	Properties
	{
		_MainTex ("Albedo", 2D) = "white" {}
        _Tint ("Tint", Color) = (1, 1, 1, 1)
        _Smoothness ("Smoothness", Range(0, 1)) = 0.5
        _FacePos("Face Position", Vector) = (0.0, 0.0, 0.0, 0.0)
        _PhotoTex1("Texture1", 2D) = "red"{}
        _PhotoTex2("Texture2", 2D) = "green"{}
        _PhotoTex3("Texture3", 2D) = "blue"{} 
        _PhotoTex4("Texture4", 2D) = "red"{}
        _PhotoTex5("Texture5", 2D) = "green"{}
        _PhotoTex6("Texture6", 2D) = "blue"{}
        _PhotoTex7("Texture7", 2D) = "red"{}
        _PhotoTex8("Texture8", 2D) = "green"{}
        _PhotoTex9("Texture9", 2D) = "blue"{}
        _PhotoTex10("Texture10", 2D) = "red"{}

	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
            Tags {
                "LightMode" = "ForwardBase"
            }
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			//#include "UnityCG.cginc"
            #include "UnityStandardBRDF.cginc"


			struct VertexData
			{
                float4 position : POSITION;
                float3 normal : NORMAL;
                float2 uv: TEXCOORD0;
			};

			struct Interpolators
			{
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal: TEXCOORD1;
                float4 worldPos: TEXCOORD2;

			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
            float3 _Tint;
			float _Smoothness;
            float4 _FacePos;
            sampler2D _PhotoTex1;
            sampler2D _PhotoTex2;
            sampler2D _PhotoTex3;
            sampler2D _PhotoTex4;
            sampler2D _PhotoTex5;
            sampler2D _PhotoTex6;
            sampler2D _PhotoTex7;
            sampler2D _PhotoTex8;
            sampler2D _PhotoTex9;
            sampler2D _PhotoTex10;
            
			Interpolators vert (VertexData v)
			{
				Interpolators i;
                
                i.position = UnityObjectToClipPos(v.position);
                i.worldPos = mul(unity_ObjectToWorld, v.position);
                i.normal = UnityObjectToWorldNormal(v.normal);
                i.uv = TRANSFORM_TEX(v.uv, _MainTex);

                
                // original code
                //
				//o.vertex = UnityObjectToClipPos(v.vertex);
				//o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//UNITY_TRANSFER_FOG(o,o.vertex);
				return i;
			}
			
			fixed4 frag (Interpolators i) : SV_Target
			{
                i.normal = normalize(i.normal);
                float3 lightDir = _WorldSpaceLightPos0.xyz;
                float3 viewDir = normalize(_FacePos.xyz - i.worldPos);
                float3 reflectionDir = reflect(-lightDir, i.normal);
                
                // float angle =  acos(DotClamped(normalize(float3(i.normal.x, 0.0, i.normal.z)),normalize(float3(viewDir.x, 0.0, viewDir.z))));
                float angle =  acos(DotClamped(normalize(float3(i.normal.x, i.normal.y, 0.0)),normalize(float3(viewDir.x, viewDir.y, 0.0))));
                //float angle =  acos(DotClamped(i.normal , viewDir));
                if(i.normal.x < viewDir.x) angle = -angle;
                
                
                float O = 60.0; 
                
                int faceNum = 10;                
                float normalAngles[10];
                float visbility[10];
                for(int idx = 0 ; idx < faceNum ; idx++){
                     visbility[idx] = 0;
                }

                for(int idx = 0 ; idx < faceNum ; idx++){
                    int zoom = floor((angle + radians(O/2.0))/radians(O));
                    //if (zoom < 0) zoom += 1;
                    float moddedAngle = (angle - zoom * radians(O));
                    //float moddedAngle = angle;
                    float slotAngle = (2 * idx + 1) * radians(O)/faceNum/2.0 - radians(O/2.0);
                    float angleDiff = abs(moddedAngle - slotAngle);                                       
                    if(idx == 0) {
                        float possibleAngleDiff = abs(moddedAngle - radians(O) - slotAngle);
                        angleDiff = min(angleDiff, possibleAngleDiff);
                    }
                    if(idx == (faceNum-1)) {
                        float possibleAngleDiff = abs(moddedAngle + radians(O) - slotAngle);
                        angleDiff = min(angleDiff, possibleAngleDiff);
                    }             
                    
                    if(angleDiff <= radians(O)/faceNum){
                        visbility[idx] = 1.0 - angleDiff/radians(O)*faceNum;
                        //visbility[idx] = 1.0 - max(0.0, min(angleDiff/(radians(O)/faceNum),1.0));               
                    }
                    
 

                    
                    //visbility[idx] = 1 - max(0.0, min(abs((angle)-((2 * idx + 1) * radians(O)/faceNum/2.0 - radians(O/2.0)))/(radians(O)/faceNum),1.0)); 
                }
                
                fixed4 col = tex2D(_PhotoTex1, i.uv.xy) * visbility[0]
                    + tex2D(_PhotoTex2, i.uv.xy) * visbility[1]
                    + tex2D(_PhotoTex3, i.uv.xy) * visbility[2]
                    + tex2D(_PhotoTex4, i.uv.xy) * visbility[3]
                    + tex2D(_PhotoTex5, i.uv.xy) * visbility[4]
                    + tex2D(_PhotoTex6, i.uv.xy) * visbility[5]
                    + tex2D(_PhotoTex7, i.uv.xy) * visbility[6]
                    + tex2D(_PhotoTex8, i.uv.xy) * visbility[7]
                    + tex2D(_PhotoTex9, i.uv.xy) * visbility[8]
                    + tex2D(_PhotoTex10, i.uv.xy) * visbility[9]
                    ;
                return col;

                
                //float3 lightColor = _LightColor0.rgb;  
                //float3 albedo = tex2D(_MainTex, i.uv).rgb * _Tint.rgb;                                    
                //float3 diffuse = albedo * lightColor * DotClamped(lightDir, i.normal);

                //return diffuse + pow(DotClamped(viewDir, reflectionDir), _Smoothness * 100);

                //return float4(diffuse,1);
                //return float4(i.normal * 0.5 + 0.5 , 1);
                
            
                // original code
                //            
				//// sample the texture
				//fixed4 col = tex2D(_MainTex, i.uv);
				//// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
				//return col;
			}
			ENDCG
		}
	}
}
