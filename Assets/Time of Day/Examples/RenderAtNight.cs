using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class RenderAtNight : MonoBehaviour
{
	private Renderer rendererComponent;

	protected void Start()
	{
		rendererComponent = GetComponent<Renderer>();

		rendererComponent.enabled = TOD_Sky.Instance.IsNight;
	}

	protected void Update()
	{
		rendererComponent.enabled = TOD_Sky.Instance.IsNight;
	}
}
