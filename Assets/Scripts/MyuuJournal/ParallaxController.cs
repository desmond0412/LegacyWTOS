using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum ParallaxType {
	Present,
	Old
}

[System.Serializable]
public struct SpriteArray {
	public Sprite[] sprite;
}

public class ParallaxController : MonoBehaviour {
	public RectTransform[] layer; 
	public ParticleSystem[] particleYellow;
	public ParticleSystem[] particleGreen;
	public Vector2[] sizeDelta;
	public bool scaleChange;
	public float inertia;
	private Color clearColor;

//	public ParallaxType pType;
	Vector2 oriMousePos = Vector2.zero;
	Vector2[] targetPos;
	bool paused = false;
	int activeBG;
	float timeLayer;

	void Awake() {
		clearColor = new Color (1f, 1f, 1f, 0f);
		oriMousePos = (Vector2)Input.mousePosition;
		targetPos = new Vector2[layer.Length];
		for (int i=1; i<layer.Length; i++) {
			targetPos[i] = layer[i].anchoredPosition;
		}
		paused = false;
		if(GUIMenu.shared () !=null)
		{
			GUIMenu.shared ().myuuPausedMenu.OnIntroStart += OnPaused ;
			GUIMenu.shared ().myuuPausedMenu.OnResume += OnResume ;
		}
//		SetBG (ParallaxType.Present);
	}

	void Start()
	{
		GlobalAudioSingleton.shared(GlobalAudioType.InGameMusic).Play();
	}

	public void SetBG(ParallaxType pt)
	{
		for (int i=0; i<layer.Length; i++) {
//			layer[i].GetComponent<Image>().sprite = spriteBG[(int)pt].sprite[i];
		}
	}
	public void ChangeBGGradually(float timeEachLayer)
	{
		activeBG = 0;
		timeLayer = timeEachLayer;
		iTween.ValueTo (gameObject, iTween.Hash ("from",Color.white,
		                                         "to",clearColor,
		                                         "time",timeLayer,
		                                         "onupdate","UpdateFade",
		                                         "oncomplete","OnEndFade",
		                                         "easetype","easeInOutSmoothBreak"));
		if (particleYellow [activeBG] != null) {
			particleYellow [activeBG].Stop();
			particleGreen [activeBG].Play();
		}

	}
	void OnEndFade()
	{
		activeBG++;
		if (activeBG < layer.Length) {
			iTween.ValueTo (gameObject, iTween.Hash ("from", Color.white,
		                                         "to", clearColor,
		                                         "time", timeLayer,
		                                         "onupdate", "UpdateFade",
		                                         "oncomplete", "OnEndFade",
		                                         "easetype", "easeInOutSmoothBreak"));
			if (particleYellow [activeBG] != null) {
				particleYellow [activeBG].Stop();
				particleGreen [activeBG].Play();
			}
		}
	}

	void UpdateFade(Color c)
	{
		layer[activeBG].transform.GetChild(0).GetComponent<Image>().color = c;
	}

	void Update () {
//		SetBG (pType);
		if (!paused) {
			if (scaleChange) {
				for (int i=0; i<sizeDelta.Length; i++) {
					layer [i].localScale = Vector3.one;
					layer [i].sizeDelta = GetLayerDeltaSize (i);
				}
			} else {
				for (int i=0; i<sizeDelta.Length; i++) {
					layer [i].localScale = new Vector3 (1.5f, 1.5f, 1f);
					layer [i].sizeDelta = Vector2.zero;
				}
			}

			Vector2 curPos = (Vector2)Input.mousePosition;
			float scaleDelta = 1f / layer.Length;
			Vector2 mouseDelta = curPos - oriMousePos;
			oriMousePos = curPos;

			for (int i=0; i<layer.Length; i++) {
				Vector2 margin = GetLayerDeltaSize (i) / 2;
				Vector2 posDelta = mouseDelta * (scaleDelta * (i + 1));
				float lpx = targetPos [i].x + posDelta.x;
				float lpy = targetPos [i].y + posDelta.y;

				if (lpx > margin.x)
					lpx = margin.x; 
				if (lpx < -margin.x)
					lpx = -margin.x; 
				if (lpy > margin.y)
					lpy = margin.y; 
				if (lpy < -margin.y)
					lpy = -margin.y; 


				Vector2 newPos = new Vector2 (lpx, lpy);
				targetPos [i] = newPos;
			}
			MoveLayerToTarget ();
		}
	}

	void MoveLayerToTarget()
	{
		for (int i=0; i<layer.Length; i++) {
			layer[i].anchoredPosition = Vector2.Lerp(layer[i].anchoredPosition,targetPos[i],inertia);
		}
	}

	Vector2 GetLayerDeltaSize(int index)
	{
		int layerIndex = Mathf.Min (index, layer.Length-1);
		Vector2 delta = Vector2.zero;
		for (int i=0; i<=layerIndex; i++) {
			delta += sizeDelta[i];
		}
		return delta;
	}

	void OnPaused()
	{
		paused = true;
	}
	void OnResume()
	{
		paused = false;
	}
	void OnDestroy()
	{
        if(GlobalAudioSingleton.shared(GlobalAudioType.InGameMusic))
            GlobalAudioSingleton.shared(GlobalAudioType.InGameMusic).Stop();

		if(GUIMenu.shared () !=null)
		{
			GUIMenu.shared ().myuuPausedMenu.OnIntroStart -= OnPaused ;
			GUIMenu.shared ().myuuPausedMenu.OnResume -= OnResume ;
		}

	}
}
