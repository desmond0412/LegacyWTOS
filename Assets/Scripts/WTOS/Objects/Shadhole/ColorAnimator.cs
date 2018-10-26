using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ColorAnimator : MonoBehaviour
{
	public delegate void ColorAnimatorDelegate(GameObject sender, float percentage, Color color);
	public event ColorAnimatorDelegate OnColorChanged;
	public event ColorAnimatorDelegate OnColorChangedCompleted;

	public float fadingTime = 1.0f;

	private float startTime;
	private Dictionary<GameObject,Coroutine> coroutineList;

	void Awake()
	{
		coroutineList = new Dictionary<GameObject, Coroutine>();
	}

	public void Trigger(Color sourceColor, Color targetColor, GameObject sender )
	{
		startTime = Time.time;

		StartCoroutine(UpdateAnimation(sender, sourceColor, targetColor));

//		if(!coroutineList.ContainsKey(key))
//		{
//		Coroutine co = StartCoroutine(UpdateAnimation(sender));
//		coroutineList.Add(sender,co);
//		}
//		print(coroutineList.Count);
	}

	private IEnumerator UpdateAnimation(GameObject sender,Color sourceColor,Color targetColor)
	{
		bool isFading = true;

		while(isFading)
		{
			float percentage = Mathf.Clamp01((Time.time - startTime)/fadingTime);
			Color currentColor = Color.Lerp(sourceColor,targetColor,percentage);
			if(OnColorChanged!=null)
				OnColorChanged(sender,percentage,currentColor );

			if(currentColor == targetColor)
			{
				isFading = false;
//				coroutineList[key] = null;
//				coroutineList.Remove(key);

				if(OnColorChangedCompleted!=null)
					OnColorChangedCompleted(sender, percentage, currentColor );

			}
			yield return new WaitForEndOfFrame();
		}
	}
}

