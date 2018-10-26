using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LevelController : GroupObjectSingleton<LevelController>
{
	public static List<string> loadedLevel = new List<string> ();
	public string currentLevelName;
	public List<string> preloadedLevel;

	private List<string> levelToLoad;

	public void Awake ()
	{
		#if UNITY_EDITOR
		if (string.IsNullOrEmpty (currentLevelName)) {
			Debug.LogError ("LEVEL TRIGGER NEEDS LEVEL NAME TO BE SET UP");
		}
		#endif

		// AUTO ADDED this level to loadedLevel
		if (!loadedLevel.Contains (currentLevelName))
			loadedLevel.Add (currentLevelName);
		PreloadLevel ();

		OnConstruct (currentLevelName);
//		this.gameObject.SetActive (IsActiveScene ());
	}


	void PreloadLevel ()
	{
		levelToLoad = preloadedLevel;
		// AUTO ADDED this level to leveltoLoad
		if (!levelToLoad.Contains (currentLevelName))
			levelToLoad.Add (currentLevelName);

		//load adjacent level
		IEnumerable<string> sceneToLoad = levelToLoad.Except (loadedLevel);
		StartCoroutine (LoadScene (sceneToLoad));

		foreach (string	item in levelToLoad) {
			if (!loadedLevel.Contains (item))
				loadedLevel.Add (item);
		}

//		this.print ("loaded Level:", loadedLevel.ToArray ());
	}

	#region LoadScene Coroutine

	private IEnumerator LoadScene (IEnumerable<string> sceneToLoad)
	{
//		List<AsyncOperation> asyncOp = new List<AsyncOperation> ();

		foreach (string scene in sceneToLoad) {
			SceneManager.LoadSceneAsync (scene, LoadSceneMode.Additive);
//			AsyncOperation op = SceneManager.LoadSceneAsync (scene, LoadSceneMode.Additive);
//			op.allowSceneActivation = false;
//			asyncOp.Add (op);
		}	

//		foreach (AsyncOperation op in asyncOp) {
//			while (op.progress < 0.9f) {
//				yield return null;	
//			}
//		}
//
//		foreach (AsyncOperation op in asyncOp) {
//			op.allowSceneActivation = true;
//		}
//
//		asyncOp.Clear ();
		Resources.UnloadUnusedAssets ();
		yield return null;	
	}

	#endregion

	#region helper

	private bool IsActiveScene ()
	{
		return currentLevelName == SceneManager.GetActiveScene ().name;
	}

	private void print (string x, string[] s)
	{
		string ss = "";
		foreach (var item in s) {
			ss += item;
		}

		Debug.Log (x + " " + ss);
	}
	#endregion

	public static void ActivateLevel (List<string> levelNames)
	{
		foreach (string levelName in levelNames) {
			LevelController.shared (levelName).gameObject.SetActive (true);
		}

		List<string> keys = new List<string> (LevelController._instance.Keys);
		IEnumerable<string> otherLevel = keys.Except (levelNames);

		foreach (string levelName in otherLevel) {
			LevelController.shared (levelName).gameObject.SetActive (false);
		}

	}


	public void OnDestroy ()
	{
		OnDestruct(currentLevelName);
		loadedLevel.Clear ();

	}



	[ContextMenu ("GetActiveLevelName")]
	void GetLevelName ()
	{
		currentLevelName = SceneManager.GetActiveScene ().name;
	}
	
	
}
