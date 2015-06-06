using UnityEngine;

/// Component manager class.
///
/// Component of the main camera of the scene.

[ExecuteInEditMode]
public class TOD_Components : MonoBehaviour
{
	/// Sun game object reference.
	public GameObject Sun = null;

	/// Moon game object reference.
	public GameObject Moon = null;

	/// Atmosphere game object reference.
	public GameObject Atmosphere = null;

	/// Clear game object reference.
	public GameObject Clear = null;

	/// Clouds game object reference.
	public GameObject Clouds = null;

	/// Space game object reference.
	public GameObject Space = null;

	/// Light game object reference.
	public GameObject Light = null;

	/// Projector game object reference.
	public GameObject Projector = null;

	/// Billboards game object reference.
	public GameObject Billboards = null;

	/// Transform component of the sky dome game object.
	public Transform DomeTransform { get; set; }

	/// Transform component of the sun game object.
	public Transform SunTransform { get; set; }

	/// Transform component of the moon game object.
	public Transform MoonTransform { get; set; }

	/// Transform component of the light source game object.
	public Transform LightTransform { get; set; }

	/// Transform component of the space game object.
	public Transform SpaceTransform { get; set; }

	/// Renderer component of the space game object.
	public Renderer SpaceRenderer { get; set; }

	/// Renderer component of the atmosphere game object.
	public Renderer AtmosphereRenderer { get; set; }

	/// Renderer component of the clear game object.
	public Renderer ClearRenderer { get; set; }

	/// Renderer component of the cloud game object.
	public Renderer CloudRenderer { get; set; }

	/// Renderer component of the sun game object.
	public Renderer SunRenderer { get; set; }

	/// Renderer component of the moon game object.
	public Renderer MoonRenderer { get; set; }

	/// MeshFilter component of the space game object.
	public MeshFilter SpaceMeshFilter { get; set; }

	/// MeshFilter component of the atmosphere game object.
	public MeshFilter AtmosphereMeshFilter { get; set; }

	/// MeshFilter component of the clear game object.
	public MeshFilter ClearMeshFilter { get; set; }

	/// MeshFilter component of the cloud game object.
	public MeshFilter CloudMeshFilter { get; set; }

	/// MeshFilter component of the sun game object.
	public MeshFilter SunMeshFilter { get; set; }

	/// MeshFilter component of the moon game object.
	public MeshFilter MoonMeshFilter { get; set; }

	/// Main material of the space game object.
	public Material SpaceMaterial { get; set; }

	/// Main material of the atmosphere game object.
	public Material AtmosphereMaterial { get; set; }

	/// Main material of the clear game object.
	public Material ClearMaterial { get; set; }

	/// Main material of the cloud game object.
	public Material CloudMaterial { get; set; }

	/// Main material of the sun game object.
	public Material SunMaterial { get; set; }

	/// Main material of the moon game object.
	public Material MoonMaterial { get; set; }

	/// Main material of the projector game object.
	public Material ShadowMaterial { get; set; }

	/// Light component of the light source game object.
	public Light LightSource { get; set; }

	/// Projector component of the shadow projector game object.
	public Projector ShadowProjector { get; set; }

	/// Sky component of the sky dome game object.
	public TOD_Sky Sky { get; set; }

	/// Animation component of the sky dome game object.
	public TOD_Animation Animation { get; set; }

	/// Time component of the sky dome game object.
	public TOD_Time Time { get; set; }

	/// Weather component of the sky dome game object.
	public TOD_Weather Weather { get; set; }

	/// Main component of the camera game object.
	public TOD_Camera Camera { get; set; }

	/// God ray component of the camera game object.
	public TOD_Rays Rays { get; set; }

	/// Scattering component of the camera game object.
	public TOD_Scattering Scattering { get; set; }

	/// Initializes all component references.
	public void Initialize()
	{
		DomeTransform = GetComponent<Transform>();

		Sky       = GetComponent<TOD_Sky>();
		Animation = GetComponent<TOD_Animation>();
		Time      = GetComponent<TOD_Time>();
		Weather   = GetComponent<TOD_Weather>();

		if (Space)
		{
			SpaceTransform  = Space.GetComponent<Transform>();
			SpaceRenderer   = Space.GetComponent<Renderer>();
			SpaceMaterial   = SpaceRenderer.sharedMaterial;
			SpaceMeshFilter = Space.GetComponent<MeshFilter>();
		}
		else
		{
			Debug.LogError("Space reference not set.");
		}

		if (Atmosphere)
		{
			AtmosphereRenderer   = Atmosphere.GetComponent<Renderer>();
			AtmosphereMaterial   = AtmosphereRenderer.sharedMaterial;
			AtmosphereMeshFilter = Atmosphere.GetComponent<MeshFilter>();
		}
		else
		{
			Debug.LogError("Atmosphere reference not set.");
		}

		if (Clear)
		{
			ClearRenderer   = Clear.GetComponent<Renderer>();
			ClearMaterial   = ClearRenderer.sharedMaterial;
			ClearMeshFilter = Clear.GetComponent<MeshFilter>();
		}
		else
		{
			Debug.LogError("Clear reference not set.");
		}

		if (Clouds)
		{
			CloudRenderer   = Clouds.GetComponent<Renderer>();
			CloudMaterial   = CloudRenderer.sharedMaterial;
			CloudMeshFilter = Clouds.GetComponent<MeshFilter>();
		}
		else
		{
			Debug.LogError("Clouds reference not set.");
		}

		if (Projector)
		{
			ShadowProjector = Projector.GetComponent<Projector>();
			ShadowMaterial  = ShadowProjector.material;
		}
		else
		{
			Debug.LogError("Projector reference not set.");
		}

		if (Light)
		{
			LightTransform = Light.GetComponent<Transform>();
			LightSource    = Light.GetComponent<Light>();
		}
		else
		{
			Debug.LogError("Light reference not set.");
		}

		if (Sun)
		{
			SunTransform  = Sun.GetComponent<Transform>();
			SunRenderer   = Sun.GetComponent<Renderer>();
			SunMaterial   = SunRenderer.sharedMaterial;
			SunMeshFilter = Sun.GetComponent<MeshFilter>();
		}
		else
		{
			Debug.LogError("Sun reference not set.");
		}

		if (Moon)
		{
			MoonTransform  = Moon.GetComponent<Transform>();
			MoonRenderer   = Moon.GetComponent<Renderer>();
			MoonMaterial   = MoonRenderer.sharedMaterial;
			MoonMeshFilter = Moon.GetComponent<MeshFilter>();
		}
		else
		{
			Debug.LogError("Moon reference not set.");
		}

		if (Billboards)
		{
			// Intentionally left empty
		}
		else
		{
			Debug.LogError("Billboards reference not set.");
		}
	}
}
