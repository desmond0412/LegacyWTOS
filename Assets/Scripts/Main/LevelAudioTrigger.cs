using UnityEngine;
using System.Collections;

public class LevelAudioTrigger : MonoBehaviour {

	bool isTriggered = false;	
	bool isLoadOpening;
	void Awake()
	{
		isLoadOpening = GameDataManager.shared ().LOCAL_IsLoadOpening;
	}

	void OnTriggerEnter(Collider other)
	{
		if(isTriggered) return;
		if (other.CompareTag("Player") && isLoadOpening ) {
			isTriggered = true;
			PlayMainMenuBGM();
		}
		else if (other.CompareTag("Player")) {
			isTriggered = true;
			PlayLevelBGM();
		}
	}

	public void PlayMainMenuBGM()
	{
		GlobalAudioSingleton.shared(GlobalAudioType.MainMenuMusic).Play();
	}

	public void PlayLevelBGM()
	{
		GlobalAudioSingleton.shared(GlobalAudioType.InGameMusic).Play();
		GlobalAudioSingleton.shared(GlobalAudioType.Ambience).Play(true);
	}
}
