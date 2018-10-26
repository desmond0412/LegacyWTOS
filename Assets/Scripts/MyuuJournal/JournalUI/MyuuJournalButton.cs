using UnityEngine;
using System.Collections;

public class MyuuJournalButton : MonoBehaviour {
	public delegate void MyuuJournalButtonDelegate ();
	public event MyuuJournalButtonDelegate OnClick;

	public bool buttonEnabled = true;
	public bool hideFromStart = true;
	Material mat;
	void Start()
	{
		MeshRenderer mr = GetComponent<MeshRenderer> ();
		if (mr!=null)
			mat = mr.material;
		if ((mat!=null)&&(hideFromStart)) {
			mat.SetFloat("_DissolveIntensity",0f);
			buttonEnabled = false;
		}
	}

	void OnMouseUp()
	{
		if (buttonEnabled) {
			if (OnClick != null)
				OnClick ();
		}
	}

	public void StartIntro()
	{
		if ((mat != null) && (hideFromStart)) {
			iTween.ValueTo (gameObject, iTween.Hash ("from",0f,
			                                         "to",1f,
			                                         "time",1f,
			                                         "onupdate","UpdateCutoff",
			                                         "oncomplete","OnCutoffComplete",
			                                         "ignoretimescale",(Time.timeScale==0f),
			                                         "easetype","linear"));
		}
	}
	void UpdateCutoff(float val)
	{
		mat.SetFloat("_DissolveIntensity",val);
	}
	void OnCutoffComplete() 
	{
		buttonEnabled = true;
	}

	public void StartOutro()
	{
		buttonEnabled = false;
		if ((mat != null) && (hideFromStart)) {
			iTween.ValueTo (gameObject, iTween.Hash ("from",1f,
			                                         "to",0f,
			                                         "time",0.5f,
			                                         "onupdate","UpdateCutoff",
			                                         "ignoretimescale",(Time.timeScale==0f),
			                                         "easetype","linear"));
		}
	}
}
