using UnityEngine;
using System.Collections;

public class L02SnowParticles : MonoBehaviour {

	GameObject lev;
	public CinematicCall cinematicCaller;

	IEnumerator Start(){
		while (!lev){
			if (MainObjectSingleton.shared (MainObjectType.Player) != null)
				lev = MainObjectSingleton.shared (MainObjectType.Player).gameObject;
			yield return null;
		}

		cinematicCaller.OnMovieStart += HandlerCinematicCaller_OnMovieStart;
	}

	void HandlerCinematicCaller_OnMovieStart ()
	{
		cinematicCaller.OnMovieStart -= HandlerCinematicCaller_OnMovieStart;
		Destroy (gameObject, 1.5f);

//		this.gameObject.SetActive (false);
	}

	void Update(){
		if (lev)
			transform.position = lev.transform.position;
	}
}
