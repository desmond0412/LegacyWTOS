using UnityEngine;
using System.Collections;

public class ShadeHole : MonoBehaviour {

	public GameObject[] Enable;
	public GameObject[] Disable;

	public GameObject ground;
	public ShadeHoleVFXHandler vfx;

	protected Coroutine triggerCoroutine;

	public bool state = true;


	void Start()
	{
		//Initiate the objects
		if (!state)
			vfx.SetVFXEnable (state);
		//if the shade hole is visible at the first time then all the enable item should be disable 
		if (Enable.Length > 0)			
		{
			foreach (GameObject item in Enable) {
				if(item!=null)
					item.SetActive(false);
			}
		}

		//if the shade hole is visible at the first time then all the disable item should be enable
		if (Disable.Length > 0)			
		{
			foreach (GameObject item in Disable) {
				if(item!=null)
					item.SetActive(true);
			}
		}




	}

	/// <summary>
	/// Triggers the shadhole.
	/// </summary>
	/// <param name="isTurnOn">If set to <c>true</c> is turn on.</param>
	/// <param name="delay">Delay.</param>
	public virtual void TriggerShadhole(bool isTurnOn,float delay = 0){
		state = isTurnOn;
		if(triggerCoroutine !=null) StopCoroutine(triggerCoroutine);
		triggerCoroutine = StartCoroutine (TriggerMainObject(isTurnOn,delay));
	}

	IEnumerator TriggerMainObject(bool isTurnOn,float delay)
	{
		yield return new WaitForSeconds(delay);

		if (Enable.Length > 0)			
		{
			foreach (GameObject item in Enable) {
				if(item!=null)
					item.SetActive(!isTurnOn);
			}
		}

		if (Disable.Length > 0)			
		{
			foreach (GameObject item in Disable) {
				if(item!=null)
					item.SetActive(isTurnOn);
			}
		}

		TriggerOtherObject(isTurnOn);
		triggerCoroutine = null;
	}


	protected virtual void TriggerOtherObject(bool isTurnOn)
	{
		if( ground!=null)
		{
			if(ground.GetComponent<ShaderAnimationHandler>() != null)
				ground.GetComponent<ShaderAnimationHandler>().TriggerActivation(isTurnOn);
			if(ground.GetComponent<SpriteRendererHandler>() != null)
				ground.GetComponent<SpriteRendererHandler>().TriggerColorActivation(isTurnOn);
		}
		vfx.SetVFXEnable(isTurnOn);
	}

//	void OnGUI ()
//	{
//		if (GUILayout.Button ("on")) {
//			TriggerShadhole (true);
//		}
//		if (GUILayout.Button ("off")) {
//			TriggerShadhole (false);
//		}
//	}
//



}

