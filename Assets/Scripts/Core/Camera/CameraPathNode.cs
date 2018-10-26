using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Artoncode.Core.CameraPlatformer {
	public class CameraPathNode : MonoBehaviour {

		public enum CameraPathNodeType {
			Point,
			Volume,
		};

		public CameraPathNodeType type;
		public CameraSetting cameraSetting;

		public Vector3 boxSize;
		public int priority;

		void Start () {
			if (cameraSetting.cameraTargetType != CameraTargetGrabber.CameraTargetType.SceneObject) {
				CameraTargetGrabber targetGrabber = gameObject.AddComponent<CameraTargetGrabber> ();
				targetGrabber.type = cameraSetting.cameraTargetType;
			}
		}

		public bool isInside (Vector3 point) {
			point = transform.InverseTransformPoint (point);

			float halfX = (boxSize.x * 0.5f);
			float halfY = (boxSize.y * 0.5f);
			float halfZ = (boxSize.z * 0.5f);
			if (point.x < halfX && point.x > -halfX && 
				point.y < halfY && point.y > -halfY && 
				point.z < halfZ && point.z > -halfZ) {
				return true;
			}
			else {
				return false;
			}
		}
	}
}