#ifndef TOD_SCATTERING_INCLUDED
#define TOD_SCATTERING_INCLUDED

#ifndef TOD_SAMPLES
#define TOD_SAMPLES 2
#endif

#ifndef TOD_DIRECTIONAL
#define TOD_DIRECTIONAL 1
#endif

inline float Scale(float inCos)
{
	float x = 1.0 - inCos;
	return 0.25 * exp(-0.00287 + x*(0.459 + x*(3.83 + x*(-6.80 + x*5.25))));
}

inline float MiePhase(float eyeCos, float eyeCos2)
{
	return TOD_kBetaMie.x * (1.0 + eyeCos2) / pow(TOD_kBetaMie.y + TOD_kBetaMie.z * eyeCos, 1.5);
}

inline float RayleighPhase(float eyeCos2)
{
	return 0.75 + 0.75 * eyeCos2;
}

inline float3 NightSkyColor(float3 dir)
{
	return lerp(TOD_MoonSkyColor, float3(0, 0, 0), dir.y);
}

inline float3 MoonHaloColor(float3 dir)
{
	return TOD_MoonHaloColor * pow(max(0, dot(dir, TOD_LocalMoonDirection)), TOD_MoonHaloPower);
}

#if TOD_DIRECTIONAL
inline void ScatteringCoefficients(float3 dir, float depth, out float3 inscatter, out float3 outscatter)
#else
inline void ScatteringCoefficients(float3 dir, float depth, out float3 inscatter)
#endif
{
	dir.y = saturate(dir.y);

	float kInnerRadius  = TOD_kRadius.x;
	float kInnerRadius2 = TOD_kRadius.y;
	float kOuterRadius2 = TOD_kRadius.w;

	float kScale               = TOD_kScale.x * depth;
	float kScaleOverScaleDepth = TOD_kScale.z;
	float kCameraHeight        = TOD_kScale.w;

	float3 kKr4PI = TOD_k4PI.xyz;
	float  kKm4PI = TOD_k4PI.w;

	float3 kKrESun = TOD_kSun.xyz;
	float  kKmESun = TOD_kSun.w;

	// Current camera position
	float3 cameraPos = float3(0, kInnerRadius + kCameraHeight, 0);

	// Length of the atmosphere
	float far = sqrt(kOuterRadius2 + kInnerRadius2 * dir.y * dir.y - kInnerRadius2) - kInnerRadius * dir.y;

	// Ray starting position and its scattering offset
	float startDepth  = exp(kScaleOverScaleDepth * (-kCameraHeight));
	float startAngle  = dot(dir, cameraPos) / (kInnerRadius + kCameraHeight);
	float startOffset = startDepth * Scale(startAngle);

	// Scattering loop variables
	float  sampleLength = far / float(TOD_SAMPLES);
	float  scaledLength = sampleLength * kScale;
	float3 sampleRay    = dir * sampleLength;
	float3 samplePoint  = cameraPos + sampleRay * 0.5;

	float3 sunColor = float3(0.0, 0.0, 0.0);

	// Loop through the sample rays
	for (int i = 0; i < int(TOD_SAMPLES); i++)
	{
		float height    = length(samplePoint);
		float invHeight = 1.0 / height;

		float depth = exp(kScaleOverScaleDepth * (kInnerRadius - height));
		float atten = depth * scaledLength;

		float cameraAngle = dot(dir, samplePoint) * invHeight;
		float sunAngle    = dot(TOD_LocalSunDirection,  samplePoint) * invHeight;
		float sunScatter  = startOffset + depth * (Scale(sunAngle)  - Scale(cameraAngle));

		float3 sunAtten = exp(-sunScatter * (kKr4PI + kKm4PI));

		sunColor    += sunAtten * atten;
		samplePoint += sampleRay;
	}

	// Sun scattering
	inscatter = TOD_SunSkyColor * sunColor * kKrESun;

	#if TOD_DIRECTIONAL
	outscatter = TOD_SunSkyColor * sunColor * kKmESun;
	#endif
}

#if TOD_DIRECTIONAL
inline float4 ScatteringColor(float3 dir, float3 inscatter, float3 outscatter)
#else
inline float4 ScatteringColor(float3 dir, float3 inscatter)
#endif
{
	float3 resultColor = float3(0, 0, 0);

	// Ground factor
	float ground = smoothstep(0.0, 1.25, -dir.y);

	// Sun angle
	float sunCos  = dot(TOD_LocalSunDirection, dir);
	float sunCos2 = sunCos * sunCos;

	// Add sun light
	resultColor += RayleighPhase(sunCos2) * inscatter;

	#if TOD_DIRECTIONAL
	resultColor += MiePhase(sunCos, sunCos2) * outscatter;
	#endif

	// Add moon light
	resultColor += NightSkyColor(dir);

	#if TOD_DIRECTIONAL
	resultColor += MoonHaloColor(dir);
	#endif

	// Lerp to fog color
	resultColor = lerp(resultColor, TOD_CloudColor, TOD_Fogginess);

	// Lerp to ground color
	resultColor = lerp(resultColor, TOD_GroundColor, ground);

	// Adjust output color
	return float4(pow(resultColor * TOD_Brightness, TOD_Contrast), 1.0);
}

inline float4 ScatteringColor(float3 dir, float depth)
{
	#if TOD_DIRECTIONAL
	float3 inscatter, outscatter;
	ScatteringCoefficients(dir, depth, inscatter, outscatter);
	return ScatteringColor(dir, inscatter, outscatter);
	#else
	float3 inscatter;
	ScatteringCoefficients(dir, depth, inscatter);
	return ScatteringColor(dir, inscatter);
	#endif
}

#endif
