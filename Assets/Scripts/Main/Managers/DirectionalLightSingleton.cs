using UnityEngine;
using System.Collections;

public enum DirectionalLightType
{
	Environment = 0,
	Shadow = 1,
}


public class DirectionalLightSingleton : GroupObjectSingleton<DirectionalLightSingleton>
{
	public DirectionalLightType type;
	public float interpolateTime = 10;

	void Awake()
	{
		OnConstruct(type.ToString());
	}

	protected override void OnInstanceExist (string key)
	{	
		//disable the light, but keep the setting;		
		this.gameObject.SetActive(false);
	}

	void OnDestroy()
	{
		OnDestruct(type.ToString());
	}



	public void ActivateLight()
	{
		if(shared(type.ToString()) == null) return;

		if(shared(type.ToString()) == this) return;

		//activate this light
		this.gameObject.SetActive(true);

		//disable current active light
		shared(type.ToString()).gameObject.SetActive(false);


		//interpolate from current light to THIS light
		InterpolateFrom( shared(type.ToString()).GetComponent<Light>() , interpolateTime);

		//Register this light as active light
		this.OnInit(type.ToString());

	}

	public void InterpolateFrom(Light fromSetting, float transitionTime)
	{	
		StartCoroutine( Interpolate(fromSetting,transitionTime) );
	}

	IEnumerator Interpolate(Light fromLightSetting,float transitiontime)
	{
		Light myLight= this.GetComponent<Light>();
		float percentage = 0;
		float startTime = Time.time;
		float duration = transitiontime;

		//only ROTATION, COLOR, INTENSITY, BOUNCE INTENSITY
		Vector3 fromAnglePos = fromLightSetting.transform.eulerAngles;
		Vector3 toAnglePos = myLight.transform.eulerAngles;

		Color FromColor = fromLightSetting.color;
		Color ToColor = myLight.color;

		float fromIntensity = fromLightSetting.intensity;
		float toIntensity = myLight.intensity;

		float fromBIntensity = fromLightSetting.bounceIntensity;
		float toBIntensity = myLight.bounceIntensity;


		while(percentage <1.0f)
		{
			percentage = Mathf.Clamp01((Time.time - startTime) / duration);

			// Rotation Interpolation
			myLight.transform.eulerAngles = Vector3.Slerp(fromAnglePos,toAnglePos,percentage);
			// or Using lerp angle 1 by 1

			// Color Interpolation
			myLight.color = Color.Lerp(FromColor,ToColor,percentage);

			// intensity
			myLight.intensity = Mathf.Lerp (fromIntensity,toIntensity,percentage);

			// bounce intensity
			myLight.bounceIntensity = Mathf.Lerp (fromBIntensity,toBIntensity,percentage);

			yield return new WaitForEndOfFrame();	
		}

		yield return new WaitForEndOfFrame();

	}



}
