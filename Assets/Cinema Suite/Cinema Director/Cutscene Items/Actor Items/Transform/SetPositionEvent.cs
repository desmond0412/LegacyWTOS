using UnityEngine;

namespace CinemaDirector
{
    /// <summary>
    /// Detaches all children in hierarchy from this Parent.
    /// </summary>
    [CutsceneItemAttribute("Transform", "Set Position", CutsceneItemGenre.ActorItem)]
    public class SetPositionEvent : CinemaActorEvent
    {
        public Transform Position;

        public override void Trigger(GameObject actor)
        {
            if (actor != null)
            {
                actor.transform.position = Position.position;
            }
        }

	}
}