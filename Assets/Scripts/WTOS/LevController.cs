using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Artoncode.Core.Utility;
using Artoncode.WTOS;

public class LevController : MonoBehaviour, IGroundCheckerDelegate {
	public delegate void LevStateChangedDelegate ();
	public event LevStateChangedDelegate OnLanding;

	public delegate void CustomAnimationDelegate (GameObject sender);
	public event CustomAnimationDelegate OnCustomAnimationFinished;

	public enum Direction
	{
		Left, 
		Right
	};

	public enum LevAnimLayer
	{
		BaseLayer,
		BodyHeadLayer,
		RHandLayer,
		CustomAnimationLayer,
	}

	public Animator levAnimator;
	public LevAudioController levAudioController;
	public LevGauntletController levGauntletController;
	public LevInteractionController levInteractionController;
	public LevControllerSO levSetting;
	private float speedModifier = 1;

	public CapsuleCollider bodyCollider;

	private Rigidbody body;
	private Vector2 velocity;
	public Direction facingDirection {
		set {
			facingDir = value;
			if (facingDir == Direction.Left) {
				targetDirection = rightAngle + 180;
			}
			else {
				targetDirection = rightAngle;
			}
		}
		get {
			return facingDir;
		}
	}
	public Direction FacingDirectionInstant {
		set {
			facingDirection = value;
			direction = targetDirection;
			transform.rotation = Quaternion.AngleAxis(direction, Vector3.up);
		}
	}
	private Direction facingDir;
	private float direction;
	private float targetDirection;
	private float rightAngle;
	private Ray walkingLine;
	private Vector3 levRight;
	private float modifiedOnAirSpeed;

	[HideInInspector] public bool isInputEnabled;
	[HideInInspector] public bool isGrounded;
	[HideInInspector] public bool isJumping;
	private bool isMoving;
	private bool isJumpVelocityLocked;

	// Moving platform support
	private Transform activePlatform;
	private Vector3 previousPlatformPosition;
	private Vector3 lastPlatformVelocity;

	private Coroutine bodyHeadBlendLayerCoroutine;
	private Coroutine customAnimationBlendLayerCoroutine;
	private Coroutine landingCoroutine;

	public static int IdleAnimationHash = Animator.StringToHash("Base Layer.Idle.Idle");

	[Header ("Materials")]
	public Material[] bodyMaterials;

	void Start () {
		body = GetComponent<Rigidbody> ();
		velocity = Vector2.zero;
		direction = 0;
		facingDirection = Direction.Right;
		changeWalkingLine (transform.position, Vector3.right);
		setInputEnable (true);
		setLevSetting (levSetting);
		initMeshBounds ();
	}

	void Update () {
		direction = GameUtility.changeTowards (direction, targetDirection, 180 / levSetting.turnDuration, Time.deltaTime);
		transform.rotation = Quaternion.AngleAxis(direction, Vector3.up);

		// Handle inputs
		if (!isInputEnabled) {
			return;
		}
		bool goingLeft = Input.GetKey(KeyCode.A);
		bool goingRight = Input.GetKey(KeyCode.D);
		bool isJumpTriggered = Input.GetKeyDown(KeyCode.Space);

		if (goingLeft && goingRight) {
			goingLeft = goingRight = false;
		}

		if (goingLeft) {
			moveLeft ();
		}
		if (goingRight) {
			moveRight ();
		}
		if (!goingLeft && !goingRight && !isJumpVelocityLocked) {
			stopMoving ();
		}
		if (isJumpTriggered) {
			jump ();
		}

//		if (Input.GetKeyDown (KeyCode.Alpha1)) {
//			AkSoundEngine.PostEvent ("atatar_attack", gameObject);
//		}
//		if (Input.GetKeyDown (KeyCode.Alpha2)) {
//			AkSoundEngine.PostEvent ("atatar_fs", gameObject);
//		}
//		if (Input.GetKeyDown (KeyCode.Alpha3)) {
//			AkSoundEngine.PostEvent ("atatar_idle", gameObject);
//		}
//		if (Input.GetKeyDown (KeyCode.Alpha4)) {
//			AkSoundEngine.PostEvent ("atatar_nomnom_start", gameObject);
//		}
//		if (Input.GetKeyDown (KeyCode.Alpha5)) {
//			AkSoundEngine.PostEvent ("atatar_nomnom_stop", gameObject);
//		}

//		if (Input.GetKeyDown (KeyCode.S)) {
//			turnToForeground ();
//		}
//		if (Input.GetKeyDown (KeyCode.W)) {
//			turnToBackground ();
//		}
//		if (Input.GetKeyDown (KeyCode.Q)) {
//			turnToNormal ();
//		}

//		if (Input.GetKeyDown (KeyCode.Q)) {
//			lookBack ();
//		}
//		if (Input.GetKeyUp (KeyCode.Q)) {
//			stopLookBack ();
//		}

//		if (Input.GetKeyDown (KeyCode.Q)) {
//			setIdleVariation (1);
//		}

//		if (Input.GetKeyDown (KeyCode.Q)) {
//			playCustomAnimation ("AC_Lev_NoGauntlet_MainMenu_Continue");
//		}

//		if (Input.GetKeyDown (KeyCode.Q)) {
//			jumpByZRoot (new Vector3(-1,1,0).normalized, 10);
//		}

//		if (Input.GetKeyDown (KeyCode.Z)) {
//			
//			GetComponentInChildren<LevAudioController> ().playStepSound ();
//		}
	}

