using UnityEngine;

/// Material and mesh wrapper class.
///
/// Component of the sky dome parent game object.

public class TOD_Resources : MonoBehaviour
{
	public Mesh Quad;

	public Mesh SphereHigh;
	public Mesh SphereMedium;
	public Mesh SphereLow;

	public Mesh IcosphereHigh;
	public Mesh IcosphereMedium;
	public Mesh IcosphereLow;

	public Mesh HalfIcosphereHigh;
	public Mesh HalfIcosphereMedium;
	public Mesh HalfIcosphereLow;

	public Material CloudMaterial;
	public Material ShadowMaterial;
	public Material BillboardMaterial;
	public Material SpaceMaterial;
	public Material AtmosphereMaterial;
	public Material SunMaterial;
	public Material MoonMaterial;
	public Material ClearMaterial;
	public Material SkyboxMaterial;

	public int ID_SunSkyColor { get; private set; }
	public int ID_MoonSkyColor { get; private set; }

	public int ID_SunCloudColor { get; private set; }
	public int ID_MoonCloudColor { get; private set; }

	public int ID_SunMeshColor { get; private set; }
	public int ID_MoonMeshColor { get; private set; }

	public int ID_CloudColor { get; private set; }
	public int ID_GroundColor { get; private set; }
	public int ID_AmbientColor { get; private set; }
	public int ID_MoonHaloColor { get; private set; }

	public int ID_SunDirection { get; private set; }
	public int ID_MoonDirection { get; private set; }
	public int ID_LightDirection { get; private set; }

	public int ID_LocalSunDirection { get; private set; }
	public int ID_LocalMoonDirection { get; private set; }
	public int ID_LocalLightDirection { get; private set; }

	public int ID_Contrast { get; private set; }
	public int ID_Brightness { get; private set; }
	public int ID_Fogginess { get; private set; }
	public int ID_Directionality { get; private set; }
	public int ID_MoonHaloPower { get; private set; }

	public int ID_CloudDensity { get; private set; }
	public int ID_CloudSharpness { get; private set; }
	public int ID_CloudShadow { get; private set; }
	public int ID_CloudScale { get; private set; }
	public int ID_CloudUV { get; private set; }

	public int ID_SpaceTiling { get; private set; }
	public int ID_SpaceBrightness { get; private set; }

	public int ID_SunMeshContrast { get; private set; }
	public int ID_SunMeshBrightness { get; private set; }

	public int ID_MoonMeshContrast { get; private set; }
	public int ID_MoonMeshBrightness { get; private set; }

	public int ID_kBetaMie { get; private set; }
	public int ID_kSun { get; private set; }
	public int ID_k4PI { get; private set; }
	public int ID_kRadius { get; private set; }
	public int ID_kScale { get; private set; }

	public int ID_World2Sky { get; private set; }
	public int ID_Sky2World { get; private set; }

	/// Initializes all resource references.
	public void Initialize()
	{
		ID_SunSkyColor = Shader.PropertyToID("TOD_SunSkyColor");
		ID_MoonSkyColor = Shader.PropertyToID("TOD_MoonSkyColor");

		ID_SunCloudColor = Shader.PropertyToID("TOD_SunCloudColor");
		ID_MoonCloudColor = Shader.PropertyToID("TOD_MoonCloudColor");

		ID_SunMeshColor = Shader.PropertyToID("TOD_SunMeshColor");
		ID_MoonMeshColor = Shader.PropertyToID("TOD_MoonMeshColor");

		ID_CloudColor = Shader.PropertyToID("TOD_CloudColor");
		ID_GroundColor = Shader.PropertyToID("TOD_GroundColor");
		ID_AmbientColor = Shader.PropertyToID("TOD_AmbientColor");
		ID_MoonHaloColor = Shader.PropertyToID("TOD_MoonHaloColor");

		ID_SunDirection = Shader.PropertyToID("TOD_SunDirection");
		ID_MoonDirection = Shader.PropertyToID("TOD_MoonDirection");
		ID_LightDirection = Shader.PropertyToID("TOD_LightDirection");

		ID_LocalSunDirection = Shader.PropertyToID("TOD_LocalSunDirection");
		ID_LocalMoonDirection = Shader.PropertyToID("TOD_LocalMoonDirection");
		ID_LocalLightDirection = Shader.PropertyToID("TOD_LocalLightDirection");

		ID_Contrast = Shader.PropertyToID("TOD_Contrast");
		ID_Brightness = Shader.PropertyToID("TOD_Brightness");
		ID_Fogginess = Shader.PropertyToID("TOD_Fogginess");
		ID_Directionality = Shader.PropertyToID("TOD_Directionality");
		ID_MoonHaloPower = Shader.PropertyToID("TOD_MoonHaloPower");

		ID_CloudDensity = Shader.PropertyToID("TOD_CloudDensity");
		ID_CloudSharpness = Shader.PropertyToID("TOD_CloudSharpness");
		ID_CloudShadow = Shader.PropertyToID("TOD_CloudShadow");
		ID_CloudScale = Shader.PropertyToID("TOD_CloudScale");
		ID_CloudUV = Shader.PropertyToID("TOD_CloudUV");

		ID_SpaceTiling = Shader.PropertyToID("TOD_SpaceTiling");
		ID_SpaceBrightness = Shader.PropertyToID("TOD_SpaceBrightness");

		ID_SunMeshContrast = Shader.PropertyToID("TOD_SunMeshContrast");
		ID_SunMeshBrightness = Shader.PropertyToID("TOD_SunMeshBrightness");

		ID_MoonMeshContrast = Shader.PropertyToID("TOD_MoonMeshContrast");
		ID_MoonMeshBrightness = Shader.PropertyToID("TOD_MoonMeshBrightness");

		ID_kBetaMie = Shader.PropertyToID("TOD_kBetaMie");
		ID_kSun = Shader.PropertyToID("TOD_kSun");
		ID_k4PI = Shader.PropertyToID("TOD_k4PI");
		ID_kRadius = Shader.PropertyToID("TOD_kRadius");
		ID_kScale = Shader.PropertyToID("TOD_kScale");

		ID_World2Sky = Shader.PropertyToID("TOD_World2Sky");
		ID_Sky2World = Shader.PropertyToID("TOD_Sky2World");
	}

	// Creates a quad.
	// \param minUV Minimum uv values.
	// \param maxUV Maximum uv values.
	public static Mesh CreateQuad(Vector2 minUV, Vector2 maxUV)
	{
		return new Mesh()
		{
			name = "Quad " + minUV + " " + maxUV,
			vertices = new Vector3[]
			{
				new Vector3(-1, -1, 0),
				new Vector3(-1, +1, 0),
				new Vector3(+1, +1, 0),
				new Vector3(+1, -1, 0)
			},
			uv = new Vector2[]
			{
				new Vector2(minUV.x, minUV.y),
				new Vector2(minUV.x, maxUV.y),
				new Vector2(maxUV.x, maxUV.y),
				new Vector2(maxUV.x, minUV.y)
			},
			triangles = new int[]
			{
				0, 3, 2,
				0, 2, 1
			},
			normals = new Vector3[]
			{
				new Vector3(0, 0, 1),
				new Vector3(0, 0, 1),
				new Vector3(0, 0, 1),
				new Vector3(0, 0, 1)
			},
			tangents = new Vector4[]
			{
				new Vector4(1, 0, 0, 1),
				new Vector4(1, 0, 0, 1),
				new Vector4(1, 0, 0, 1),
				new Vector4(1, 0, 0, 1)
			}
		};
	}
}
