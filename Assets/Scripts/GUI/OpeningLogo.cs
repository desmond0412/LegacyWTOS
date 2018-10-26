using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class OpeningLogo : MonoBehaviour {
	public delegate void OpeningLogoDelegate ();
	
	public event OpeningLogoDelegate OnFinishOpening;

	public ColorFader fader;
	public CameraBlurer camBlur;

	public Vector3 deltaCamPos;
	public Vector2 deltaOpLogo;
	Vector3 endCamPos;
	public float camTime;
	public float camDelay;

	public bool openingRun = false;

	void Awake()
	{
		transform.localScale = new Vector3 (0.95f, 0.95f, 0.95f);
		camBlur.SetBlur (true);
	}

	public void StartOpening(float delay)
	{
		openingRun = false;
		transform.localScale = new Vector3 (0.95f, 0.95f, 0.95f);
		camBlur.SetBlur (false);
		GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;
//		print ("Fader State: "+fader.gameObject.activeSelf);
		StartCoroutine (StartOpeningDelayed (delay));
//		print ("OPmenu cur pos "+GetComponent<RectTransform> ().anchoredPosition);
	}
	IEnumerator StartOpeningDelayed(float delay)
	{
		yield return new WaitForSeconds (delay);

//		camBlur.BlurOut (0.7f);
		GUIMenu.shared ().camera.SetCamFoV (GUICamZoom.In);
		GUIMenu.shared ().camera.SetCamPos (GUICamPos.Up);
		iTween.ScaleTo (gameObject, iTween.Hash ("scale",Vector3.one,
		                                         "time",2f,
		                                         "ignoretimescale",(Time.timeScale==0f),
		                                         "oncomplete","OnBlurOutFinished",
		                                         "easetype","easeInOutSmoothBreak"));
//		camBlur.OnBlurOutEnd += OnBlurOutFinished;

	}
	void OnBlurOutFinished()
	{
		camBlur.OnBlurOutEnd -= OnBlurOutFinished;
		openingRun = true;
		fader.FadeToClear (1f, 2f);
		GUIMenu.shared ().camera.MoveCamPos (GUICamPos.Center,camTime,camDelay,()=>OnCamMoveComplete());

		//MOVE OPENING LOGO
		iTween.ValueTo(gameObject, iTween.Hash ("name","OPLogoMove",
												"from",Vector2.zero,
		                                        "to",deltaOpLogo,
		                                        "time",camTime - 0.1f,
		                                        "delay",camDelay,
		                                        "ignoretimescale",(Time.timeScale==0f),
		                                        "onupdate","UpdateOPLogo",
		                                        "easetype","easeInOutSmoothBreak"));
	}
	void UpdateOPLogo(Vector2 newPos)
	{
		GetComponent<RectTransform> ().anchoredPosition = newPos;
	}

	void OnCamMoveComplete()
	{
		openingRun = false;
//		iTween.StopByName ("OPLogoMove");
		gameObject.SetActive (false);
		if (OnFinishOpening != null)
			OnFinishOpening ();
	}

	public void FinishOpeningRun()
	{
		iTween.Stop ();
		fader.SetFadeColor (ColorFaderType.Clear);
		iTween.Stop (fader.gameObject);
		GUIMenu.shared ().camera.SetCamPos (GUICamPos.Center);
		GetComponent<RectTransform> ().anchoredPosition = deltaOpLogo;
		OnCamMoveComplete ();
	}
}
