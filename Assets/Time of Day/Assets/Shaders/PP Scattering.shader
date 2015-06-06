Shader "Hidden/Time of Day/Scattering"
{
	Properties
	{
		_DitheringTexture ("Dithering Lookup Texture (A)", 2D) = "black" {}
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}

	CGINCLUDE
	#include "UnityCG.cginc"

	#include "TOD_Base.cginc"
	#include "TOD_Scattering.cginc"

	#define BAYER_DIM 8.0

	uniform sampler2D _MainTex;
	uniform sampler2D_float _CameraDepthTexture;
	uniform sampler2D _DitheringTexture;

	uniform float4x4 _FrustumCornersWS;
	uniform float4 _MainTex_TexelSize;
	uniform float4 _Density;

	struct v2f {
		float4 pos       : SV_POSITION;
		float2 uv        : TEXCOORD0;
		float2 uv_depth  : TEXCOORD1;
		float2 uv_dither : TEXCOORD2;
		float3 cameraRay : TEXCOORD3;
		float3 skyDir    : TEXCOORD4;
	};

	v2f vert(appdata_img v) {
		v2f o;

		half index = v.vertex.z;
		v.vertex.z = 0.1;

		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

		o.uv        = v.texcoord.xy;
		o.uv_depth  = v.texcoord.xy;
		o.uv_dither = v.texcoord.xy * _ScreenParams.xy * (1.0 / BAYER_DIM);

		#if UNITY_UV_STARTS_AT_TOP
		if (_MainTex_TexelSize.y < 0)
			o.uv.y = 1-o.uv.y;
		#endif

		o.cameraRay = _FrustumCornersWS[(int)index];
		o.skyDir    = normalize(mul((float3x3)TOD_World2Sky, o.cameraRay));

		return o;
	}

	inline float FogDensity(float3 cameraToWorldPos)
	{
		float heightFalloff = _Density.x;
		float heightDensity = _Density.y;
		float globalDensity = _Density.z;

		// Unpack depth value
		float fogIntensity = length(cameraToWorldPos) * heightDensity;

		// Apply height falloff
		if (heightFalloff > 0 && abs(cameraToWorldPos.y) > 0.01)
		{
			float t = heightFalloff * cameraToWorldPos.y;
			fogIntensity *= (1.0 - exp(-t)) / t;
		}

		// Clamp intensity
		fogIntensity = min(10, globalDensity * fogIntensity);

		return 1.0 - exp(-fogIntensity);
	}

	half4 frag(v2f i) : COLOR {
		half4 sceneColor = tex2D(_MainTex, i.uv);

		float rawDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv_depth);
		float depth = Linear01Depth(rawDepth);
		float3 cameraToWorldPos = depth * i.cameraRay;

		if (depth != 1) depth = FogDensity(cameraToWorldPos);

		half dither = tex2D(_DitheringTexture, i.uv_dither).a * (1.0 / (BAYER_DIM * BAYER_DIM + 1.0));
		half4 scattering = ScatteringColor(i.skyDir, depth);

		if (depth == 1)
		{
			return half4(sceneColor.rgb + scattering.rgb + dither, sceneColor.a);
		}
		else
		{
			return half4(lerp(sceneColor.rgb, scattering.rgb + dither, depth), sceneColor.a);
		}
	}

	ENDCG

	SubShader
	{
		Pass
		{
			ZTest Always Cull Off ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			ENDCG
		}
	}

	Fallback Off
}
