using UnityEngine;
using System.Collections;
using Artoncode.Core.UI;
using UnityEngine.SceneManagement;

public class LevelEndSceneManager : MonoBehaviour {

	public LevController lev;
	public SceneSplasher splash;
	public SceneFader fader;
	public string nextScene;


	void Start()
	{
		lev.playCustomAnimation("MAINMENU.AC_Lev_Closing");
		StartCoroutine(DelayEnd());
	}

	IEnumerator DelayEnd()
	{
		yield return new WaitForEndOfFrame();
		lev.setInputEnable(false);

		yield return new WaitForSeconds(14);
		//SHOULD NOT RESET ALL!!
		GameDataManager.shared ().Reset();
		//SHOULD NOT RESET ALL!!

		GameDataManager.shared ().LOCAL_IsLoadOpening = true;
		print(GameDataManager.shared ().MyuuJournalState.ToString());
		if(GameDataManager.shared ().MyuuJournalState == MyuuJournalState.None)
		{
			GameDataManager.shared ().MyuuJournalState = MyuuJournalState.First;
			GameDataManager.shared ().Save(GameDataManager.GameDataType.MyuuData);
		}
		
		GameDataManager.shared ().SaveGameExist  = false;
        GameDataManager.shared ().PlayerLocation =  new LocationModel(nextScene,LocationType.Start);

		SceneManager.LoadScene (nextScene);    
		MainObjectSingleton.DestoryAllInstance();
	}

//	void Start()
//	{
//		splash.OnSceneSplashEndCompleted += Splash_OnSceneSplashEndCompleted;
//		fader.OnFadeInCompleted += Fader_OnFadeInCompleted;
//		fader.OnFadeOutCompleted += Fader_OnFadeOutCompleted;
//		fader.FadeInStart();
//	}
//
//	void Fader_OnFadeInCompleted ()
//	{
//		splash.Play();
//
//	}
//	void Splash_OnSceneSplashEndCompleted ()
//	{
//		fader.FadeOutStart();
//	}
//
//	void Fader_OnFadeOutCompleted ()
//	{
//		fader.OnFadeOutCompleted -= Fader_OnFadeOutCompleted;
//		GameDataManager.shared ().Reset();
//		GameDataManager.shared ().LOCAL_IsLoadOpening = true;
//		GameDataManager.shared ().SaveGameExist = false;
//		SceneManager.LoadScene (nextScene);    
//		MainObjectSingleton.DestoryAllInstance();
//	}
//
//	void OnDestroy()
//	{
//		splash.OnSceneSplashEndCompleted -= Splash_OnSceneSplashEndCompleted;
//	}

}
