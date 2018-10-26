using UnityEngine;
using System.Collections;

public class AkEventAnimationHandler : MonoBehaviour {

	public GameObject eventObject;

	public void postAkEvent (string eventName) {
		if (eventObject) {
			AkSoundEngine.PostEvent (eventName, eventObject);
		}
		else {
			AkSoundEngine.PostEvent (eventName, gameObject);
		}
	}
}
