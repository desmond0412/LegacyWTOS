using UnityEngine;
using System.Collections;

public class CinematicCall : MonoBehaviour {

	public delegate void CinematicCallDelegate ();

	public event CinematicCallDelegate OnMovieFinish;
	public event CinematicCallDelegate OnMovieStart;

	public GameObject moviePrefab;
	public string filename;
	public float audioVolume = 1f;

	float fadeTime = 1f;

	MediaPlayerCtrl movie;
	void Start(){
		GameObject x = Instantiate (moviePrefab) as GameObject;
		movie = x.GetComponent<MediaPlayerCtrl> ();
		movie.Load(filename);
	}

	public void PlayMovie(){

		if(GUIMenu.shared()!=null)
			GUIMenu.shared().isAccessible = false;

		movie.Play ();
        movie.SetVolume(GameDataManager.shared().CONFIG_Volume / 100.0f * audioVolume);
		StartCoroutine (FadeAlpha ());

		movie.OnEnd += MovieFinish;

		MainObjectSingleton.shared (MainObjectType.Player).GetComponent<LevController> ().setInputEnable (false);
		MainObjectSingleton.shared (MainObjectType.Player).GetComponent<LevController> ().stopMoving ();

		if (OnMovieStart != null)
			OnMovieStart ();
	}

	IEnumerator FadeAlpha(){
		float timeDamn = .8f;
		float waktu = 0f;
		UnityEngine.UI.RawImage temp = movie.GetComponentInChildren<UnityEngine.UI.RawImage> ();
		while (waktu < fadeTime) {
			waktu += Time.deltaTime;
			if (waktu < timeDamn)
				temp.color = new Color (0,0,0, waktu / fadeTime);
			else
				temp.color = new Color (1,1,1, waktu / fadeTime);
//			temp.color = new Color (temp.color.r, temp.color.g, temp.color.b, waktu / fadeTime);
			yield return null;
		}
	}


	void MovieFinish(){
		if(GUIMenu.shared()!=null)
			GUIMenu.shared().isAccessible = true;
		if (OnMovieFinish != null)
			OnMovieFinish ();
		movie.OnEnd -= MovieFinish;
	}
}
