using UnityEngine;
using System;
using System.Collections;

using RootMotion.FinalIK;

public class LevGauntletController : MonoBehaviour {

	[Serializable]
	public class GauntletAmmo {
		public GameObject ammoPrefab;
		public string aimAnimation;
		public GauntletType type;
		[HideInInspector] public GameObject ammoObject;
	}

	[System.Serializable]
	public enum GauntletType
	{
		None,
		Zroot,
		Voltex,
		Airhole,
		Bobble,
	}

	public Camera cam;
	private LevController levController;
	public Transform aimPivot;
	public Transform gauntletNozzle;
	public GauntletAmmo[] ammo;
	private GauntletAmmo currentAmmo;

	[Header ("Gauntlet Mesh")]
	public GameObject gauntletMesh;
	public GameObject gauntletGlowMesh;

	private bool isAiming;
	private bool isAimLocked;
	private bool isAbleToShoot;
	private bool shouldStopAiming;
	private bool shouldStartAiming;
	private Vector3 targetPosOnScreen;

	private float aimAngle;

	private Coroutine prepareShootCoroutine;

	// Use this for initialization
	void Start () {
		levController = GetComponent<LevController> ();
		isAiming = false;

		for (int i=0; i<ammo.Length; i++) {
			ammo[i].ammoObject = GameObject.Instantiate (ammo[i].ammoPrefab);
			ammo[i].ammoObject.transform.SetParent (levController.transform, false);
			ammo[i].ammoObject.SetActive (false);
		}

		setCurrentAmmo (GauntletType.Zroot);
	}

	void Update () {
		if (levController.isInputEnabled || isAiming) {
			if (Input.GetMouseButton (1)) {
				shouldStartAiming = true;
				shouldStopAiming = false;
			}
			if (!Input.GetMouseButton (1)) {
				shouldStopAiming = true;
				shouldStartAiming = false;
			}

			if (Input.GetMouseButtonDown (0) && isAbleToShoot) {
				shoot ();
			}
		}

		setTargetPositionOnScreen (Input.mousePosition);
		checkAimRelease ();
		calculateAimAngle ();
	}

	void LateUpdate () {
		if (shouldStartAiming) {
			startAim ();
			shouldStartAiming = false;
		}
		if (shouldStopAiming) {
			stopAim ();
			shouldStopAiming = false;
		}
	}

	public void setHasGauntlet (bool hasGauntlet) {
		gauntletMesh.SetActive (hasGauntlet);
		gauntletGlowMesh.SetActive (hasGauntlet);

		enabled = hasGauntlet;
	}

	public void setCurrentAmmo (GauntletType type) {
		currentAmmo = null;
		for (int i=0; i<ammo.Length; i++) {
			if (ammo[i].type == type) {
				currentAmmo = ammo[i];
				currentAmmo.ammoObject.SetActive (true);
				break;
			}
		}
	}

	public void shoot () {
		if (!isAiming || !levController.isGrounded || levController.isJumping || currentAmmo == null) {
			return;
		}

		levController.FacingDirectionInstant = levController.facingDirection;
		Vector3 aimDirection = Quaternion.AngleAxis (aimAngle, levController.transform.forward) * levController.transform.right;

		switch (currentAmmo.type) {
			case GauntletType.Zroot:
				ZRoot zRoot = currentAmmo.ammoObject.GetComponent<ZRoot> ();
				if (!zRoot.isShot ()) {
					zRoot.shootZRoot (gauntletNozzle.position, aimDirection);
					levController.levAnimator.SetTrigger ("isShooting");
					setLockAim (true);
				}
				break;
		}
	}

	public void checkAimRelease () {
		if (!isAiming) {
			return;
		}

		switch (currentAmmo.type) {
			case GauntletType.Zroot:
				ZRoot zRoot = currentAmmo.ammoObject.GetComponent<ZRoot> ();
				if (!zRoot.isShot () && !levController.isJumping) {
					setLockAim (false);
				}
				break;
		}
	}

	public void setLockAim (bool isAimLocked) {
		this.isAimLocked = isAimLocked;
	}

	public void jumpByZRoot(Vector3 direction,float forcePower){
		levController.jumpByZRoot (direction, forcePower);
		levController.OnLanding += OnLandingByZRoot;
	}

	private void OnLandingByZRoot () {
		levController.OnLanding -= OnLandingByZRoot;
		isAiming = false;
		if (!shouldStopAiming) {
			startAim ();
		}
	}

	public void startAim () {
		if (!isAiming && levController.isGrounded && !levController.isJumping && currentAmmo != null) {
			isAiming = true;
			levController.setInputEnable (false);
			levController.stopMoving ();
			levController.levAnimator.CrossFade ("Base Layer.Aim." + currentAmmo.aimAnimation, 0.1f, 0, 0);
			if (prepareShootCoroutine != null) {
				StopCoroutine (prepareShootCoroutine);
			}
			prepareShootCoroutine = StartCoroutine (prepareShoot (0.1f));
		}
	}

	IEnumerator prepareShoot (float delay) {
		yield return new WaitForSeconds (delay);
		isAbleToShoot = true;
		prepareShootCoroutine = null;
	}

	public void stopAim () {
		if (isAiming && !isAimLocked) {
			isAiming = false;
			levController.setInputEnable (true);
			if (!levController.isJumping) {
				levController.levAnimator.CrossFade ("Base Layer." + "Idle.Idle", 0.02f, 0, 0);
			}
			if (prepareShootCoroutine != null) {
				StopCoroutine (prepareShootCoroutine);
				prepareShootCoroutine = null;
			}
			isAbleToShoot = false;
		}
	}

	public void setTargetPositionOnScreen (Vector3 pos) {
		targetPosOnScreen = pos;
	}

	public void calculateAimAngle () {
		if (!isAiming || isAimLocked) {
			return;
		}

		Vector3 c = cam.WorldToScreenPoint (aimPivot.position);
		c.z = 0;
		Vector3 aimDirection = targetPosOnScreen - c;
		aimAngle = Mathf.Atan2 (aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

		if (aimAngle > 90 || aimAngle < -90) {
			levController.facingDirection = LevController.Direction.Left;
			if (aimAngle > 90) {
				aimAngle = 180 - aimAngle;
			}
			else {
				aimAngle = -180 - aimAngle;
			}
		}
		else {
			levController.facingDirection = LevController.Direction.Right;
		}

		levController.levAnimator.SetFloat ("aimAngle", aimAngle);
	}
}
