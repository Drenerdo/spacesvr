using UnityEngine;

/// Atmospheric scattering and aerial perspective camera component.

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Time of Day/Camera Scattering")]
public class TOD_Scattering : TOD_ImageEffect
{
	public Shader ScatteringShader = null;

	public Texture2D DitheringTexture = null;

	[Range(0f, 1f)] public float GlobalDensity = 0.001f;
	[Range(0f, 1f)] public float HeightFalloff = 0.001f;
	public float ZeroLevel = 0.0f;

	private Material scatteringMaterial = null;

	protected void OnEnable()
	{
		if (!ScatteringShader) ScatteringShader = Shader.Find("Hidden/Time of Day/Scattering");

		scatteringMaterial = CreateMaterial(ScatteringShader);
	}

	protected void OnDisable()
	{
		if (scatteringMaterial) DestroyImmediate(scatteringMaterial);
	}

	protected void OnPreCull()
	{
		if (sky && sky.Initialized) sky.Components.AtmosphereRenderer.enabled = false;
	}

	protected void OnPostRender()
	{
		if (sky && sky.Initialized) sky.Components.AtmosphereRenderer.enabled = true;
	}

	[ImageEffectOpaque]
	protected void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!CheckSupport(true, true))
		{
			Graphics.Blit(source, destination);
			return;
		}

		sky.Components.Scattering = this;

		float CAMERA_NEAR = cam.nearClipPlane;
		float CAMERA_FAR = cam.farClipPlane;
		float CAMERA_FOV = cam.fieldOfView;
		float CAMERA_ASPECT_RATIO = cam.aspect;

		Vector3 position = cam.transform.position;
		Vector3 forward = cam.transform.forward;
		Vector3 right = cam.transform.right;
		Vector3 up = cam.transform.up;

		Matrix4x4 frustumCorners = Matrix4x4.identity;

		float fovWHalf = CAMERA_FOV * 0.5f;

		Vector3 toRight = right * CAMERA_NEAR * Mathf.Tan (fovWHalf * Mathf.Deg2Rad) * CAMERA_ASPECT_RATIO;
		Vector3 toTop = up * CAMERA_NEAR * Mathf.Tan (fovWHalf * Mathf.Deg2Rad);

		Vector3 topLeft = (forward * CAMERA_NEAR - toRight + toTop);
		float CAMERA_SCALE = topLeft.magnitude * CAMERA_FAR/CAMERA_NEAR;

		topLeft.Normalize();
		topLeft *= CAMERA_SCALE;

		Vector3 topRight = (forward * CAMERA_NEAR + toRight + toTop);
		topRight.Normalize();
		topRight *= CAMERA_SCALE;

		Vector3 bottomRight = (forward * CAMERA_NEAR + toRight - toTop);
		bottomRight.Normalize();
		bottomRight *= CAMERA_SCALE;

		Vector3 bottomLeft = (forward * CAMERA_NEAR - toRight - toTop);
		bottomLeft.Normalize();
		bottomLeft *= CAMERA_SCALE;

		frustumCorners.SetRow (0, topLeft);
		frustumCorners.SetRow (1, topRight);
		frustumCorners.SetRow (2, bottomRight);
		frustumCorners.SetRow (3, bottomLeft);

		float heightFalloff = HeightFalloff;
		float heightDensity = Mathf.Exp(-heightFalloff * (position.y - ZeroLevel));
		float globalDensity = GlobalDensity;

		scatteringMaterial.SetMatrix("_FrustumCornersWS", frustumCorners);
		scatteringMaterial.SetTexture("_DitheringTexture", DitheringTexture);
		scatteringMaterial.SetVector("_Density", new Vector4(heightFalloff, heightDensity, globalDensity, 0));

		CustomBlit(source, destination, scatteringMaterial);
	}
}
