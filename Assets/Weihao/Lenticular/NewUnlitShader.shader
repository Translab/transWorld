Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _MainTex ("MainTexture", 2D) = "white" {}
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
        _FacePos("Face Position", Vector) = (0.0, 0.0, 0.0, 0.0)
    
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float3 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                
            };

            sampler2D _MainTex;
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
            float3 _FacePos;
            // float3 _LeftEyePos;
            // float3 _RightEyePos;
            
            float4 _MainTex_ST;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
                
                v.normal = UnityObjectToWorldNormal(v.normal);                
                float3 viewDirect = normalize(_FacePos.xyz - v.vertex);
                o.uv.z = -acos(dot(normalize(float3(v.normal.x, 0.0 ,v.normal.z)), normalize(float3(viewDirect.x,0.0,viewDirect.z))));
                if(v.normal.x < viewDirect.x) o.uv.z = - o.uv.z;
                //o.uv.z += 3.1415926/2.0;
                //o.uv.z = dot(v.normal, viewDirect);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col; 
                
                int faceNum = 10;                
                float normalAngles[10];
                float visbility[10];
                
                float shrink = 1.0;
                float allVisAngleRange = 60.0;
                float visAngleRange = allVisAngleRange/(float)faceNum * shrink;
                
                for(int idx = 0 ; idx < faceNum ; idx++){
                    visbility[idx] = 0.0f;
                }
                for(int idx = 0 ; idx < faceNum ; idx++){
                    normalAngles[idx] = ((float)idx + 0.5) * allVisAngleRange / (float)faceNum + (180.0 - allVisAngleRange)/2.0 - 90.0;                                       
                    
                    if (abs(i.uv.z) < allVisAngleRange/2.0) {                    
                        float angleDiff = abs(i.uv.z - radians(normalAngles[idx]));
                        visbility[idx] = max(0.0, radians(visAngleRange) - angleDiff)/radians(visAngleRange)/shrink;
                    }
                    else {
                        i.uv.z = i.uv.z - radians(60.0);
                        float angleDiff = abs(i.uv.z - radians(normalAngles[idx]));
                        visbility[idx] = max(0.0, radians(visAngleRange) - angleDiff)/radians(visAngleRange)/shrink;
                        //float modedAngle = 0.0;
                        //if (i.uv.z < - allVisAngleRange/2.0) {
                        //    int zm = floor((- allVisAngleRange/2.0 - i.uv.z) / allVisAngleRange);
                        //    modedAngle = i.uv.z + (zm+1) * allVisAngleRange;
                        //}
                        //if (i.uv.z > allVisAngleRange/2.0) {
                        //    int zm = floor((i.uv.z - allVisAngleRange/2.0) / allVisAngleRange);
                        //    modedAngle = i.uv.z - (zm+1) * allVisAngleRange;
                        //}
                        //float angleDiff = abs(modedAngle - radians(normalAngles[idx]));
                        //visbility[idx] = max(0.0, radians(visAngleRange) - angleDiff)/radians(visAngleRange)/shrink;                                                
                    }
                    
                }
                
                col = tex2D(_PhotoTex1, i.uv.xy) * visbility[0]
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
            }
            ENDCG
        }
    }
}
