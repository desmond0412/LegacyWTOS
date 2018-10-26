using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Artoncode.Core.UI;
using Artoncode.Core;
using UnityEngine.SceneManagement;

public class LoadingScreen : ObjectSingleton<LoadingScreen> {
	public delegate void ContentDelegate ();
//	public event ContentDelegate OnSceneStart;
//	public event ContentDelegate OnSceneEnd;
	
	public GameObject bg;
	public GameObject front;
	public ColorFader fader;
	public TextMeshProUGUI progressText;

	public float fadeTime = 1f;

	string toLoad;
	string toUnload;
	AsyncOperation ao;
	bool prevMenuState = false;
	protected override void OnInit ()
	{
		base.OnInit ();
	}

	public void Start()
	{
		bg.SetActive (true);
		front.SetActive (true);
		fader.gameObject.SetActive (true);
		progressText.gameObject.SetActive (true);
		if (GUIMenu.shared () != null) {
			prevMenuState = GUIMenu.shared ().isIdle;
			GUIMenu.shared ().isIdle = false;
		}
		LoadScene (GameDataManager.shared ().LOCAL_NextSceneFromLoadScreen);
	}

	public void LoadScene (string sceneToLoad)
	{
		toLoad = sceneToLoad;
		gameObject.transform.SetAsLastSibling();
		fader.FadeToClear (fadeTime);
		fader.OnFadeInCompleted += OnFinishFadeIn;
		progressText.text = "0%";

	}
	void OnFinishFadeIn()
	{
		fader.OnFadeInCompleted -= OnFinishFadeIn;
		StartCoroutine (LoadSceneAsync (toLoad));
	}

	private IEnumerator LoadSceneAsync(string sceneToLoad)
	{
		ao = SceneManager.LoadSceneAsync(sceneToLoad);
		ao.allowSceneActivation = false;
		while (ao.progress<0.9f) {
			progressText.text = (ao.progress * 100).ToString("n0") + "%";
			yield return null;
		}

		progressText.text = "100%";
		//move active item to current level;
		Scene activescene = SceneManager.GetSceneByName(sceneToLoad);
		SceneManager.SetActiveScene(activescene);

		fader.FadeToColor (fadeTime);
		fader.OnFadeOutCompleted += OnFinishFadeOut;
	}
	void OnFinishFadeOut()
	{
		fader.OnFadeOutCompleted -= OnFinishFadeOut;
		ao.allowSceneActivation = true;
	}
	public static IEnumerator DirectLoadSceneAsync(string sceneToLoad)
	{
		AsyncOperation asop = SceneManager.LoadSceneAsync(sceneToLoad);
		while (!asop.isDone) {
			yield return null;
		}		
		Scene activescene = SceneManager.GetSceneByName(sceneToLoad);
		SceneManager.SetActiveScene(activescene);	
	}
	void OnDestroy()
	{
		if (GUIMenu.shared () != null)
			GUIMenu.shared ().isIdle = prevMenuState;

	}
}
