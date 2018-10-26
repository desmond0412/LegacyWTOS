using CinemaDirector;
using CinemaDirector.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/*
    To create a custom timeline item, you must create a class that derives from one of:
        
        CinemaGlobalAction  -   An item that spans a period of time (an Action), and affects the global scene.
        CinemaGlobalEvent   -   An item that triggers at a specific point in time (an Event), and affects the global scene.
        CinemaActorAction   -   An item that spans a period of time (an Action), and affects 1 or more actors in the scene.
        CinemaActorEvent    -   An item that triggers at a specific point in time (an Event), and affects 1 or more actors in the scene.

    Your class will also need a CutsceneItemAttribute where you can set what category and label your item can be found under when adding
    an item to your timeline, as well as 1 or more genres that dictates which types of timeline tracks will allow the use of your item.

    Lastly, if your cutscene item manipulates any data in your scene, you will want your class to implement the IRevertable interface,
    which will help restore your scene to its original state whenever you enter or exit play mode. The required method implementations will
    be covered below.

    This simple example cutscene item will shrink an actor until it is gone over the duration of the item at a constant rate. (The same
    effect can be achieved using a curve track)
*/

[CutsceneItem("Lev", "Disable Input Event", CutsceneItemGenre.ActorItem)]
class LevDisableInputEvent : CinemaActorEvent
{
	private LevController levController;

	public override void Trigger(GameObject Actor)
	{
		if(Actor != null)
		{
			levController = Actor.GetComponent<LevController>();
			if(levController == null )
			{
				Debug.LogError("Actor is does not have Lev Controller");
				return;					
			}

			levController.setInputEnable(false);
		}
	}


}

