using UnityEngine;
using System.Collections;
using Artoncode.Core;

public class NaivisLightCameraEventHandler : MonoBehaviour {

	public Poca usablePocaPetal;
	public Naivis naivis;
	public SmoothFollowAreaTrigger cameraAreaTrigger;
	public CinemaDirector.Cutscene cutscene;

//	public int correctLightIndex = 2;

	private Vector3 targetScale;
	private Vector3 defaultScale = Vector3.zero;
	private Vector3 activatedScale = Vector3.one;

	private float holdTime = 0.0f;
	private bool isInLightTime = false;


	// Use this for initialization
	void Start () {
//		cameraAreaTrigger.gameObject.transform.localScale = defaultScale ;

		usablePocaPetal.pocaPetal.OnPocaUsedWhileTurnedOn += UsablePocaPetal_OnPocaUsedWhileTurnedOn;
		naivis.OnLightFinishedCasting += Naivis_OnLightFinishedCasting;
		naivis.OnLightTurnedOn += Naivis_OnLightTurnedOn;
		naivis.OnObjectUsed += Naivis_OnObjectUsed;

	}

	void Naivis_OnObjectUsed (GameObject sender)
	{
		if(sender.GetComponent<Naivis>().state == false)
			MainObjectSingleton.shared(MainObjectType.Player).GetComponent<LevController>().OnCustomAnimationFinished += OnCustomAnimationFinished;
	}

	void OnCustomAnimationFinished (GameObject sender)
	{
		MainObjectSingleton.shared(MainObjectType.Player).GetComponent<LevController>().setInputEnable(false);
		MainObjectSingleton.shared(MainObjectType.Player).GetComponent<LevController>().OnCustomAnimationFinished -= OnCustomAnimationFinished;
	}

	#region Poca Delegate
	void UsablePocaPetal_OnPocaUsedWhileTurnedOn ()
	{
		//		holdTime = 0.01f;
		//		targetScale = activatedScale;
		if (naivis.TURNON_MODE)	return;

		if (usablePocaPetal.pocaPetal.index != 0)
		{
//			naivis.isLocked = true;
//			naivis.isAccessible = false;
//			usablePocaPetal.isAccessible = false;
			cutscene.Play ();
		}
			
	}
	#endregion 

	#region Naivis Delegate
	void Naivis_OnLightTurnedOn ()
	{
		//		holdTime = 1.0f;
		//		targetScale = activatedScale;
		if (naivis.TURNON_MODE)	return;
		MainObjectSingleton.shared(MainObjectType.Player).GetComponent<LevController>().setInputEnable(true);

		if (usablePocaPetal.pocaPetal.index != 0)
		{
//			naivis.isLocked = true;
//			naivis.isAccessible  = false;
//			usablePocaPetal.isAccessible = false;
			cutscene.Play ();
		}
			
	}

	void Naivis_OnLightFinishedCasting ()
	{
		
//		holdTime = 4.0f;
//		targetScale = defaultScale;
	}
	#endregion 

	void Update()
	{
//		if(holdTime > 0.0f)
//		{
//			holdTime -= Time.deltaTime;
//			if(holdTime <=0.0f)
//			{
//				cameraAreaTrigger.gameObject.transform.localScale = targetScale;
//				holdTime = 0.0f;	
//			}
//		}
	}



	void OnDestroy()
	{
		usablePocaPetal.pocaPetal.OnPocaUsedWhileTurnedOn -= UsablePocaPetal_OnPocaUsedWhileTurnedOn;
		naivis.OnLightFinishedCasting -= Naivis_OnLightFinishedCasting;
		naivis.OnLightTurnedOn -= Naivis_OnLightTurnedOn;
		naivis.OnObjectUsed -= Naivis_OnObjectUsed;

	}
}
