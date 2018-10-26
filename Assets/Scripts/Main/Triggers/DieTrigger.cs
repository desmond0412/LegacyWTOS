using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Artoncode.Core.UI;
using UnityEngine.SceneManagement;
using SmartLocalization;

public enum DieCausesType
{
	Fall 			= 0,
	Drown			= 1,
	EatenByAtatar	= 2,
	DeepShadow		= 3,
	Moluvusha		= 4,
	Phylex			= 5,
	Rock			= 6,
}

public class DieTrigger : MonoBehaviour {

	public static bool isPlayerDead = false;
	public DieCausesType causes;


	protected SceneFader fader;
	protected GameoverCameraManager gameoverCameraObject;
	protected string animationName;
	protected string dieLastWord;
	protected bool isTriggered = false;

	//CAMERA SMOOTH FOLLOW 
	protected float camTransition = 0.5f;
	protected Vector3 camPosOffset = new Vector3(0,0,-13);
	protected Vector3 camLookAtOffset = new Vector3(0,0,0);

	public virtual void Start()
	{
		if(DieTrigger.isPlayerDead)
			DieTrigger.isPlayerDead = false;
		//TODO getanimation name and text to display
		dieLastWord 	= this.GetLastWord(causes);
	}

	protected void SetLayerRecursively(GameObject obj,int layer)
	{
		obj.layer = layer;
		foreach (Transform child in obj.transform) {
			SetLayerRecursively(child.gameObject,layer);
		}
	}

	protected virtual void OnTriggerEnter(Collider other)
	{
		if(isTriggered)return;

		if(other.CompareTag("Player"))
		{
			isTriggered = true;
			Trigger();
		}
	}

	public virtual void Trigger(bool isCulpritIncluded = true){

		if(DieTrigger.isPlayerDead)return;
		DieTrigger.isPlayerDead = true;
		//TODO set the owner / gameobject ot the character gameover layer
		if(isCulpritIncluded)
			SetLayerRecursively(gameObject,LayerMask.NameToLayer("Character"));
		
		DieAnimationFlow();
	}


	public virtual void DieAnimationFlow()
	{
		CameraStopFollow();
		CameraStartFocusOnLev();

		//TODO play animation
		LevPlayCustomAnimation(causes);

		//TODO call fading 
		StartDieFX();

	}

	protected virtual void CameraStopFollow()
	{
		MainObjectSingleton.shared(MainObjectType.Camera).GetComponent<Artoncode.Core.SmoothFollow>().enabled = false;
	}

	protected virtual void CameraStartFocusOnLev()
	{
		MainObjectSingleton.shared(MainObjectType.Camera).GetComponent<Artoncode.Core.SmoothFollow>().enabled 			= true;
		MainObjectSingleton.shared(MainObjectType.Camera).GetComponent<Artoncode.Core.SmoothFollow>().target 			= MainObjectSingleton.shared(MainObjectType.Player).gameObject.transform;
		MainObjectSingleton.shared(MainObjectType.Camera).GetComponent<Artoncode.Core.SmoothFollow>().positionOffset 	= camPosOffset;
		MainObjectSingleton.shared(MainObjectType.Camera).GetComponent<Artoncode.Core.SmoothFollow>().lookAtOffset		= camLookAtOffset;
		MainObjectSingleton.shared(MainObjectType.Camera).GetComponent<Artoncode.Core.SmoothFollow>().smoothTime		= camTransition;
		MainObjectSingleton.shared(MainObjectType.Camera).GetComponent<Artoncode.Core.SmoothFollow>().IsLocked			= true;

	}


	protected void LevPlayCustomAnimation(DieCausesType causes)
	{
		LevDeathAnimationHandler lev = MainObjectSingleton.shared(MainObjectType.Player).GetComponent<LevDeathAnimationHandler>();
		lev.GetComponent<LevController> ().levInteractionController.throwPickedObject (Vector3.zero); //drop picked object
		lev.Die(causes);
	}



	protected virtual void StartDieFX()
	{
//		gameoverCameraObject = GameoverCameraManager.shared();
		gameoverCameraObject = GameObject.FindWithTag("GameoverManager").GetComponent<GameoverCameraManager>();
		gameoverCameraObject.GetComponent<Camera>().enabled = true;
		fader 				 = SceneFader.shared();


		gameoverCameraObject.OnBlackoutEnded 		+= GameoverCameraObject_OnBlackoutEnded;
		gameoverCameraObject.ShowBlackOut();
	}


	protected virtual void GameoverCameraObject_OnBlackoutEnded (GameObject sender)
	{
		gameoverCameraObject.OnBlackoutEnded 		-= GameoverCameraObject_OnBlackoutEnded;

		gameoverCameraObject.OnTextComesOutEnded 	+= GameoverCameraObject_OnTextComesOutEnded;
		sender.GetComponent<GameoverCameraManager>().ShowText(dieLastWord);

	}

	protected virtual void GameoverCameraObject_OnTextComesOutEnded (GameObject sender)
	{
		gameoverCameraObject.OnTextComesOutEnded 	-= GameoverCameraObject_OnTextComesOutEnded;
		EndDieFX();
	}

	protected virtual void EndDieFX()
	{
		fader.OnFadeOutCompleted += HandleOnFadeOutCompleted;
		SetLayerRecursively(fader.gameObject,LayerMask.NameToLayer("GameoverLayerUI"));
		fader.FadeOutStart();

	}

	void HandleOnFadeOutCompleted()
	{
		fader.OnFadeOutCompleted -= HandleOnFadeOutCompleted;
		//
		MainObjectSingleton.DestoryAllInstance();
		//load scene
		//		SceneManager.LoadScene (GameDataManager.shared().PlayerLocation.sceneName);

		SceneManager.LoadScene (GameDataManager.shared().PlayerLocation.sceneName);
	
	}



	#region helper
//	private GameObject CreateCameraPrefab()
//	{
//		GameObject cameraFXDieObject = GameObject.FindWithTag("GameOverCamera");
//		cameraFXDieObject.GetComponent<Camera>().enabled = true;
//
//		cameraFXDieObject.transform.SetAsLastSibling();
//		return cameraFXDieObject;
//	}
	protected string GetLastWord(DieCausesType type)
	{
		string key = "DeathLine.";
		switch (type) 
		{
			case DieCausesType.Fall				: 	key += "Fall";	break;
			case DieCausesType.Drown			: 	key += "Drown";	break;
			case DieCausesType.Phylex			: 	key += "Phylex";	break;
			case DieCausesType.Moluvusha		: 	key += "Moluvusha";	break;
			case DieCausesType.DeepShadow		: 	key += "Hole";	break;
			case DieCausesType.EatenByAtatar	: 	key += "Atatar";	break;
			case DieCausesType.Rock				: 	key += "Rock";	break;
		}

		return GetWordSet(key).Random();
	}

	protected string[] GetWordSet(string key)
	{
		List<string> wordset = new List<string>();
		foreach (string item in LanguageManager.Instance.GetAllKeys()) {
			
			string[] splittedPath = item.Split('.');
			if(splittedPath[0] != "DeathLine") continue;

			if(item.Contains(key))
			{
				wordset.Add(item);
			}
		}
		return wordset.ToArray();
	}

	#endregion

	void OnDestroy()
	{
		DieTrigger.isPlayerDead = false;
	}



}

