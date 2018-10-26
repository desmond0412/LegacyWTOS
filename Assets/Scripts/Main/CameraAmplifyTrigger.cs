using UnityEngine;
using System.Collections;

public class CameraAmplifyTrigger : MonoBehaviour {

	public Texture amplifyBlendTexture;
	public float duration;

	private bool isTriggered = false;
	void OnTriggerEnter(Collider other)
	{

		if(isTriggered) return;
		if(other.CompareTag("Player"))
		{
			isTriggered = true;
			MainObjectSingleton.shared(MainObjectType.Camera).GetComponent<AmplifyColorEffect>().BlendTo(amplifyBlendTexture,duration,null);
		}

	}


}
