using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Artoncode.Core.UI;

[RequireComponent(typeof(BoxCollider))]
public class PortalTrigger : MonoBehaviour {

	public bool isPortalOnTriggerEnter = true;
	public LocationModel portalTo;
	private SceneFader fader;
	bool isPortalling = false;	

	private void Start()
	{
		fader = SceneFader.shared();
	}

	public void Portal(bool force = false)
	{
		isPortalling = true;
		fader.OnFadeOutCompleted +=  Fader_OnFadeOutCompleted;
		fader.FadeOutStart();



//		if (force || stateRequirement == null || stateRequirement.isGameStateMeet () )
//		{
//			GameManager.shared().SetSpawnPosition(checkPoint.checkPointID);
//			GameManager.shared().UtilitiesData.setString ("LoadingScene_target_string", checkPoint.sceneName);
//
//
//			if(checkPoint.sceneName == Application.loadedLevelName)
//			{
//				CheckPointListType spawnPoint = GameManager.shared().GetSpawnPosition();
//				SceneController.shared().RoubPlayerObject.transform.position = SpawnController.shared().GetSpawnPosition(spawnPoint).position;
//				SceneController.shared().RoubPlayerObject.transform.eulerAngles = SpawnController.shared().GetSpawnPosition(spawnPoint).eulerAngles;
//			}
//			else{
//				InputManager.shared().disableTouch();
//				LevelController.shared().LoadLevel(checkPoint.sceneName);
//			}
//
//		}
//
	}

	void Fader_OnFadeOutCompleted ()
	{
		fader.OnFadeOutCompleted -= Fader_OnFadeOutCompleted;

		//destroy all dont destroy onload
		MainObjectSingleton.DestoryAllInstance();
		GameDataManager.shared().PlayerLocation = portalTo;
		SceneManager.LoadScene (portalTo.sceneName);
//		GameDataManager.shared ().NextSceneFromLoadScreen = portalTo.sceneName;
//		SceneManager.LoadScene ("LoadingScreen");
	}


	void OnTriggerEnter (Collider other) {
		if(other.CompareTag("Player") && isPortalOnTriggerEnter )
		{
			if (!isPortalling) {
				MainObjectSingleton.shared(MainObjectType.Player).GetComponent<LevController>().stopMoving();
				MainObjectSingleton.shared(MainObjectType.Player).GetComponent<LevController>().setInputEnable(false);
				Portal();
			}
		}
			
	}


}
