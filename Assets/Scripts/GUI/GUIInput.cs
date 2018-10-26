using UnityEngine;
using System.Collections;

public class GUIInput : MonoBehaviour {

	void Update () {
		if (Input.GetKeyUp (KeyCode.Escape)){
			if ((GUIMenu.shared().isAccessible) && (GUIMenu.shared().isIdle) && (!DieTrigger.isPlayerDead)) {
				if (GUIMenu.shared().myuuMenu)
				{
					Time.timeScale = 0f;
					GUIMenu.shared().OpenMyuuPausedMenu();
					GUIMenu.shared ().myuuPausedMenu.OnGoMainMenu += HandlePausedMenuMainMenu;
					GUIMenu.shared ().myuuPausedMenu.OnResume += HandleMyuuPausedMenuResume;	
				} else {
					Time.timeScale = 0f;
					GUIMenu.shared().OpenPausedMenu();
					GUIMenu.shared ().pausedMenu.OnGoMainMenu += HandlePausedMenuMainMenu;
					GUIMenu.shared ().pausedMenu.OnResumeGame += HandlePausedMenuResumeGame;

					GUIMenu.shared().camera.SaveCamPos();
					GUIMenu.shared().camera.SaveCamFoV();

//					GUIMenu.shared().camera.MoveCamFoV(GUICamZoom.In,0.25f,0f,null);
//					GUIMenu.shared().camera.MoveCamPos(GUICamPos.Center,0.25f,0f,null);
					
					MainObjectSingleton.shared (MainObjectType.Camera).GetComponent<Artoncode.Core.SmoothFollow> ().enabled = false;
//					MainObjectSingleton.shared (MainObjectType.Player).GetComponent<LevController> ().setInputEnable (false);
//					MainObjectSingleton.shared (MainObjectType.Player).GetComponent<LevController> ().stopMoving();
				}
			} else if (GUIMenu.shared().menuStack.Count>0) {
				GUIMenu.shared().menuStack[GUIMenu.shared().menuStack.Count-1].EscapeAction();
			} else if (GUIMenu.shared().openingLogo.openingRun){
				GUIMenu.shared().openingLogo.FinishOpeningRun();
			}
		}                                                      
	}

	void HandlePausedMenuMainMenu()
	{
		ClearEvents ();
		Time.timeScale = 1f;
		MainObjectSingleton.DestoryAllInstance();
	}

	void HandlePausedMenuResumeGame()
	{
		ClearEvents ();
		Time.timeScale = 1f;
		GUIMenu.shared ().pausedMenu.OnOutroEnd += EnableMainMenu;
		MainObjectSingleton.shared (MainObjectType.Camera).GetComponent<Artoncode.Core.SmoothFollow> ().enabled = true;
//		GUIMenu.shared().camera.MoveCamFoV(GUICamZoom.Original,0.25f,0f,null);
//		GUIMenu.shared().camera.MoveCamPos(GUICamPos.Original,0.25f,0f,null);
	}
	void EnableMainMenu()
	{
		GUIMenu.shared ().pausedMenu.OnOutroEnd -= EnableMainMenu;
		GUIMenu.shared().isIdle = true;
	}

	void HandleMyuuPausedMenuResume()
	{
		ClearEvents ();
		Time.timeScale = 1f;
		GUIMenu.shared().isIdle = true;
	}

	void ClearEvents()
	{
		GUIMenu.shared ().pausedMenu.OnGoMainMenu -= HandlePausedMenuMainMenu;
		GUIMenu.shared ().pausedMenu.OnResumeGame -= HandlePausedMenuResumeGame;
		GUIMenu.shared ().myuuPausedMenu.OnGoMainMenu -= HandlePausedMenuMainMenu;
		GUIMenu.shared ().myuuPausedMenu.OnResume -= HandleMyuuPausedMenuResume;					
	}
}
