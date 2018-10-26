using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Artoncode.Core.UI;
using Artoncode.Core;
using TMPro;
using SmartLocalization;
//
//
//public class LerpCoroutine
//{
//	public delegate void LerpCoroutineDelegate();
//	public delegate void LerpCoroutineDelegateUpdate(float percentage);
//	public static event LerpCoroutineDelegate OnStart;
//	public static event LerpCoroutineDelegateUpdate OnUpdate;
//	public static event LerpCoroutineDelegate OnEnded;
//
//	public static IEnumerator Update(float duration)
//	{
//		float startTime = Time.time;
//		float percentage = 0;
//
//		if(OnStart!=null)	OnStart();
//		
//		while(percentage < 1.0f)
//		{
//			percentage = Mathf.Clamp01((Time.time - startTime)/duration);
//			if(OnUpdate!=null)	OnUpdate(percentage);
//			
//			yield return new WaitForEndOfFrame();
//		}
//
//		if(OnEnded!=null)	OnEnded();
//	}
//
//}

public class GameoverCameraManager : MonoBehaviour {

	public delegate void GameoverCameraManagerDelegate(GameObject sender);
	public event GameoverCameraManagerDelegate OnBlackoutEnded;
	public event GameoverCameraManagerDelegate OnTextComesOutEnded;


	public Image blackFader;
	public TextMeshProUGUI textMeshPro;
	public float gameoverTextHoldTime = 2;

	[HideInInspector]
//	public SceneFader sceneFader;

	void Start()
	{
		transform.position 					= Camera.main.transform.position;
		transform.rotation					= Camera.main.transform.rotation;

		gameObject.GetComponent<Camera>().enabled = false;
		gameObject.GetComponent<Camera>().fieldOfView = Camera.main.fieldOfView;
//		gameObject.AddComponent<SmoothFollow>(Camera.main.GetComponent<SmoothFollow>());

//		sceneFader = SceneFader.shared();
	}

	#region Blackout
	public void ShowBlackOut(float transition = 2)
	{
		//Start For Black Animation
		blackFader.color = Color.clear;
		StartCoroutine(UpdateBlackOut(transition));
	}

	public IEnumerator UpdateBlackOut(float duration)
	{
		float startTime = Time.time;
		float percentage = 0;
		Color startColor = Color.clear;
		Color endColor = Color.black;


		while(percentage < 1.0f)
		{
			percentage = Mathf.Clamp01((Time.time - startTime)/duration);
			blackFader.color = Color.Lerp(startColor,endColor,percentage);
			yield return new WaitForEndOfFrame();
		}
		blackFader.color = Color.black;
		if(OnBlackoutEnded != null)
			OnBlackoutEnded (this.gameObject);
	}

	#endregion 



	#region TextComesOut
	public void ShowText(string key, float transition = 2)
	{
		textMeshPro.text = LanguageManager.Instance.GetTextValue(key);
		textMeshPro.color = Color.clear;

		//Start For Font Animation
		StartCoroutine(UpdateTextOut(transition));
	}

	public IEnumerator UpdateTextOut(float duration)
	{
		float startTime = Time.time;
		float percentage = 0;
		Color startColor = Color.clear;
		Color endColor = Color.white;

		while(percentage < 1.0f)
		{
			percentage = Mathf.Clamp01((Time.time - startTime)/duration);
			textMeshPro.color = Color.Lerp(startColor,endColor,percentage);
			yield return new WaitForEndOfFrame();
		}

		yield return new WaitForSeconds(gameoverTextHoldTime);

		textMeshPro.color = Color.white;
		if(OnTextComesOutEnded != null)
			OnTextComesOutEnded(this.gameObject);
	}


	#endregion 



}
