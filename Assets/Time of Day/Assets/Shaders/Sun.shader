Shader "Time of Day/Sun"
{
	Properties
	{
	}

	SubShader
	{
		Tags
		{
			"Queue"="Background+20"
			"RenderType"="Background"
			"IgnoreProjector"="True"
		}

		Pass
		{
			Cull Back
			ZWrite Off
			ZTest LEqual
			Blend One One
			Fog { Mode Off }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "TOD_Base.cginc"

			struct v2f {
				float4 position : SV_POSITION;
				half3  tex      : TEXCOORD0;
			};

			v2f vert(appdata_base v) {
				v2f o;

				o.position = TOD_TRANSFORM_VERT(v.vertex);

				float3 skyPos = mul(TOD_World2Sky, mul(_Object2World, v.vertex)).xyz;

				o.tex.xy = 2.0 * v.texcoord - 1.0;
				o.tex.z  = skyPos.y * 25;

				return o;
			}

			half4 frag(v2f i) : COLOR {
				half4 color = half4(TOD_SunMeshColor, 1);

				half  dist  = length(i.tex.xy);
				half  spot  = 1.0 - smoothstep(0.5, 1.0, dist);
				half  alpha = saturate(i.tex.z) * TOD_SunMeshBrightness * pow(spot, TOD_SunMeshContrast);

				color.rgb *= alpha;

				return color;
			}

			ENDCG
		}
	}

	Fallback Off
}
