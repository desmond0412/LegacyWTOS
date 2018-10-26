using UnityEngine;
using System.Collections;

public class LevControllerSO : ScriptableObject {
	public RuntimeAnimatorController animatorController;
	public bool hasGauntlet = false;
	public float runSpeed = 4f;
	public float runAcceleration = 30;
	public float onAirSpeed = 3f;
	public float onAirAcceleration = 10f;
	public float turnDuration = 0.2f;
	public float maxJumpHeight = 0.5f;
	public float landingTime = 0.067f;
	public float maxSlopeAngle = 30;

	[Header ("Appearance")]
	public float unlitValue = 0.15f;

	public LevControllerSO clone () {
		LevControllerSO newObj = ScriptableObject.CreateInstance<LevControllerSO> ();
		newObj.animatorController = animatorController;
		newObj.hasGauntlet = hasGauntlet;
		newObj.runSpeed = runSpeed;
		newObj.runAcceleration = runAcceleration;
		newObj.onAirSpeed = onAirSpeed;
		newObj.onAirAcceleration = onAirAcceleration;
		newObj.turnDuration = turnDuration;
		newObj.maxJumpHeight = maxJumpHeight;
		newObj.landingTime = landingTime;
		newObj.maxSlopeAngle = maxSlopeAngle;

		newObj.unlitValue = unlitValue;

		return newObj;
	}
}