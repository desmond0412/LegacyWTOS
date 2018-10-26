using UnityEngine;
using System.Collections;
using Artoncode.Core.UI;
using UnityEngine.SceneManagement;

public class SplashSceneManager : MonoBehaviour {

    public SceneSplasher splash;
    public SceneFader fader;
    public string nextScene;
    
	public AsyncOperation op;

    void Start()
    {
        splash.OnSceneSplashEndCompleted += Splash_OnSceneSplashEndCompleted;
        fader.OnFadeInCompleted += Fader_OnFadeInCompleted;
        fader.OnFadeOutCompleted += Fader_OnFadeOutCompleted;
        fader.FadeInStart();
		LoadNextScene();
    }

	void LoadNextScene()
	{
		GameDataManager.shared ().LOCAL_IsLoadOpening = true;
		//		GameDataManager.shared ().LOCAL_NextSceneFromLoadScreen = nextScene;

		if (GameDataManager.shared ().PlayerLocation != null) {
			//			GameDataManager.shared ().LOCAL_NextSceneFromLoadScreen = GameDataManager.shared ().PlayerLocation.sceneName;
			nextScene = GameDataManager.shared ().PlayerLocation.sceneName;
		} else {
            GameDataManager.shared ().PlayerLocation = new LocationModel(nextScene,LocationType.Start);
		}
        GameDataManager.shared ().PlayerLocation.location = LocationType.Start;

		//SKIP THE LOADING SCREEN
		op = SceneManager.LoadSceneAsync (nextScene);    
		op.allowSceneActivation = false;
	}

    void Fader_OnFadeInCompleted ()
    {
        splash.Play();

    }
    void Splash_OnSceneSplashEndCompleted ()
    {
        fader.FadeOutStart();
    }

    void Fader_OnFadeOutCompleted ()
    {
        fader.OnFadeOutCompleted -= Fader_OnFadeOutCompleted;
		op.allowSceneActivation = true;
		MainObjectSingleton.DestoryAllInstance();

    }

    void OnDestroy()
    {
        splash.OnSceneSplashEndCompleted -= Splash_OnSceneSplashEndCompleted;
    }

//
}
