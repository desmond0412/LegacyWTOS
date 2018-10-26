using UnityEngine;
using System.Collections;

public class L02TriggerLookBack : MonoBehaviour {

	float delayLookback = 1f;

	void OnTriggerEnter(Collider col){
		LevController levC = MainObjectSingleton.shared (MainObjectType.Player).GetComponent<LevController> ();
		if (col.CompareTag ("Player") && levC.GetComponentInChildren<TorchUsable>() == null) {
			levC.lookBack ();
			StartCoroutine (StopLookback (levC));
		}
	}

	IEnumerator StopLookback(LevController lev){
		float waktu = delayLookback;
		while (lev.facingDirection == LevController.Direction.Right && lev.isGrounded && !lev.isJumping && lev.levAnimator.GetFloat("horizontalSpeed") > 0.01f && waktu > 0f) {
			waktu -= Time.deltaTime;
			yield return null;
		}
		lev.stopLookBack ();
	}
}
