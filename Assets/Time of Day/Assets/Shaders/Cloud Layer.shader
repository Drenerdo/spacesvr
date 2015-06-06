Shader "Time of Day/Cloud Layer"
{
	Properties
	{
		_MainTex ("Alpha Layers (RGB)", 2D) = "white" {}
		_BumpMap ("Normal Map",         2D) = "bump"  {}
	}

	SubShader
	{
		Tags
		{
			"Queue"="Geometry+520"
			"RenderType"="Background"
			"IgnoreProjector"="True"
		}

		Pass
		{
			Cull Front
			ZWrite Off
			ZTest LEqual
			Blend SrcAlpha OneMinusSrcAlpha
			Fog { Mode Off }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile GAMMA LINEAR
			#pragma multi_compile FASTEST DENSITY BUMPED
			#include "UnityCG.cginc"
			#include "TOD_Base.cginc"

			uniform sampler2D _MainTex;
			uniform sampler2D _BumpMap;

			struct v2f {
				float4 position : SV_POSITION;
				float4 color    : TEXCOORD0;
				float4 cloudUV  : TEXCOORD1;
#if BUMPED
				float3 sunDir   : TEXCOORD2;
				float3 moonDir  : TEXCOORD3;
#endif
			};

			v2f vert(appdata_tan v) {
				v2f o;

				o.position = TOD_TRANSFORM_VERT(v.vertex);

				// Vertex position and uv coordinates
				float3 vertnorm = normalize(v.vertex.xyz);
				float2 vertuv   = vertnorm.xz / pow(vertnorm.y + 0.1, 0.75);
				float  vertfade = saturate(100 * vertnorm.y * vertnorm.y);

				// Base color
				o.color.rgb = TOD_CloudColor;
				o.color.a   = TOD_CloudDensity * vertfade;

#if FASTEST
				// UVs
				o.cloudUV.xy = (vertuv + TOD_CloudUV.xy) / TOD_CloudScale.xy;
				o.cloudUV.z  = TOD_CloudSharpness * 0.15 - max(0, 1-TOD_CloudSharpness) * 0.3;
				o.cloudUV.w  = 0;
#else
				// UVs
				o.cloudUV.xy = (vertuv + TOD_CloudUV.xy) / TOD_CloudScale.xy;
				o.cloudUV.zw = (vertuv + TOD_CloudUV.zw) / TOD_CloudScale.zw;
#endif

#if BUMPED
				// Light directions
				TANGENT_SPACE_ROTATION;
				o.sunDir  = normalize(mul(rotation, TOD_LocalSunDirection));
				o.moonDir = normalize(mul(rotation, TOD_LocalMoonDirection));
#else
				// Apply shading
				o.color.rgb += (0.5 + 0.5 * dot(v.normal, TOD_LocalSunDirection))  * TOD_SunCloudColor;
				o.color.rgb += (0.5 + 0.5 * dot(v.normal, TOD_LocalMoonDirection)) * TOD_MoonCloudColor;

#if GAMMA
				// Adjust output color
				o.color.rgb = TOD_LINEAR2GAMMA(o.color.rgb);
#endif
#endif

				return o;
			}

			half4 frag(v2f i) : COLOR {
				half4 color = i.color;

#if FASTEST
				// Sample texture
				half4 noise = tex2D(_MainTex, i.cloudUV.xy).r;
				half a = (noise - i.cloudUV.z);
#else
				// Sample texture
				half noise1 = tex2D(_MainTex, i.cloudUV.xy).r;
				half noise2 = tex2D(_MainTex, i.cloudUV.zw).g;
				half noise3 = tex2D(_MainTex, i.cloudUV.zy).b;

				half layer1 = noise3;
				half layer2 = noise1 * noise2;

				half a = pow(lerp(layer1, layer2, layer2), TOD_CloudSharpness);
#endif

				// Apply texture
				color.a *= saturate(a);

#if BUMPED
				// Sample normals
				half3 cloudNormal = UnpackNormal(tex2D(_BumpMap, i.cloudUV.xy));

				// Apply shading
				color.rgb += (0.5 + 0.5 * dot(cloudNormal, i.sunDir))  * TOD_SunCloudColor;
				color.rgb += (0.5 + 0.5 * dot(cloudNormal, i.moonDir)) * TOD_MoonCloudColor;

#if GAMMA
				// Adjust output color
				color.rgb = TOD_LINEAR2GAMMA(color.rgb);
#endif
#endif

				return color;
			}

			ENDCG
		}
	}

	Fallback Off
}
