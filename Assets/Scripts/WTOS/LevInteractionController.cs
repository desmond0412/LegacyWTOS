using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;

public class LevInteractionController : MonoBehaviour {

	private LevController levController;
	public FullBodyBipedIK levFBBIK;
	public Transform pickHandle;
	private List <PickableObject> pickables;
	private List <UsableObject> usables;

	private PickableObject heldPickable;
	private bool isHoldingPickable;
	private UsableObject heldUsable;

	void Start () {
		levController = GetComponentInParent<LevController> ();
		pickables = new List<PickableObject> ();
		usables = new List<UsableObject> ();
	}

	void Update () {
		if (!levController.isInputEnabled || !levController.isGrounded) {
			return;
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			if (heldPickable) {
				drop ();
			}
			else {
				pick ();
			}
		}
		if (Input.GetKeyDown (KeyCode.F)) {
			use ();
		}
		if (isHoldingPickable && !heldPickable) {
			levController.stopCarry (0.5f);
			isHoldingPickable = false;
		}
	}

	void FixedUpdate () {
		for (int i=pickables.Count-1; i>=0; i--) {
			PickableObject p = pickables[i];
			if (p == null || !p.gameObject.activeInHierarchy) {
				pickables.Remove(p);
			}
		}
		for (int i=usables.Count-1; i>=0; i--) {
			UsableObject u = usables[i];
			if (u == null || !u.gameObject.activeInHierarchy) {
				usables.Remove(u);
			}
		}
	}

	#region PICK
	public void pick () {
		heldPickable = getFirstPickableObject ();
		if (heldPickable) {
			if (heldPickable.objectLayer == PickableObject.LayerObject.Back) {
				levController.turnToBackground ();
			}
			else if (heldPickable.objectLayer == PickableObject.LayerObject.Front) {
				levController.turnToForeground ();
			}
			levController.OnCustomAnimationFinished += handleOnPickFinished;
			levController.playCustomAnimation (heldPickable.pickAnimation);
            levController.stopMoving ();
            heldPickable.OnStartLocked();
			isHoldingPickable = true;

		}
	}

	void handleOnPickFinished (GameObject sender) {
		levController.startCarry (heldPickable.carryAnimation);
		levController.OnCustomAnimationFinished -= handleOnPickFinished;
		levController.turnToNormal ();
	}

	public void blendPickIK (float duration) {
		levFBBIK.solver.rightHandEffector.target = heldPickable.pickHandle;
		StartCoroutine (blendIKEffector (levFBBIK.solver.rightHandEffector, 1, duration));
	}

	public void grabPickedObject () {
		StartCoroutine (blendIKEffector (levFBBIK.solver.rightHandEffector, 0, 0.2f));
		StartCoroutine (grabPickedObjectCoroutine ());
	}

	IEnumerator grabPickedObjectCoroutine () {
		yield return new WaitForEndOfFrame ();
		heldPickable.PickFunction ();
		heldPickable.root.transform.SetParent (pickHandle);
		heldPickable.root.transform.rotation = pickHandle.rotation;
		heldPickable.root.transform.position = pickHandle.position + (heldPickable.root.transform.position - heldPickable.pickHandle.transform.position);
	}

	private PickableObject getFirstPickableObject () {
		float nearestDistSqr = float.MaxValue;
		PickableObject nearestObj = null;
		foreach (PickableObject pickable in pickables) {
			if (!pickable.isAccessible) continue;

			float distSqr = (pickable.pickHandle.transform.position - transform.position).sqrMagnitude;
			if (distSqr < nearestDistSqr) {
				if (pickable.objectLayer != PickableObject.LayerObject.Mid || levController.isInFront (pickable.pickHandle.transform.position)) {
					nearestDistSqr = distSqr;
					nearestObj = pickable;
				}
			}
		}
		return nearestObj;
	}
	#endregion

	#region DROP
	public void drop () {
		levController.OnCustomAnimationFinished += handleOnDropFinished;
		levController.playCustomAnimation (heldPickable.dropAnimation);
		levController.stopMoving ();
	}

