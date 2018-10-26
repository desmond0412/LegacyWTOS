using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MyuuJournalController : MonoBehaviour {
	public delegate void MyuuJournalControllerDelegate ();
	public event MyuuJournalControllerDelegate OnSceneLoaded;
	public event MyuuJournalControllerDelegate OnJournalClosed;

	public MyuuJournalButton parallaxPage1;
	public MyuuJournalButton parallaxPage2;
	public MyuuJournalButton parallaxButton;
	public MyuuJournalButton exitButton;

	public ColorFader fader;
	public MyuuModelBase myuuModelBase;

	void Start () {
		parallaxButton.OnClick += ParallaxClicked;
		exitButton.OnClick += ExitClicked;
		myuuModelBase.OnFinishIntro += FinishIntro;
		GUIMenu.shared ().camera.SaveCamFoV ();
		GUIMenu.shared ().isIdle = false;
	}
	void FinishIntro()
	{
		parallaxPage1.StartIntro ();
		parallaxPage2.StartIntro ();
		parallaxButton.StartIntro ();
	}

	void ParallaxClicked()
	{
		SetButtons(false);
		fader.FadeToColor ();
		fader.OnFadeOutCompleted += LoadParallax;
	}
	void LoadParallax()
	{
		fader.OnFadeOutCompleted -= LoadParallax;
		StartCoroutine(LoadSceneAsync("Parallax"));
		GameDataManager.shared ().LOCAL_SceneToUnLoad = "Parallax";
		OnSceneLoaded += SetParallax;

	}
	void SetParallax()
	{
		OnSceneLoaded -= SetParallax;
		gameObject.SetActive (false);
		GameObject g = GameObject.FindWithTag("MyuuContentBase");
		if (g != null) {
			MyuuContentBase mcb = g.GetComponent<MyuuContentBase>();
			mcb.OnSceneEnd += ContentEnded;
		}
	}

	IEnumerator LoadSceneAsync(string sceneToLoad)
	{
		AsyncOperation ao = SceneManager.LoadSceneAsync(sceneToLoad,LoadSceneMode.Additive);
		while (!ao.isDone) {
			yield return null;
		}
		//move active item to current level;
		Scene activescene = SceneManager.GetSceneByName(sceneToLoad);
		SceneManager.SetActiveScene(activescene);
		
		fader.FadeToClear ();
		fader.OnFadeInCompleted += AfterLoad;
		if (OnSceneLoaded != null)
			OnSceneLoaded ();
	}

	void AfterLoad()
	{
		fader.gameObject.SetActive (false);
	}

	void ContentEnded()
	{
		GameObject g = GameObject.FindWithTag("MyuuContentBase");
		if (g != null) {
			MyuuContentBase mcb = g.GetComponent<MyuuContentBase>();
			mcb.OnSceneEnd -= ContentEnded;
		}
		gameObject.SetActive (true);
//		print ("heloooo");
		myuuModelBase.myuuAnim.SetTrigger ("SetClose");
        if (!GUIMenu.shared().myuuPausedMenu.newGame) {
			GUIMenu.shared ().fader.gameObject.SetActive (false);
            fader.SetFadeColor(ColorFaderType.Full);
            fader.FadeToClear ();
            fader.OnFadeInCompleted += FinishFader;
        } else {
			GUIMenu.shared ().fader.gameObject.SetActive (true);
        }
	}
	void FinishFader()
	{
		fader.OnFadeInCompleted -= FinishFader;
		SetButtons (true);
	}

	void ExitClicked()
	{
		SetButtons (false);
		parallaxPage1.StartOutro ();
		parallaxPage2.StartOutro ();
		parallaxButton.StartOutro ();
		myuuModelBase.myuuAnim.SetTrigger ("OutroClose");
		myuuModelBase.OnFinishExit += FinishOutroClose;
	}
	void FinishOutroClose()
	{
		myuuModelBase.OnFinishExit -= FinishOutroClose;
		GameDataManager.shared ().LOCAL_IsLoadOpening = true;
		GUIMenu.shared ().fader.SetFadeColor(ColorFaderType.Full);
		GUIMenu.shared ().fader.gameObject.SetActive (true);
		if (GameDataManager.shared ().PlayerLocation != null)
			SceneManager.LoadScene (GameDataManager.shared ().PlayerLocation.sceneName);
		else 
			print ("Null lhoooo");
//		StartCoroutine (LoadingScreen.DirectLoadSceneAsync(GameDataManager.shared ().LOCAL_NextSceneFromLoadScreen));
		if (OnJournalClosed != null)
			OnJournalClosed ();
	}


	void SetButtons(bool state)
	{
		parallaxButton.buttonEnabled = state;
		exitButton.buttonEnabled = state;
	}

	void OnDestroy()
	{
		GameObject g = GameObject.FindWithTag("MyuuContentBase");
		if (g != null) {
			MyuuContentBase mcb = g.GetComponent<MyuuContentBase>();
			mcb.OnSceneEnd -= ContentEnded;
		}
		myuuModelBase.OnFinishIntro -= FinishIntro;
		GUIMenu.shared ().fader.SetFadeColor(ColorFaderType.Full);
		GUIMenu.shared ().fader.gameObject.SetActive (true);
		parallaxButton.OnClick -= ParallaxClicked;
		exitButton.OnClick -= ExitClicked;
	}
}
