using UnityEngine;
using System.Collections;

namespace Artoncode.Core.CameraPlatformer {
	public class CameraTargetGrabber : MonoBehaviour {

		public CameraTargetType type;
		private CameraPathNode node;
		private bool isFound;

		public enum CameraTargetType {
			SceneObject,
			Lev,
		}

		void Awake () {
			node = GetComponent<CameraPathNode> ();
		}

		IEnumerator Start () {
			if (type == CameraTargetType.SceneObject) yield break;

			do{
				switch (type) {
					case CameraTargetType.Lev:
						ActorSingleton lev = ActorSingleton.shared (ActorType.Lev);
						if (lev) {
							node.cameraSetting.cameraTarget = lev.transform;
							isFound = true;
						}
						break;
				}
				yield return new WaitForEndOfFrame();
			} while (!isFound);
			Destroy (this);
		}
	}
}