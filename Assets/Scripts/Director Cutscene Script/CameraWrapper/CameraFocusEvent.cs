using CinemaDirector;
using CinemaDirector.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ScionEngine;

[CutsceneItem("Camera", "Focus Event", CutsceneItemGenre.ActorItem)]
class CameraFocusEvent : CinemaActorEvent,IRevertable
{
    public Transform target;

	// Options for reverting in editor.
	[SerializeField]
	private RevertMode editorRevertMode = RevertMode.Revert;

	// Options for reverting during runtime.
	[SerializeField]
	private RevertMode runtimeRevertMode = RevertMode.Revert;
	/// <summary>
	/// Option for choosing when this Event will Revert to initial state in Editor.
	/// </summary>
	public RevertMode EditorRevertMode
	{
		get { return editorRevertMode; }
		set { editorRevertMode = value; }
	}

	/// <summary>
	/// Option for choosing when this Event will Revert to initial state in Runtime.
	/// </summary>
	public RevertMode RuntimeRevertMode
	{
		get { return runtimeRevertMode; }
		set { runtimeRevertMode = value; }
	}


	/// <summary>
	/// Cache the state of all actors related to this event.
	/// </summary>
	/// <returns></returns>
	public RevertInfo[] CacheState()
	{
		List<Transform> actors = new List<Transform>(GetActors());
		List<RevertInfo> reverts = new List<RevertInfo>();
		foreach (Transform go in actors)
		{
			if (go != null)
			{
				reverts.Add(new RevertInfo(this, go.gameObject, "SetActive", go.gameObject.activeSelf));
			}
		}

		return reverts.ToArray();
	}

    public override void Trigger(GameObject Actor)
    {
        if(Actor != null)
        {
            if(target){
                Actor.GetComponent<ScionPostProcessBase>().depthOfFieldTargetTransform = target;
            }else{
                Actor.GetComponent<ScionPostProcessBase>().depthOfFieldTargetTransform = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }
    }
}

