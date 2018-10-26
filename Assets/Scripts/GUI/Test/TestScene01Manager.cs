using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TestScene01Manager : MonoBehaviour {

	LoadingScreen loadingScreen;

	void Awake () {
		GUIMenu.shared ().AnimateOpening ();
	}
}
