using UnityEngine;
using System.Collections;

public class L02ClueTrigger : MonoBehaviour {
	public Atataraina atataraina;
	public GameObject plank;

	//parameters
	public float delay = 3f;
	public float timeShaking = .3f;
	public float shakeAmount = .15f;

	float delayLookback = .5f;

	iTween tweenInstance;
	Coroutine coroutineInstance;
	float startY;

	void OnTriggerEnter(Collider other)
	{
		LevController levC = MainObjectSingleton.shared (MainObjectType.Player).GetComponent<LevController> ();
		if(other.CompareTag("Player") && levC.GetComponentInChildren<TorchUsable>() != null)
		{
			levC.lookBack ();
			StartCoroutine (StopLookback (levC));
//			startY = plank.transform.position.y;
//			CreateItween ();
		}
	}

	IEnumerator StopLookback(LevController lev){
		float waktu = delayLookback;
		while (lev.facingDirection == LevController.Direction.Right && lev.isGrounded && !lev.isJumping && lev.levAnimator.GetFloat("horizontalSpeed") > 0.01f  && waktu > 0f) {
			waktu -= Time.deltaTime;
			yield return null;
		}
		lev.stopLookBack ();
	}

	void CreateItween(){
		if (tweenInstance == null && !atataraina.isLevDied) {
			iTween.ShakePosition (plank, iTween.Hash ("y", shakeAmount,
				"time", timeShaking,
				"easetype", iTween.EaseType.linear,
				"oncomplete","StartDelay",
				"oncompletetarget",this.gameObject));
			tweenInstance = plank.GetComponent<iTween> ();
		}
	}

	void StartDelay(){
		coroutineInstance = StartCoroutine (ItweenCoroutine ());
	}
	IEnumerator ItweenCoroutine(){
		yield return new WaitForSeconds (delay);
		CreateItween ();
	}

//	void OnTriggerExit(Collider other)
//	{
//		if(other.CompareTag("Player"))
//		{
//			if (tweenInstance != null) {
//				Destroy (tweenInstance);
//				plank.transform.position = new Vector3 (plank.transform.position.x,startY,plank.transform.position.z);
//			}
//			if (coroutineInstance != null) {
//				StopCoroutine (coroutineInstance);
//				coroutineInstance = null;
//			}
//		}
//	}
}
