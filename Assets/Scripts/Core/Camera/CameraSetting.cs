using UnityEngine;
using System;
using System.Collections;

namespace Artoncode.Core.CameraPlatformer {
	[Serializable]
	public class CameraSetting {

		public CameraTargetGrabber.CameraTargetType cameraTargetType;
		public Transform cameraTarget;

		public float smoothTime = 0.3f;
		public float scale = 1f;
		public Vector3 positionOffset = Vector3.zero;
		public Vector3 rotationOffset = Vector3.zero;
		public Vector3 min = Vector3.one * float.NegativeInfinity;
		public Vector3 max = Vector3.one * float.PositiveInfinity;

		public CameraSetting () {
			cameraTarget = null;

			smoothTime = 0.3f;
			scale = 1f;
			positionOffset = Vector3.zero;
			rotationOffset = Vector3.zero;
			min = Vector3.one * float.NegativeInfinity;
			max = Vector3.one * float.PositiveInfinity;
		}

		public CameraSetting (CameraSetting setting) {
			cameraTarget = setting.cameraTarget;

			smoothTime = setting.smoothTime;
			scale = setting.scale;
			positionOffset = setting.positionOffset;
			rotationOffset = setting.rotationOffset;
			min = setting.min;
			max = setting.max;
		}

		public static CameraSetting Lerp (CameraSetting a, CameraSetting b, float t) {
			CameraSetting newCameraSetting = new CameraSetting ();

			if (t < 0.5f) {
				newCameraSetting.cameraTarget = a.cameraTarget;
			}
			else {
				newCameraSetting.cameraTarget = b.cameraTarget;
			}

			newCameraSetting.smoothTime = Mathf.Lerp (a.smoothTime, b.smoothTime, t);
			newCameraSetting.scale = Mathf.Lerp (a.scale, b.scale, t);
			newCameraSetting.positionOffset = Vector3.Lerp (a.positionOffset, b.positionOffset, t);
			newCameraSetting.rotationOffset = Vector3.Lerp (a.rotationOffset, b.rotationOffset, t);

			if (!float.IsNegativeInfinity (a.min.x) && !float.IsNegativeInfinity (b.min.x)) newCameraSetting.min.x = Mathf.Lerp (a.min.x, b.min.x, t);
			else if (t < 0.5f) newCameraSetting.min.x = a.min.x;
			else newCameraSetting.min.x = b.min.x;

			if (!float.IsNegativeInfinity (a.min.y) && !float.IsNegativeInfinity (b.min.y)) newCameraSetting.min.y = Mathf.Lerp (a.min.y, b.min.y, t);
			else if (t < 0.5f) newCameraSetting.min.y = a.min.y;
			else newCameraSetting.min.y = b.min.y;

			if (!float.IsNegativeInfinity (a.min.z) && !float.IsNegativeInfinity (b.min.z)) newCameraSetting.min.z = Mathf.Lerp (a.min.z, b.min.z, t);
			else if (t < 0.5f) newCameraSetting.min.z = a.min.z;
			else newCameraSetting.min.z = b.min.z;

			if (!float.IsPositiveInfinity (a.max.x) && !float.IsPositiveInfinity (b.max.x)) newCameraSetting.max.x = Mathf.Lerp (a.max.x, b.max.x, t);
			else if (t < 0.5f) newCameraSetting.max.x = a.max.x;
			else newCameraSetting.max.x = b.max.x;

			if (!float.IsPositiveInfinity (a.max.y) && !float.IsPositiveInfinity (b.max.y)) newCameraSetting.max.y = Mathf.Lerp (a.max.y, b.max.y, t);
			else if (t < 0.5f) newCameraSetting.max.y = a.max.y;
			else newCameraSetting.max.y = b.max.y;

			if (!float.IsPositiveInfinity (a.max.z) && !float.IsPositiveInfinity (b.max.z)) newCameraSetting.max.z = Mathf.Lerp (a.max.z, b.max.z, t);
			else if (t < 0.5f) newCameraSetting.max.z = a.max.z;
			else newCameraSetting.max.z = b.max.z;

			return newCameraSetting;
		}
	}
}