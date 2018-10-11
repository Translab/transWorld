/*
                                            _
   _ __ __ _ _   _ _ __ ___   __ _ _ __ ___| |__ (_)_ __   __ _ 
  | '__/ _` | | | | '_ ` _ \ /  ` | '__/ __| '_ \| | '_ \ /  ` |
  | | | (_| | |_| | | | | | |     | | | (__| | | | | | | |     |
  |_|  \__,_|\__, |_| |_| |_|\__,_|_|  \___|_| |_|_|_| |_|\__, |
             |___/                                        |___/ 
   _              _ _    _ _   
  | |_ ___   ___ | | | _(_) |_ 
  | __/   \ /   \| | |/ / | __|
  | ||     |     | |   <| | |_   for Unity
   \__\___/ \___/|_|_|\_\_|\__|
                              

  This shader was automatically generated from
  [[SOURCE_FILE]]
  
  for Raymarcher '[[RAYMARCHER_NAME]]' in scene '[[SCENE_NAME]]'.

*/


Shader "Hidden/Raymarch Template NOZ"
{

SubShader
{

Tags {
	"RenderType" = "Transparent"
	"Queue" = "Transparent-1"
	"DisableBatching" = "True"
	"IgnoreProjector" = "True"
}

Cull Off
ZWrite On
// ZTest Always

Pass
{

CGPROGRAM

#pragma shader_feature RENDER_OBJECT

#pragma vertex vert
#pragma fragment frag

#if RENDER_OBJECT
#include "UnityCG.cginc" // @noinlineinclude
#include "UnityPBSLighting.cginc" // @noinlineinclude
#endif

#define USE_OPTIMIZED_NORMAL 1

float _Steps; // STUB
float ConservativeStepFactor; // STUB
uniform float _DrawDistance;

//[[GLOBALS]]

#include "../../../Assets/Shaders/Raymarching.cginc"
#include "../../../Assets/Shaders/CustomIncludes.cginc"

//[[LIGHTUNIFORMS]]

//[[UNIFORMS_AND_FUNCTIONS]]

float2 map(float3 p) {
	float2 result = float2(1.0, 0.0);
	//[[OBJECTS]]
	return result;
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

fixed4 raymarch(float3 ro, float3 rd, float s, inout float3 raypos, out float objectID)
{
	bool found = false;
	objectID = 0.0;

	float2 d;
	float t = 0; // current distance traveled along ray
	float3 p = float3(0, 0, 0);

#if FADE_TO_SKYBOX
	const float skyboxAlpha = 0;
#else
	const float skyboxAlpha = 1;
#endif

#if FOG_ENABLED
	fixed4 ret = fixed4(FogColor, skyboxAlpha);
#else
	fixed4 ret = fixed4(0,0,0,0);
#endif

	int numSteps;
	d = trace(ro, rd, raypos, numSteps, found);
	t = d.x;
	p = raypos;

#if DEBUG_STEPS
	float3 c = float3(1,0,0) * (1-(t / (float)numSteps));
	return fixed4(c, 1);
#elif DEBUG_MATERIALS
	float3 c = float3(1,1,1) * (d.y / 20);
	return fixed4(c, 1);
#endif

	[branch]
	if (found)
	{
		// First, we sample the map() function around our hit point to find the normal.
		float3 n = calcNormal(p);

		// Then, we get the color of the world at that point, based on our material ids.
		float3 color = MaterialFunc(d.y, n, p, rd, objectID);
		float3 light = getLights(color, p, n);

		// The ambient color is applied.
		color *= _AmbientColor.xyz;

		// And lights are added.
		color += light;

		// If enabled, darken with ambient occlusion.
		#if AO_ENABLED
		color *= ambientOcclusion(p, n);
		#endif

		// If fog is enabled, lerp towards the fog color based on the distance.
		#if FOG_ENABLED
		color = lerp(color, FogColor, 1.0-exp2(-FogDensity * t * t));
		#endif

		// If fading to the skybox is enabled, reduce the alpha value of the output pizel.
		#if FADE_TO_SKYBOX
		float alpha = lerp(1.0, 0, 1.0 - (_DrawDistance - t) / FadeToSkyboxDistance);
		#else
		float alpha = 1.0;
		#endif

		ret = fixed4(color, alpha);
	}

	raypos = p;
	return ret;
}

struct vertexFSTriangleInput
{
	float4 vertex : POSITION;
};

#if RENDER_OBJECT
struct v2f
{
    float4 pos         : SV_POSITION;
    float4 screenPos   : TEXCOORD0;
    float4 worldPos    : TEXCOORD1;
};
#else
struct v2f
{
	float4 vertex : SV_POSITION;
	float4 screenPos : TEXCOORD0;
};
#endif

v2f vert(vertexFSTriangleInput input)
{
	#if RENDER_OBJECT
		v2f o;
		o.pos = UnityObjectToClipPos(input.vertex);
		o.screenPos = o.pos;
		o.worldPos = mul(unity_ObjectToWorld, input.vertex);
	#else
		v2f o;
		UNITY_INITIALIZE_OUTPUT(v2f, o);
		o.vertex = input.vertex;
		o.screenPos = input.vertex;
	#endif

	return o;
}

struct FragOut
{
	fixed4 col : COLOR0;
	float4 col1 : COLOR1;
	float depth : SV_Depth;
};

#include "../../../Assets/Shaders/camera.cginc"

FragOut frag (v2f i)
{
	FragOut o;
	
	float depth = _DrawDistance;
    float3 rayDir = GetCameraDirection(i.screenPos);
    float3 rayOrigin = GetCameraPosition() + GetCameraNearClip() * rayDir;
    
	float3 raypos = float3(0, 0, 0);
	float objectID = 0.0;
	
	o.col = raymarch(rayOrigin, rayDir, depth, raypos, objectID);
	o.col1 = float4(objectID, 0, 0, 1);
	// o.depth = compute_depth(mul(UNITY_MATRIX_VP, float4(raypos, 1.0)));
	
	// #if !FOG_ENABLED
	// clip(o.col.a < 0.001 ? -1.0 : 1.0);
	// #endif
	
	return o;
}
ENDCG

}
}
}
