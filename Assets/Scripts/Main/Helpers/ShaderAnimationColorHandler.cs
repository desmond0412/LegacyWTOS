using UnityEngine;
using System.Collections;



public class ShaderAnimationColorHandler : ShaderAnimationHandler {

	public Color clearValue = Color.clear; 
	Color originValue;
	Color targetValue;

	void Start () {
		if( meshMat == null ) return;
			originValue = meshMat.GetColor(propertiesName);
	}

	void Update () {
		if( meshMat == null ) return;

		if(isFading)
		{
			float percentage = Mathf.Clamp01( (Time.time-startTime) * fadingSpeed);
			meshMat.SetColor(propertiesName,Color.Lerp(meshMat.GetColor(propertiesName),targetValue,percentage));
			if( meshMat.GetColor(propertiesName) == targetValue )
			{
				isFading = false;
				meshMat.SetColor(propertiesName,targetValue);
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
