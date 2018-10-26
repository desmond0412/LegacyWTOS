using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIMenu : ObjectSingleton<GUIMenu> {

	public OpeningLogo openingLogo;
	public GUICam camera;
	public ColorFader fader;
	public CircularOpeningMenu openingMenu;
	public CircularPausedMenu pausedMenu;
	public CircularMyuuPausedMenu myuuPausedMenu;
	public bool isAccessible = true;
	public bool isIdle = false;
	public bool myuuMenu = false;
	public bool debugMode;
	public List<BaseMenu> menuStack;

	protected override void OnInit ()
	{
		base.OnInit ();
		fader.gameObject.SetActive (false);
		openingMenu.cameraBlurer.SetBlur(false);
		openingMenu.cameraBlurer.faderTrans.gameObject.SetActive (false);
		menuStack = new List<BaseMenu>();
		openingLogo.gameObject.SetActive (false);
		DontDestroyOnLoad (gameObject);
	}

	public void AnimateOpening()
	{
//		print ("Begin Opening");
		fader.gameObject.SetActive (true);
		fader.SetFadeColor (ColorFaderType.Full);
		isIdle = false;
		openingLogo.gameObject.SetActive (true);
		openingLogo.StartOpening (0f);
		openingLogo.OnFinishOpening += OpenMainMenu;

		//set continue button state
		openingMenu.itemData [1].accessible = GameDataManager.shared().SaveGameExist;
	}

	void OpenMainMenu()
	{
		openingLogo.OnFinishOpening -= OpenMainMenu;
		StartCoroutine (OpenMainMenuDelayed (1f));

	}
	IEnumerator OpenMainMenuDelayed(float delay)
	{
		yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(delay));
		openingMenu.ShowIntro(false,0);
	}

	public void OpenPausedMenu()
	{
		isIdle = false;
		pausedMenu.ShowIntro (true, 0);
		pausedMenu.cameraBlurer.BlurIn (transform);
	}
	public void OpenMyuuPausedMenu()
	{
		isIdle = false;
		myuuPausedMenu.ShowIntro (true, 0);
		myuuPausedMenu.cameraBlurer.BlurIn (transform);
	}

	void OnGUI()
	{
		if (debugMode) {
			if (GUI.Button(new Rect(5, 5, 100, 50), "Set Blur Off"))
				openingMenu.cameraBlurer.SetBlur(false);
			if (GUI.Button(new Rect(5, 60, 100, 50), "Set Blur On"))
				openingMenu.cameraBlurer.SetBlur(true);
		}
	}
}
