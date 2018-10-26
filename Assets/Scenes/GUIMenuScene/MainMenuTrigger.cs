using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class MainMenuTrigger : MonoBehaviour {

	private bool isTriggered = false;
	private LevelAudioTrigger audio;

	private void Awake()
	{
		audio = this.GetComponent<LevelAudioTrigger>();
	}

	void OnTriggerEnter(Collider other)
	{
		if(isTriggered) return;
		if (other.CompareTag("Player") && GameDataManager.shared ().LOCAL_IsLoadOpening) {
			isTriggered = true;
			GameDataManager.shared ().LOCAL_IsLoadOpening = false;

			if(GUIMenu.shared() !=null )
			{
				GUIMenu.shared().camera.SaveCamPos();
				GUIMenu.shared().camera.SaveCamFoV();

				// Activate Opening
				GUIMenu.shared ().AnimateOpening ();
				

				GUIMenu.shared ().openingMenu.OnMyuuJournal += HandleOpeningMenuStartingNewGame;
				GUIMenu.shared ().openingMenu.OnStartingNewGame += HandleOpeningMenuStartingNewGame;
				GUIMenu.shared ().openingMenu.OnContinueGame += HandleOpeningMenuContinueGame;
				GUIMenu.shared ().openingMenu.OnMenuHighlighted += HandleOpeningMenuHighlighted;
			}

			MainObjectSingleton.shared(MainObjectType.Camera).GetComponent<Artoncode.Core.SmoothFollow>().enabled = false;
//			MainObjectSingleton.shared(MainObjectType.Camera).GetComponent<Artoncode.Core.CameraPlatformer.CameraController>().enabled = false;
			MainObjectSingleton.shared (MainObjectType.Player).GetComponent<LevController> ().setInputEnable (false);
			MainObjectSingleton.shared (MainObjectType.Player).GetComponent<LevController> ().turnToForeground();
			MainObjectSingleton.shared (MainObjectType.Player).GetComponent<LevController> ().playCustomAnimation("MAINMENU.AC_Lev_NoGauntlet_MainMenu_Idle");
		}
	}

	void HandleOpeningMenuHighlighted (string menu)
	{
		string animationName = "MAINMENU.";
		switch (menu) {
		case "CONTINUE" : 
			animationName += "AC_Lev_NoGauntlet_MainMenu_Continue";
			break;

		case "NEW" : 
			animationName += "AC_Lev_NoGauntlet_MainMenu_NewGame";
			break;

		case "SETTING" : 
			animationName += "AC_Lev_NoGauntlet_MainMenu_Setting";
			break;

		case "QUIT" : 
			animationName += "AC_Lev_NoGauntlet_MainMenu_Exit";
			break;


		}
		if (!MainObjectSingleton.shared (MainObjectType.Player).GetComponent<LevController> ().levAnimator.GetCurrentAnimatorStateInfo (3).IsName ("AC_Lev_MyuuJournal_Unlock"))
			MainObjectSingleton.shared (MainObjectType.Player).GetComponent<LevController> ().playCustomAnimation (animationName);

	}

	void HandleOpeningMenuContinueGame ()
	{
		
		ClearEvents ();
		MainObjectSingleton.shared(MainObjectType.Camera).GetComponent<Artoncode.Core.SmoothFollow>().enabled = true;
//		MainObjectSingleton.shared(MainObjectType.Camera).GetComponent<Artoncode.Core.CameraPlatformer.CameraController>().enabled = true;
		MainObjectSingleton.shared(MainObjectType.Player).GetComponent<LevController> ().setInputEnable(true);
		MainObjectSingleton.shared (MainObjectType.Player).GetComponent<LevController> ().levAnimator.SetTrigger("doneMainmenu");
		if (MainObjectSingleton.shared (MainObjectType.Player).GetComponent<LevController> ().levAnimator.GetCurrentAnimatorStateInfo (3).IsName ("AC_Lev_MyuuJournal_Unlock"))
			MainObjectSingleton.shared (MainObjectType.Player).GetComponent<LevController> ().playCustomAnimation ("MAINMENU.AC_Lev_NoGauntlet_MainMenu_Idle");

		if(GUIMenu.shared() !=null )
		{
			GUIMenu.shared().isIdle = true;
			GUIMenu.shared ().camera.MoveCamFoV (GUICamZoom.Original,0.3f,0f,null);
		}

		audio.PlayLevelBGM();
	}


	void HandleOpeningMenuStartingNewGame ()
	{
		ClearEvents ();
		GUIMenu.shared ().isIdle = true;
		MainObjectSingleton.DestoryAllInstance();
	}

	void ClearEvents()
	{
		if(GUIMenu.shared() !=null )
		{
			GUIMenu.shared ().openingMenu.OnContinueGame -= HandleOpeningMenuContinueGame;
			GUIMenu.shared().openingMenu.OnStartingNewGame -= HandleOpeningMenuStartingNewGame;
			GUIMenu.shared ().openingMenu.OnMyuuJournal -= HandleOpeningMenuStartingNewGame;
			GUIMenu.shared ().openingMenu.OnMenuHighlighted -= HandleOpeningMenuHighlighted;
		}
	}

}
