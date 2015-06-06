using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class RenderAtWeather : MonoBehaviour
{
	public TOD_WeatherType type;

	private Renderer rendererComponent;

	protected void Start()
	{
		rendererComponent = GetComponent<Renderer>();

		rendererComponent.enabled = (TOD_Sky.Instance.Components.Weather.Weather == type);
	}

	protected void Update()
	{
		rendererComponent.enabled = (TOD_Sky.Instance.Components.Weather.Weather == type);
	}
}
