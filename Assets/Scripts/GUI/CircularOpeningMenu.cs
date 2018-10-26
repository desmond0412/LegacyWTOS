using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class CircularOpeningMenu : CircularBaseMenu {
	public delegate void CircularOpeningMenuDelegate ();
	public delegate void CircularOpeningMenuStrParamDelegate (string param);


	public event CircularOpeningMenuDelegate OnFinishCamMove;
	public event CircularOpeningMenuDelegate OnStartingNewGame;
	public event CircularOpeningMenuDelegate OnContinueGame;
	public event CircularOpeningMenuDelegate OnMyuuJournal;


	public event CircularOpeningMenuStrParamDelegate OnMenuHighlighted;

	public CircularSettingsMenu settingsMenu;
	public MyuuOpeningJournal myuuOpeningJournal;
	public Vector3 deltaCamPos;
	public DialogMenu dialogMenu;
	public LocationModel FirstScene;

	public override void SetItemAction() 
	{
		base.SetItemAction ();
		itemList [0].OnItemClicked += NewClicked;
		itemList [1].OnItemClicked += ContinueClicked;
		itemList [2].OnItemClicked += QuitClicked;
		itemList [3].OnItemClicked += SettingsClicked;

		itemList [0].OnItemEnter   += NewEnter;
		itemList [1].OnItemEnter   += ContinueEnter;
		itemList [2].OnItemEnter   += QuitEnter;
		itemList [3].OnItemEnter   += SettingsEnter;	

		OnEscapeAction += QuitClicked;
		OnIntroEnd += myuuOpeningJournal.StartIntro;

//		print ("Save Data? " + GameDataManager.shared ().SaveGameExist);

	}



	void QuitEnter()
	{
		if(OnMenuHighlighted !=null){
			OnMenuHighlighted("QUIT");
		}
	}

	void SettingsEnter()
	{
		if(OnMenuHighlighted !=null){
			OnMenuHighlighted("SETTING");
		}

	}

	void ContinueEnter()
	{
		if(OnMenuHighlighted !=null){
			OnMenuHighlighted("CONTINUE");
		}

	}

	void NewEnter ()
	{
		if(OnMenuHighlighted !=null){
			OnMenuHighlighted("NEW");
		}

	}

	void NewClicked()
	{
		if (GameDataManager.shared ().SaveGameExist) {
			cameraBlurer.BlurIn(transform);
			dialogMenu.ShowDialog ("This will reset saved game.\nAre you sure?");
			dialogMenu.OnOutroStart += StartReturnBlur;
			dialogMenu.OnOutroEnd += NewYes;
		} else {
			dialogMenu.selectionYes = true;
			NewYes();
		}
	}

	void ContinueClicked()
	{
		ShowOutro ();
		myuuOpeningJournal.MyuuClicked("AUI_MyuuJournal_Remove");
		if(OnContinueGame!=null)
			OnContinueGame();
		
	}

	void QuitClicked()
	{
		cameraBlurer.BlurIn (transform);
		dialogMenu.ShowDialog ("Are you sure you want to quit?");
		dialogMenu.OnOutroStart += StartReturnBlur;
		dialogMenu.OnOutroEnd += QuitYes;
	}

	void SettingsClicked()
	{
		settingsMenu.ShowIntro (this);
		cameraBlurer.BlurIn (transform);   
		GUIMenu.shared ().camera.MoveCamFoV (GUICamZoom.Increase,0.5f,0f,null);
	}

	void StartReturnBlur()
	{
		dialogMenu.OnOutroStart -= StartReturnBlur;
		cameraBlurer.BlurOut(transform);
	}

	void NewYes()
	{
		dialogMenu.OnOutroEnd -= NewYes;
		if (dialogMenu.selectionYes) {
			Vector3 newPos = Camera.main.transform.position + deltaCamPos;
			MoveCam (newPos);
			OnFinishCamMove += AfterNew;
		}
	}
	void AfterNew()
	{
		OnFinishCamMove -= AfterNew;
		GameDataManager.shared ().SaveGameExist = false;
		GameDataManager.shared ().LOCAL_IsLoadOpening = false;
		GameDataManager.shared ().PlayerLocation = FirstScene;
		SceneManager.LoadScene (FirstScene.sceneName);

		GameDataManager.shared ().Save();

		cameraBlurer.faderBlur.SetFadeColor (ColorFaderType.Clear);
		myuuOpeningJournal.ResetPosition ();
		GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;

		if(OnStartingNewGame !=null)
			OnStartingNewGame();

	}

	void QuitYes()
	{
		dialogMenu.OnOutroEnd -= QuitYes;
		if (dialogMenu.selectionYes) {
			Vector3 newPos = Camera.main.transform.position - deltaCamPos;
			MoveCam (newPos);
			OnFinishCamMove += AfterQuit;
		}
	}
	void AfterQuit()
	{
		OnFinishCamMove -= AfterQuit;
		print ("Exit Game");
		Application.Quit ();
	}
	void MoveCam(Vector3 newPos)
	{
		cameraBlurer.faderBlur.FadeToColor (1f, 0.25f);
		GameObject mainCam = Camera.main.gameObject;
		iTween.MoveTo (mainCam, iTween.Hash ("position",newPos,
		                                     "time",2f,
		                                     "delay",0f,
		                                     "oncomplete","OnCamMoveComplete",
		                                     "oncompletetarget",gameObject,
		                                     "easetype","easeInOutSmoothBreak"));
		ShowOutro ();
		OnOutroEnd += KeepOn;
	}
	void KeepOn()
	{
		OnOutroEnd -= KeepOn;
		gameObject.SetActive (true);
	}
	void OnCamMoveComplete()
	{
		gameObject.SetActive (false);
		if (OnFinishCamMove != null)
			OnFinishCamMove ();
	}

	public void MyuuJournalHighlighted()
	{
		myuuOpeningJournal.MyuuHighlight ();
	}
	public void MyuuJournalUnhighlighted()
	{
		myuuOpeningJournal.MyuuUnhighlight ();
	}

	public void MyuuJournalClicked()
	{
		myuuOpeningJournal.MyuuClicked ("AUI_MyuuJournal_Select1");
		cameraBlurer.faderBlur.FadeToColor (0.5f,2.5f);
		ShowOutro ();
		cameraBlurer.faderBlur.OnFadeOutCompleted += MyuuJournalFinishFade;
	}
	void MyuuJournalFinishFade()
	{
		cameraBlurer.faderBlur.OnFadeOutCompleted -= MyuuJournalFinishFade;
		if (OnMyuuJournal != null)
			OnMyuuJournal ();
		gameObject.SetActive (true);
		StartCoroutine(LoadSceneAsync ("JournalUI"));
	}

	IEnumerator LoadSceneAsync(string sceneToLoad)
	{
		AsyncOperation ao = SceneManager.LoadSceneAsync(sceneToLoad);
		while (!ao.isDone) {
			yield return null;
		}
//		print ("Journal Loaded");
		//move active item to current level;
		Scene activescene = SceneManager.GetSceneByName(sceneToLoad);
		SceneManager.SetActiveScene(activescene);
		gameObject.SetActive (false);

		cameraBlurer.faderBlur.FadeToClear (0.3f);
	}
}
