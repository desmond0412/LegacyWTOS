using UnityEngine;
using System.Collections;



public class ShaderAnimationFloatHandler : ShaderAnimationHandler {

	public float clearValue = 0; 
	public float originValue;
	float targetValue;

	void Start () {
		if( meshMat == null ) return;
			originValue = meshMat.GetFloat(propertiesName);
	}

	void Update () {
		if( meshMat == null ) return;

		if(isFading)
		{
			float percentage = Mathf.Clamp01( (Time.time-startTime) * fadingSpeed);

			meshMat.SetFloat(propertiesName,Mathf.Lerp(meshMat.GetFloat(propertiesName),targetValue,percentage));

			if( Mathf.Abs(targetValue - meshMat.GetFloat(propertiesName) ) < 0.01f )
			{
				isFading = false;
				meshMat.SetFloat(propertiesName,targetValue);
				fadingSpeed = defaultFadingSpeed;
			}
		}	
	}

	public override void TriggerActivation(bool isOn, float speed = -1)
	{
		base.TriggerActivation(isOn,speed);
		targetValue = isOn ? originValue : clearValue;

	}

}
