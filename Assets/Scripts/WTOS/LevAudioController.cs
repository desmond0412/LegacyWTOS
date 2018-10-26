using UnityEngine;
using System.Collections;

public class LevAudioController : MonoBehaviour {
	
	public void playStepSound () {
		AkSoundEngine.PostEvent ("lev_fs", gameObject);
	}

	public void playAimSound () {
		AkSoundEngine.PostEvent ("lev_aim", gameObject);
	}

	public void playDropDeadSound()	{
		AkSoundEngine.PostEvent ("lev_dropdead", gameObject);
	}

	public void playDrownSound(){
		AkSoundEngine.PostEvent ("lev_drown", gameObject);

	}
	public void playFallSound(){
		AkSoundEngine.PostEvent ("lev_fall", gameObject);
	}
//
//
//	void OnGUI()
//	{
//		if(GUILayout.Button("jato mati"))
//		{
//			playDropDeadSound();
//		}
//		if(GUILayout.Button("blup blup"))
//		{
//			playDrownSound();
//		}if(GUILayout.Button("fall"))
//		{
//			playFallSound();
//		}
//
//	}
}

