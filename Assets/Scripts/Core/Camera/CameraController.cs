using UnityEngine;
using System.Collections;

namespace Artoncode.Core.CameraPlatformer {
	public class CameraController : MonoBehaviour {

		public CameraSetting currentSetting;
		public Transform proxyObject;
		public Vector3 maxMotionNoise;
		public float motionNoiseSpeedScale;

		private Vector3 currentPosition;
		private Vector3 targetPosition;
		private Vector3 positionVelocity = Vector3.zero;

		private Vector3 currentRotation;
		private Vector3 rotationVelocity = Vector3.zero;

		private bool isShaking;
		private float shakeOffset;
		private float shakeVelocity;

		private bool isSettingLocked;

		void Start () {
			targetPosition = Vector3.zero;
			currentPosition = transform.position;
			currentRotation = transform.eulerAngles;
		}
		
		void Update () {
			updateSetting ();

			if(currentSetting.cameraTarget != null) {
				targetPosition = currentSetting.cameraTarget.position;
			}
				
			// Apply position transition
			if (currentSetting.scale <= 0)
				currentSetting.scale = 0.1f;
			float x = Mathf.SmoothDamp (currentPosition.x, targetPosition.x + currentSetting.positionOffset.x / currentSetting.scale, ref positionVelocity.x, currentSetting.smoothTime);
			float y = Mathf.SmoothDamp (currentPosition.y, targetPosition.y + currentSetting.positionOffset.y / currentSetting.scale, ref positionVelocity.y, currentSetting.smoothTime);
			float z = Mathf.SmoothDamp (currentPosition.z, targetPosition.z + currentSetting.positionOffset.z / currentSetting.scale, ref positionVelocity.z, currentSetting.smoothTime);

			x = Mathf.Min (Mathf.Max (x, currentSetting.min.x), currentSetting.max.x);
			y = Mathf.Min (Mathf.Max (y, currentSetting.min.y), currentSetting.max.y);
			z = Mathf.Min (Mathf.Max (z, currentSetting.min.z), currentSetting.max.z);

			transform.position = new Vector3 (x, y, z);

			currentPosition = transform.position;

			// Apply rotation transition
			float rx = Mathf.SmoothDamp (currentRotation.x, currentSetting.rotationOffset.x, ref rotationVelocity.x, currentSetting.smoothTime);
			float ry = Mathf.SmoothDamp (currentRotation.y, currentSetting.rotationOffset.y, ref rotationVelocity.y, currentSetting.smoothTime);
			float rz = Mathf.SmoothDamp (currentRotation.z, currentSetting.rotationOffset.z, ref rotationVelocity.z, currentSetting.smoothTime);

			transform.eulerAngles = new Vector3 (rx, ry, rz);

			currentRotation = transform.eulerAngles;

			// Add motion noise
			transform.position = currentPosition + new Vector3 (maxMotionNoise.x * Mathf.PerlinNoise (Time.time * motionNoiseSpeedScale, 0),
				maxMotionNoise.y * Mathf.PerlinNoise (Time.time * motionNoiseSpeedScale, 1),
				maxMotionNoise.z * Mathf.PerlinNoise (Time.time * motionNoiseSpeedScale, 2));


			if (isShaking) {
				shakeOffset -= shakeVelocity * Time.deltaTime;
				if (shakeOffset <= 0) {
					shakeOffset = 0;
					isShaking = false;
				}
				transform.position = transform.position + Random.onUnitSphere * shakeOffset;
			}
		}

		public void shake (float shakeOffset, float shakeDuration) {
			this.shakeOffset = shakeOffset;
			this.shakeVelocity = shakeOffset / shakeDuration;
			isShaking = true;
		}

		public void updateSetting () {
			if (isSettingLocked || !proxyObject) {
				return;
			}

			CameraSetting nearestcameraSetting = CameraPath.shared ().getNearestCameraSetting (proxyObject.position);
			if (nearestcameraSetting != null) {
				currentSetting = nearestcameraSetting;
			}
		}
	}
}