	void initMeshBounds () {
		SkinnedMeshRenderer[] renderers = GetComponentsInChildren<SkinnedMeshRenderer> ();
		foreach (SkinnedMeshRenderer r in renderers) {
			r.localBounds = new Bounds (Vector3.zero, Vector3.one * 100);
		}
	}

	void FixedUpdate () {
		levRight = facingDirection == Direction.Right ? walkingLine.direction : -walkingLine.direction;

		Vector3 movementAdjustment = calculateMovementAdjustment(transform.position);
		transform.Translate (new Vector3 (transform.right.z * -movementAdjustment.x, 0, transform.right.x * -movementAdjustment.z));

		if (!isJumpVelocityLocked) {
			if (isMoving) {
				float maxSpeed = (isGrounded ? levSetting.runSpeed : modifiedOnAirSpeed);
				Vector3 currBodyVelocity = body.velocity;
				currBodyVelocity.y = 0;
				bool isMovingForward = isInFront (transform.position + currBodyVelocity);
				velocity.x = Mathf.Clamp ((isMovingForward ? 1 : -1) * currBodyVelocity.magnitude, -Mathf.Abs (maxSpeed), Mathf.Abs (maxSpeed));
				velocity.x = GameUtility.changeTowards (velocity.x, maxSpeed, (isGrounded ? levSetting.runAcceleration : levSetting.onAirAcceleration), Time.fixedDeltaTime);
			}
			else {
				velocity.x = GameUtility.changeTowards (velocity.x, 0, (isGrounded? levSetting.runAcceleration : levSetting.onAirAcceleration), Time.fixedDeltaTime);
			}
		}

		velocity.y = body.velocity.y + Physics.gravity.y * Time.fixedDeltaTime;
		velocity.y = Mathf.Clamp(velocity.y, -30, 50);

		if (isGrounded && !isJumping) {
			Ray ray = new Ray(transform.position, Vector3.down);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 0.5f) && !hit.collider.isTrigger) {
				float groundAngleRad = (Mathf.Atan2 (hit.normal.y, hit.normal.x) - Mathf.PI / 2);
				float groundAngleDeg = groundAngleRad * Mathf.Rad2Deg;

				// Snap Lev's pivot to ground
				float distFromGround = bodyCollider.radius / Mathf.Cos (Mathf.Abs (groundAngleRad)) - bodyCollider.radius;
				Vector3 moveDistance = new Vector3 (0, distFromGround - hit.distance, 0);
				if (moveDistance.y < -1e-4 && moveDistance.y > 0.2f) {
					transform.Translate (moveDistance);
				}

				// Stop Lev from sliding slope
				if ((groundAngleDeg > 0 && groundAngleDeg < levSetting.maxSlopeAngle) || (groundAngleDeg < 0 && groundAngleDeg > -levSetting.maxSlopeAngle)) {
					if (!isMoving) {
						Rigidbody groundRB = hit.collider.GetComponent<Rigidbody> ();
						if (groundRB) {
							if (groundRB.IsSleeping ()) {
								velocity.y = GameUtility.changeTowards (velocity.y, 0, -Physics.gravity.y, Time.fixedDeltaTime);
							}
							else if (!groundRB.IsSleeping () && groundRB.angularVelocity.sqrMagnitude < 1e-5 && groundRB.velocity.sqrMagnitude < 1e-5) {
								velocity.y = GameUtility.changeTowards (velocity.y, 0, -Physics.gravity.y, Time.fixedDeltaTime);
								groundRB.AddForceAtPosition (Physics.gravity * body.mass, transform.position);
							}
						}
						else {
							velocity.y = GameUtility.changeTowards (velocity.y, 0, -Physics.gravity.y, Time.fixedDeltaTime);
						}
					}
				}
			}
		}

		bool shouldLand = shouldLanding();
		if (shouldLand && (!isGrounded || isJumping)) {
			if (landingCoroutine == null) {
				landingCoroutine = StartCoroutine (landing ());
			}
		}
		body.velocity = new Vector3 (levRight.x * velocity.x, velocity.y, levRight.z * velocity.x);

		levAnimator.SetFloat ("horizontalSpeed", velocity.x);
		levAnimator.SetFloat ("verticalSpeed", velocity.y);
		levAnimator.SetBool ("isGrounded", isGrounded);

		if (velocity.x > 0.01f || !isGrounded) {
			setIdleVariation ();
		}

		// Moving/Rotating platform support
		if (activePlatform != null) {
			Vector3 moveDistance = (activePlatform.position - previousPlatformPosition);
			if (moveDistance != Vector3.zero)
				transform.Translate (moveDistance, Space.World);
			lastPlatformVelocity = moveDistance / Time.deltaTime;
			previousPlatformPosition = activePlatform.position;
		}
	}

	public void setLevSetting (LevControllerSO setting) {
		levSetting = setting.clone ();
		levGauntletController.setHasGauntlet (levSetting.hasGauntlet);
		levAnimator.runtimeAnimatorController = levSetting.animatorController;
		foreach (Material m in bodyMaterials) {
			m.SetFloat ("_UnlitValue", levSetting.unlitValue);
		}

		modifiedOnAirSpeed = levSetting.onAirSpeed;
	}

	public void setInputEnable (bool enable) {
		isInputEnabled = enable;
	}

	public void moveLeft () 
	{
		if (facingDirection == Direction.Right) {
			modifiedOnAirSpeed = levSetting.onAirSpeed;
		}
		facingDirection = Direction.Left;
		isMoving = true;
		isJumpVelocityLocked = false;
	}

	public void moveRight () 
	{
		if (facingDirection == Direction.Left) {
			modifiedOnAirSpeed = levSetting.onAirSpeed;
		}
		facingDirection = Direction.Right;
		isMoving = true;
		isJumpVelocityLocked = false;
	}
	
	public void turnToForeground () {
		if (facingDirection == Direction.Right) {
			targetDirection = rightAngle + 90;
		}
		else {
			targetDirection = rightAngle + 90;
		}
	}
	
	public void turnToBackground () {
		if (facingDirection == Direction.Right) {
			targetDirection = rightAngle - 90;
		}
		else {
			targetDirection = rightAngle + 270;
		}
	}

	public void turnToNormal () {
		facingDirection = facingDirection;
	}

	public void lookBack () {
		levAnimator.SetBool ("isLookBack", true);
		if (bodyHeadBlendLayerCoroutine != null) {
			StopCoroutine (bodyHeadBlendLayerCoroutine);
		}
		bodyHeadBlendLayerCoroutine = StartCoroutine (blendAnimatorLayerWeight ((int)LevAnimLayer.BodyHeadLayer, 1, 0.2f));
	}

	public void stopLookBack () {
		levAnimator.SetBool ("isLookBack", false);
		if (bodyHeadBlendLayerCoroutine != null) {
			StopCoroutine (bodyHeadBlendLayerCoroutine);
		}
		bodyHeadBlendLayerCoroutine = StartCoroutine (blendAnimatorLayerWeight ((int)LevAnimLayer.BodyHeadLayer, 0, 0.2f));
	}

	public void setIdleVariation (int var = 0) {
		levAnimator.SetInteger ("idleVariation", var);
	}

	public void playCustomAnimation (string stateName, float transitionDuration = 0.25f) {
		setInputEnable (false);
		levAnimator.CrossFade ("Custom Animation Layer." + stateName, 0, (int)LevAnimLayer.CustomAnimationLayer, 0);
		if (customAnimationBlendLayerCoroutine != null) {
			StopCoroutine (customAnimationBlendLayerCoroutine);
		}
		customAnimationBlendLayerCoroutine = StartCoroutine (blendAnimatorLayerWeight ((int)LevAnimLayer.CustomAnimationLayer, 1, transitionDuration));
	}

	public void stopCustomAnimation (float transitionDuration = 0.25f) {
		if (customAnimationBlendLayerCoroutine != null) {
			StopCoroutine (customAnimationBlendLayerCoroutine);
		}
		customAnimationBlendLayerCoroutine = StartCoroutine (blendAnimatorLayerWeight ((int)LevAnimLayer.CustomAnimationLayer, 0, transitionDuration));
		setInputEnable (true);
		if (OnCustomAnimationFinished != null) {
			OnCustomAnimationFinished (gameObject);
		}
	}

	public void startCarry (string carryAnimation) {
		levAnimator.CrossFade ("RHand Layer." + carryAnimation, 0);
		StartCoroutine (blendAnimatorLayerWeight ((int)LevAnimLayer.RHandLayer, 1, 0));
	}

	public void stopCarry (float duration) {
		levAnimator.CrossFade ("RHand Layer.Empty", duration);
		StartCoroutine (blendAnimatorLayerWeight ((int)LevAnimLayer.RHandLayer, 0, duration));
	}

	public IEnumerator blendAnimatorLayerWeight (int layerIdx, float targetWeight, float duration) {
		float currentWeight = levAnimator.GetLayerWeight (layerIdx);
		float time = 0;
		float percentage = 0;
		while (percentage < 1) {
			time += Time.deltaTime;
			percentage = time / duration;
			levAnimator.SetLayerWeight (layerIdx, Mathf.Lerp (currentWeight, targetWeight, percentage));
			yield return null;
		}
		levAnimator.SetLayerWeight (layerIdx, targetWeight);
	}

	public void stopMoving () {
		isMoving = false;
	}

	public void jump() {
		if (isGrounded && !isJumping) {
			body.velocity = new Vector3 (body.velocity.x, calculateJumpSpeed (), body.velocity.z);

			Vector3 horizontalPlatformVelocity = lastPlatformVelocity;
			horizontalPlatformVelocity.y = 0;
			if (horizontalPlatformVelocity != Vector3.zero) {
				Vector3 jumpVelocityAffectedByPlatform = Vector3.Dot (horizontalPlatformVelocity, levRight) * levRight;
				body.velocity += jumpVelocityAffectedByPlatform;

				print (body.velocity);

				bool isJumpForward = isInFront (transform.position + body.velocity);
				float platformSpeed = jumpVelocityAffectedByPlatform.magnitude;
				modifiedOnAirSpeed = levSetting.onAirSpeed + (isJumpForward ? platformSpeed : -platformSpeed);
			}
			else {
				modifiedOnAirSpeed = levSetting.onAirSpeed;
			}

			isJumping = true;
			isJumpVelocityLocked = true;
			levAnimator.SetTrigger ("jump");
			setIdleVariation ();
			velocity = new Vector3 (body.velocity.x * levRight.x, body.velocity.y, body.velocity.z * levRight.z);
		}
	}

	public void jumpByZRoot (Vector3 direction, float velocity) {
		if (isGrounded && !isJumping) {
			this.velocity = direction * velocity;
			body.velocity = this.velocity;
			modifiedOnAirSpeed = body.velocity.x;
			isJumping = true;
			levAnimator.SetTrigger ("jump");
			setIdleVariation ();
			isMoving = true;
			isJumpVelocityLocked = true;
			setInputEnable (true);
		}
	}

	public IEnumerator landing () 
	{
		yield return new WaitForSeconds (levSetting.landingTime);
		isJumping = false;
		isJumpVelocityLocked = false;
		landingCoroutine = null;
		modifiedOnAirSpeed = levSetting.onAirSpeed;
		if (OnLanding != null) {
			OnLanding ();
		}
	}

	private float calculateJumpSpeed() 
	{
		return Mathf.Sqrt (-2 * levSetting.maxJumpHeight * Physics.gravity.y);
	}

	public void groundCheckUpdate (bool isOnGround, Collider firstCollider) 
	{
		isGrounded = isOnGround;
		if (firstCollider) {
			if (!isJumping) {
				if (activePlatform == null || activePlatform != firstCollider.transform) {
					activePlatform = firstCollider.transform;
					previousPlatformPosition = activePlatform.position;
				}
			}
			else {
				activePlatform = null;
			}
		}
		else {
			activePlatform = null;
		}

//		animator.SetBool ("grounded", isGrounded);
	}

	private bool shouldLanding() 
	{
		if (velocity.y<=0.1f) 
		{
			float landingDistanceY = Mathf.Abs(velocity.y * levSetting.landingTime + 0.5f * Physics.gravity.y * levSetting.landingTime * levSetting.landingTime);

			if (landingDistanceY>0)
			{
				float landingDistanceX = velocity.x * levSetting.landingTime;
				Vector3 landingPoint = new Vector3(landingDistanceX, -landingDistanceY, 0);

				Ray ray = new Ray(transform.position, landingPoint);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, landingPoint.magnitude) && !hit.collider.isTrigger) {
					return true;
				}
			}
			if (isGrounded) {
				return true;
			}
		}
		return false;
	}

	public void changeWalkingLine(Vector3 origin, Vector3 direction) {
		walkingLine.origin = origin;
		walkingLine.direction = direction.normalized;

		rightAngle = Mathf.Atan2 (-direction.z, direction.x) * Mathf.Rad2Deg;
		facingDirection = facingDirection;
	}

	public bool isInFront (Vector3 position) {
		Vector3 direction = position - transform.position;
		direction.y = 0;
		return Vector3.Angle (levRight, direction) < 90;
	}

	Vector3 calculateMovementAdjustment(Vector3 position) {
		position = position - walkingLine.origin;
		position.y = 0;
		float scalarProjection = Vector3.Dot(position, walkingLine.direction);
		Vector3 orthogonalProjection = scalarProjection * walkingLine.direction;
		return position - orthogonalProjection;
	}
}
