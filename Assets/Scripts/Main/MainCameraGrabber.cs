using UnityEngine;
using System.Collections;
using CinemaDirector;
using ScionEngine;

public class MainCameraGrabber : MonoBehaviour {

	private ActorTrackGroup actorTrack;
	private CameraSmoothFollowEvent CSFEvent;
//	private CutsceneTrigger cutSceneTrigger;
	private ScionPostProcessBase scion;
	private CameraFocusEvent cameraFocus;

	void Awake()
	{
		actorTrack 		= this.GetComponent<ActorTrackGroup>();
//		cutSceneTrigger = this.GetComponent<CutsceneTrigger>();
		CSFEvent		= this.GetComponent<CameraSmoothFollowEvent>();
		cameraFocus		= this.GetComponent<CameraFocusEvent>();
		scion			= this.GetComponent<ScionPostProcessBase>();
	}

	IEnumerator Start()
	{
		GameObject theActor = null;
		int numOfTry = 30;
		int counter = 0;
		do{
			yield return new WaitForEndOfFrame();

			if( MainObjectSingleton.shared(MainObjectType.Camera) != null )
			{
				theActor = MainObjectSingleton.shared(MainObjectType.Camera).gameObject;

				if(cameraFocus !=null)
					cameraFocus.target = theActor.transform;

				if(scion !=null)
					scion.depthOfFieldTargetTransform = theActor.transform;

				if(actorTrack !=null)
					actorTrack.Actor = theActor.transform;
				
				if(CSFEvent !=null)
					CSFEvent.target  = theActor.transform;
			}

		}while (theActor == null && counter++ < numOfTry);
	}


}
