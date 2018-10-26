using UnityEngine;
using System.Collections;

public class ShadeHoleVFXHandler : MonoBehaviour {

	public float speed = 0.1f;
	public SpriteRendererHandler fogSprite;
	public ShaderAnimationHandler[] shaderObjects;


	public void SetVFXEnable (bool value)
	{
		foreach (ShaderAnimationHandler item in shaderObjects) {
			if(item.gameObject.activeSelf)
				item.TriggerActivation (value,speed);
		}

		if(fogSprite !=null)
			fogSprite.TriggerColorActivation(value,speed);

	}
}
