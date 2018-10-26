using UnityEngine;
using System.Collections;
using CinemaDirector;
using ScionEngine;
using Artoncode.Core.CameraPlatformer;

public class MainActorGrabber : MonoBehaviour {

	public ActorType actor;

	private ActorTrackGroup actorTrack;
	private CameraSmoothFollowEvent CSFEvent;
//	private CutsceneTrigger cutSceneTrigger;
	private ScionPostProcessBase scion;
	private CameraFocusEvent cameraFocus;
	private CameraController cameraController;

	void Awake()
	{
		actorTrack 		= this.GetComponent<ActorTrackGroup>();
//		cutSceneTrigger = this.GetComponent<CutsceneTrigger>();
		CSFEvent		= this.GetComponent<CameraSmoothFollowEvent>();
		cameraFocus		= this.GetComponent<CameraFocusEvent>();
		scion			= this.GetComponent<ScionPostProcessBase>();
		cameraController = this.GetComponent<CameraController> ();
	}

	IEnumerator Start()
	{
		GameObject theActor = null;
		int numOfTry = 30;
		int counter = 0;
		do{
			yield return new WaitForEndOfFrame();

			if( ActorSingleton.shared(actor) != null )
			{
				theActor = ActorSingleton.shared(actor).gameObject;

				if(cameraFocus !=null)
					cameraFocus.target = theActor.transform;

				if(scion !=null)
					scion.depthOfFieldTargetTransform = theActor.transform;

				if(actorTrack !=null)
					actorTrack.Actor = theActor.transform;
				
				if(CSFEvent !=null)
					CSFEvent.target  = theActor.transform;

				if(cameraController !=null)
					cameraController.proxyObject = theActor.transform;
			}

		}while (theActor == null && counter++ < numOfTry);
	}


}