	void handleOnDropFinished (GameObject sender) {
		levController.stopCarry (0);
		levController.OnCustomAnimationFinished -= handleOnDropFinished;
	}

	public void throwPickedObject (Vector3 handSpeed) {
		StartCoroutine (throwPickedObjectCoroutine (handSpeed));
	}

	IEnumerator throwPickedObjectCoroutine (Vector3 handSpeed) {
		yield return new WaitForEndOfFrame ();
		if (heldPickable) {
			heldPickable.root.transform.SetParent (null, false);
			heldPickable.root.transform.rotation = pickHandle.rotation;
			heldPickable.root.transform.position = pickHandle.position + (heldPickable.root.transform.position - heldPickable.pickHandle.transform.position);
            heldPickable.DropFunction (handSpeed * 10);
            heldPickable.OnEndLocked();
			heldPickable = null;
			isHoldingPickable = false;

		}
	}
	#endregion

	#region USE
	public void use () {
		if (heldPickable) {
			heldUsable = heldPickable.GetComponent<UsableObject> ();
		}
		else if (usables.Count > 0) {
			heldUsable = getFirstUsableObject ();
		}
		if (heldUsable) {			
			if (heldUsable.objectLayer == UsableObject.LayerObject.Back) {
				levController.turnToBackground ();
			}
			else if (heldUsable.objectLayer == UsableObject.LayerObject.Front) {
				levController.turnToForeground ();
			}
			levController.OnCustomAnimationFinished += handleOnUseFinished;
			levController.playCustomAnimation (heldUsable.useAnimation);
			levController.stopMoving ();
		}
	}

	void handleOnUseFinished (GameObject sender) {
		levController.OnCustomAnimationFinished -= handleOnUseFinished;
		levController.turnToNormal ();
		heldUsable = null;
	}

	public void blendUseIK (float duration) {
		levFBBIK.solver.rightHandEffector.target = heldUsable.useHandle;
		StartCoroutine (blendIKEffector (levFBBIK.solver.rightHandEffector, 1, duration));
	}
	
	public void stopUseIK () {
		StartCoroutine (blendIKEffector (levFBBIK.solver.rightHandEffector, 0, 0.2f));
	}

	public void useObject () {
		heldUsable.UseFunction ();
	}

	private UsableObject getFirstUsableObject () {
		float nearestDistSqr = float.MaxValue;
		UsableObject nearestObj = null;
		foreach (UsableObject usable in usables) {
			if (!usable.isAccessible) continue;

			if (!usable.GetComponent<PickableObject> ()) {
				float distSqr = (usable.useHandle.transform.position - transform.position).sqrMagnitude;
				if (distSqr < nearestDistSqr) {
					if (usable.objectLayer != UsableObject.LayerObject.Mid || levController.isInFront (usable.useHandle.transform.position)) {
						nearestDistSqr = distSqr;
						nearestObj = usable;
					}
				}
			}
		}
		return nearestObj;
	}
	#endregion

	IEnumerator blendIKEffector (IKEffector ikEffector, float targetWeight, float duration) {
		float currentWeight = ikEffector.positionWeight;
		float time = 0;
		float percentage = 0;
		while (percentage < 1) {
			time += Time.deltaTime;
			percentage = time / duration;
			ikEffector.positionWeight = Mathf.Lerp (currentWeight, targetWeight, percentage);
			yield return null;
		}
		ikEffector.positionWeight = targetWeight;
		ikEffector.target = null;
	}

	void LateUpdate () {
		pickables.Clear ();
		usables.Clear ();
	}

	void OnTriggerStay (Collider c) {
		PickableObject p = c.GetComponent<PickableObject> ();
		if (p && !pickables.Contains (p)) {
			pickables.Add (p);
		}

		UsableObject u = c.GetComponent<UsableObject> ();
		if (u && !usables.Contains (u)) {
			usables.Add (u);
		}
	}
}
