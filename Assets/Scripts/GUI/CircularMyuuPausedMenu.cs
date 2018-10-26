using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class CircularMyuuPausedMenu : CircularBaseMenu {
	public delegate void CircularMyuuPausedMenuDelegate ();

	public event CircularMyuuPausedMenuDelegate OnGoMainMenu;
	public event CircularMyuuPausedMenuDelegate OnResume;

	public CircularSettingsMenu settingsMenu;
	public DialogMenu dialogMenu;
	public bool newGame = false;

	public override void SetItemAction() 
	{
		base.SetItemAction ();
		OnEscapeAction += ResumeClicked;
		itemList [0].OnItemClicked += ResumeClicked;
		itemList [1].OnItemClicked += MainMenuClicked;
		itemList [2].OnItemClicked += BackClicked;
		itemList [3].OnItemClicked += SettingsClicked;
	}
	void MainMenuClicked()
	{
		newGame = false;
		cameraBlurer.ToBlur (transform);
		dialogMenu.ShowDialog ("Unsaved progress will be lost.\nAre you sure?");
		dialogMenu.OnOutroStart += StartReturnBlur;
		dialogMenu.OnOutroEnd += EndReturnBlur;
		dialogMenu.OnOutroEnd += NewYes;
	}

	void ResumeClicked()
	{
		newGame = false;
		ShowOutro ();
		cameraBlurer.BlurOut (transform);
		if(OnResume!=null)
			OnResume();
		
	}

	void BackClicked()
	{
		newGame = false;
		cameraBlurer.ToBlur (transform);
		dialogMenu.ShowDialog ("Return to Journal?");
		dialogMenu.OnOutroStart += StartReturnBlur;
		dialogMenu.OnOutroEnd += EndReturnBlur;
		dialogMenu.OnOutroEnd += BackYes;
	}

	void SettingsClicked()
	{
		newGame = false;
		settingsMenu.ShowIntro (this);
		cameraBlurer.ToBlur (transform);   
	}

	void StartReturnBlur()
	{
		dialogMenu.OnOutroStart -= StartReturnBlur;
		cameraBlurer.FromBlur (transform);
	}
	void EndReturnBlur()
	{
		dialogMenu.OnOutroEnd -= EndReturnBlur;
	}

	void NewYes()
	{
		dialogMenu.OnOutroEnd -= NewYes;
		if (dialogMenu.selectionYes) {
			ShowOutro();
			cameraBlurer.BlurOut(transform);
			cameraBlurer.faderBlur.FadeToColor();
			cameraBlurer.faderBlur.OnFadeOutCompleted += AfterNew;
		}
	}
	void AfterNew()
	{
		cameraBlurer.faderBlur.OnFadeOutCompleted -= AfterNew;
		newGame = true;
		GameDataManager.shared ().LOCAL_IsLoadOpening = true;

		SceneManager.LoadScene (GameDataManager.shared().PlayerLocation.sceneName);


		GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;

		if(OnGoMainMenu !=null)
			OnGoMainMenu();

	}

	void BackYes()
	{
		dialogMenu.OnOutroEnd -= BackYes;
		if (dialogMenu.selectionYes) {
			ShowOutro();
			GUIMenu.shared ().camera.SetCamFoV (GUICamZoom.Original);
			cameraBlurer.BlurOut(transform);
			cameraBlurer.faderBlur.FadeToColor();
			cameraBlurer.faderBlur.OnFadeOutCompleted += AfterBack;
		}
	}
	void AfterBack()
	{
		cameraBlurer.faderBlur.OnFadeOutCompleted -= AfterBack;
		Scene activescene = SceneManager.GetSceneByName("JournalUI");
		SceneManager.SetActiveScene(activescene);
		SceneManager.UnloadScene (GameDataManager.shared ().LOCAL_SceneToUnLoad);
	}

	void KeepOn()
	{
		OnOutroEnd -= KeepOn;
		gameObject.SetActive (true);
	}
}
