using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Artoncode.Core.UI;
using UnityEngine.SceneManagement;

public class SplashSceneManagerTemp : MonoBehaviour {

	public SceneSplasher splash;
	public SceneFader fader;
	public Canvas splashCanvas;
	public string nextScene;
		
	void Start()
	{
		splash.OnSceneSplashEndCompleted += Splash_OnSceneSplashEndCompleted;
		fader.OnFadeInCompleted += Fader_OnFadeInCompleted;
		fader.OnFadeOutCompleted += Fader_OnFadeOutCompleted;
		fader.FadeInStart();
	}

	void Fader_OnFadeInCompleted ()
	{
		splash.Play();
		fader.OnFadeInCompleted -= Fader_OnFadeInCompleted;

	}
	void Splash_OnSceneSplashEndCompleted ()
	{
		fader.FadeOutStart();
	}

	void Fader_OnFadeOutCompleted ()
	{
		fader.OnFadeOutCompleted -= Fader_OnFadeOutCompleted;
		splashCanvas.gameObject.SetActive (false);
		GameDataManager.shared ().LOCAL_IsLoadOpening = true;
		GameDataManager.shared ().LOCAL_NextSceneFromLoadScreen = nextScene;
		SceneManager.LoadScene ("LoadingScreen");
	}
	void OnDestroy()
	{
		splash.OnSceneSplashEndCompleted -= Splash_OnSceneSplashEndCompleted;
	}

}
