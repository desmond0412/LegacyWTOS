using UnityEngine;
using System.Collections;

public class TorchTrigger : MonoBehaviour {

	public GameObject burnedRope;
	public TriggerCinematic triggerCinematic;
	public PortalTrigger triggerNextScene;
	public Atataraina rainatar;

	LevController levC;
	void OnTriggerEnter(Collider col){
		TorchUsable temp = col.GetComponent<TorchUsable> ();
		if (temp != null) {
			GetComponent<Collider> ().enabled = false;

			if (rainatar) {
				if (rainatar.isLevDied)
					return;
				rainatar.isStillHauntingLev = false;
			}

			levC = MainObjectSingleton.shared (MainObjectType.Player).GetComponent<LevController> ();
			levC.setInputEnable(false);
			levC.stopMoving();
			levC.OnCustomAnimationFinished += ForceLevMove;

			burnedRope.GetComponentsInChildren<BurnedRopeParticle> ().ForEach (t => t.LightMyFire ());
		}
	}

	void ForceLevMove(GameObject sender){
		levC.setInputEnable(false);
		levC.stopMoving();
		StartCoroutine (MoveLev (levC));
		if (levC != null) {
			levC.OnCustomAnimationFinished -= ForceLevMove;
		}
	}

	IEnumerator MoveLev(LevController lev){
		while (lev.transform.position.x < transform.position.x + 2.6f){
			lev.moveRight ();
			yield return null;
		}
		float waktu = 1f;
		lev.facingDirection = LevController.Direction.Left;
		while (waktu > 0f) {
			lev.stopMoving ();
			waktu -= Time.deltaTime;
			yield return null;
		}
		triggerCinematic.TriggerThis (0f);
		TorchPickable.StopAllTorches();
		yield return new WaitForSeconds(1);
		rainatar.rainaAnim.speed = 0 ;

	}
}
