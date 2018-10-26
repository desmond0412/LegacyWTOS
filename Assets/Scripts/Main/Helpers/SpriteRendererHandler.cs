using UnityEngine;
using System.Collections;

public class SpriteRendererHandler : MonoBehaviour
{

	SpriteRenderer sRenderer;
	ColorAnimator cAnimator;

	Color originColor;
	Color clearColor = Color.clear;


	void Awake ()
	{
		sRenderer = this.GetComponent<SpriteRenderer> ();

		if(!(cAnimator = this.gameObject.GetComponent<ColorAnimator>()))
			cAnimator =	this.gameObject.AddComponent<ColorAnimator>();

		cAnimator.OnColorChanged += CAnimator_OnColorChanged;
		originColor = sRenderer.color;

	}

	void OnDestroy ()
	{
		if(cAnimator != null )
			cAnimator.OnColorChanged -= CAnimator_OnColorChanged;
	}


	public void TriggerColorActivation (bool isOn,float speed = -1)
	{
		Color targetColor = isOn ? originColor : clearColor;
		cAnimator.Trigger (sRenderer.color, targetColor,this.gameObject);
		if(speed != -1)
			cAnimator.fadingTime = speed;
	}

	void CAnimator_OnColorChanged (GameObject sender, float percentage, Color color)
	{
		sRenderer.color = color;
	}
	
	// Update is called once per frame


//	void OnGUI ()
//	{
//		if (GUILayout.Button ("on")) {
//			TriggerColorActivation (true);
//		}
//		if (GUILayout.Button ("off")) {
//			TriggerColorActivation (false);
//		}
//	}

}
