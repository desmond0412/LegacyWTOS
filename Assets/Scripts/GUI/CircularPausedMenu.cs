using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class CircularPausedMenu : CircularBaseMenu {
	public delegate void CircularPausedMenuDelegate ();

	public event CircularPausedMenuDelegate OnFinishCamMove;
	public event CircularPausedMenuDelegate OnGoMainMenu;
	public event CircularPausedMenuDelegate OnResumeGame;



	public CircularSettingsMenu settingsMenu;
	public Vector3 deltaCamPos;
	public DialogMenu dialogMenu;
	public override void SetItemAction() 
	{
		base.SetItemAction ();
		OnEscapeAction += ResumeClicked;
		itemList [0].OnItemClicked += ResumeClicked;
		itemList [1].OnItemClicked += MainMenuClicked;
		itemList [2].OnItemClicked += QuitClicked;
		itemList [3].OnItemClicked += SettingsClicked;
	}
	void MainMenuClicked()
	{
		cameraBlurer.ToBlur (transform);
		dialogMenu.ShowDialog ("Unsaved progress will be lost.\nAre you sure?");
		dialogMenu.OnOutroStart += StartReturnBlur;
		dialogMenu.OnOutroEnd += EndReturnBlur;
		dialogMenu.OnOutroEnd += NewYes;
	}

	void ResumeClicked()
	{
		ShowOutro ();
		cameraBlurer.BlurOut (transform);
		if(OnResumeGame!=null)
			OnResumeGame();
		
	}

	void QuitClicked()
	{
		cameraBlurer.ToBlur (transform);
		dialogMenu.ShowDialog ("Unsaved progress will be lost.\nAre you sure?");
		dialogMenu.OnOutroStart += StartReturnBlur;
		dialogMenu.OnOutroEnd += EndReturnBlur;
		dialogMenu.OnOutroEnd += QuitYes;
	}

	void SettingsClicked()
	{
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
		MainObjectSingleton.shared (MainObjectType.Camera).GetComponent<Artoncode.Core.SmoothFollow> ().enabled = false;
		if (dialogMenu.selectionYes) {
			Vector3 newPos = Camera.main.transform.position + deltaCamPos;
			MoveCam (newPos);
			OnFinishCamMove += AfterNew;
		}
	}
	void AfterNew()
	{
		OnFinishCamMove -= AfterNew;
		GameDataManager.shared ().LOCAL_IsLoadOpening = true;

		SceneManager.LoadScene (GameDataManager.shared().PlayerLocation.sceneName);

		cameraBlurer.faderBlur.transform.SetAsFirstSibling();
		GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;

		if(OnGoMainMenu !=null)
			OnGoMainMenu();

	}

	void QuitYes()
	{
		dialogMenu.OnOutroEnd -= QuitYes;
		MainObjectSingleton.shared (MainObjectType.Camera).GetComponent<Artoncode.Core.SmoothFollow> ().enabled = false;
		if (dialogMenu.selectionYes) {
			cameraBlurer.BlurOut(transform);
			Vector3 newPos = Camera.main.transform.position - deltaCamPos;
			MoveCam (newPos);
			OnFinishCamMove += AfterQuit;
		}
	}
	void AfterQuit()
	{
		OnFinishCamMove -= AfterQuit;
		Application.Quit ();
	}
	void MoveCam(Vector3 newPos)
	{
		cameraBlurer.faderBlur.FadeToColor (1.25f);
		GameObject mainCam = Camera.main.gameObject;
		iTween.MoveTo (mainCam, iTween.Hash ("position",newPos,
		                                     "time",2f,
		                                     "delay",0f,
		                                     "oncomplete","OnCamMoveComplete",
		                                     "oncompletetarget",gameObject,
		                                     "ignoretimescale",(Time.timeScale==0f),
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
}
