using UnityEngine;
using System.Collections;

public class LevAnimatorHandler : MonoBehaviour {

	private LevController levController;
	public VFXTriggerHandler vfxHandler;
	public Transform pickHandle;

	private Vector3 startThrowPickedObjectPos;
	private float startThrowPickedObjectTime;

	void Start()
	{
		levController = GetComponentInParent<LevController> ();
		levController.OnLanding += LevController_OnLanding;

	}

	void OnDestroy()
	{
		levController.OnLanding -= LevController_OnLanding;

	}

	void LevController_OnLanding ()
	{
		vfxHandler.PlayVFX(VFXTriggerType.Lev_Landing);
	}


	public void blendPickIK (float duration) {
		levController.levInteractionController.blendPickIK (duration);
	}

	public void grabPickedObject () {
		levController.levInteractionController.grabPickedObject ();
	}

	public void startThrowPickedObject () {
		startThrowPickedObjectPos = pickHandle.position;
		startThrowPickedObjectTime = Time.time;
	}

	public void endThrowPickedObject () {
		Vector3 movement = pickHandle.position - startThrowPickedObjectPos;
		float deltaTime = Time.time - startThrowPickedObjectTime;
		levController.levInteractionController.throwPickedObject (deltaTime == 0 ? Vector3.zero : (movement / deltaTime));
	}
	
	public void blendUseIK (float duration) {
		levController.levInteractionController.blendUseIK (duration);
	}

	public void stopUseIK () {
		levController.levInteractionController.stopUseIK ();
	}

	public void useObject () {
		levController.levInteractionController.useObject ();
	}

	public void onFootStep()
	{
		levController.levAudioController.playStepSound ();
		//TODO play particle snow;
		vfxHandler.PlayVFX(VFXTriggerType.Lev_Walk);
	}

	public void onDropDeadLanding()
	{
		levController.levAudioController.playDropDeadSound();
	}

	public void onFall()
	{
		levController.levAudioController.playFallSound();
	}

	public void onDrowned()
	{
		levController.levAudioController.playDrownSound();
	}


	public void dieOnGround() {
		levController.FacingDirectionInstant = LevController.Direction.Right;
	}
}
