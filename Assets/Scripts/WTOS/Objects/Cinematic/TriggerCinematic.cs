using UnityEngine;
using System.Collections;

public class TriggerCinematic : MonoBehaviour {

	public CinematicCall cinematic;
	public PortalTrigger portal;
	public bool isUsingOnTrigger;

	bool hasTriggered = false;

	public void TriggerThis(float delayPlayMovie){
		hasTriggered = true;
		StartCoroutine (DelayPlay (delayPlayMovie));
	}
	IEnumerator DelayPlay(float delay){
		yield return new WaitForSeconds (delay);
		cinematic.PlayMovie ();
		cinematic.OnMovieFinish += NextScene;
		GlobalAudioSingleton.StopAll();
	}
		
	void OnTriggerEnter(Collider other)
	{
		if (!isUsingOnTrigger)
			return;
		if (!hasTriggered && other.CompareTag("Player")) //only player can trigger the cutscene
		{
			hasTriggered = true;
			cinematic.PlayMovie ();
			cinematic.OnMovieFinish += NextScene;
			GlobalAudioSingleton.StopAll();
		}
	}

	virtual public void NextScene(){
		if (portal != null)
			portal.Portal ();
		cinematic.OnMovieFinish -= NextScene;
	}
}
