using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Artoncode.WTOS;
using Artoncode.Core;
using Artoncode.Core.CameraPlatformer;

public class ZRoot : MonoBehaviour {

	public enum ZRootClawType {
		Handle = 1,
		Flat,
		Knuckle
	}

	private LevController levController;
	private LevGauntletController gauntletController;

	public GameObject zRootObject;
    public Transform zRootClaw;
	public List<Transform> trunkList;

    public float zRootMaximumDistance = 7.5f;
	public float modelLength = 7.5f;
    public LayerMask characterLayerMask;

	public float angularSpeed = 720;
	public float zRootSpeed = 15;
	private Vector3 attachPoint;
	private float currentAngle;
	private float startingBoneDistance;
	private float shakeTime;
	private float shakeMultiplier;
	private Vector2 shakeOffset;

	public GameObject gauntletVFXPrefab;
	public Transform leafVFX;
	public bool useBoxVFX;
	public Transform boxVFX;
	public bool useEmitterVFX;
	public Transform emitterVFX;

	void Start () {
		levController = GetComponentInParent<LevController> ();
		gauntletController = levController.levGauntletController;

        zRootObject.SetActive(false);
        startingBoneDistance = (trunkList[1].localPosition - trunkList[0].localPosition).magnitude;
	}

	void Update () {
		if (isShot ()) {
			attachZRoot ();
		}
	}

	public bool isShot () {
		return zRootObject.activeInHierarchy;
	}

	public void shootZRoot (Vector3 originPosition, Vector3 direction) {
		for(int i = trunkList.Count-1; i>=0; i--){
			trunkList[i].localPosition = Vector3.zero;
			trunkList[i].localScale = Vector3.zero;
		}
		zRootClaw.localPosition = Vector3.zero;
		StartCoroutine (shootZRootCoroutine (originPosition, direction));
	}

	IEnumerator spawnGauntletVFX () {
		GameObject vfx = Instantiate (gauntletVFXPrefab);
		vfx.transform.SetParent (gauntletController.gauntletNozzle, false);
		yield return new WaitForSeconds (2f);
		Destroy (vfx);
	}

	private void playVFX () {
		if (useBoxVFX) {
			ParticleSystem[] particles = boxVFX.GetComponentsInChildren<ParticleSystem> ();
			foreach (ParticleSystem particle in particles) {
				particle.Play ();
			}
		}
		if (useEmitterVFX) {
			ParticleSystem[] particles = emitterVFX.GetComponentsInChildren<ParticleSystem> ();
			foreach (ParticleSystem particle in particles) {
				particle.Play ();
			}
		}

		ParticleSystem[] leaves = leafVFX.GetComponentsInChildren<ParticleSystem> ();
		foreach (ParticleSystem leaf in leaves) {
			leaf.Play ();
		}
	}

	private void updateVFX () {
		if (useBoxVFX) {
			boxVFX.localScale = new Vector3 (1, 1, zRootClaw.localPosition.magnitude);
		}
		if (useEmitterVFX) {
			emitterVFX.position = zRootClaw.position;
		}
	}

	private void stopVFX () {
		if (useBoxVFX) {
			ParticleSystem[] particles = boxVFX.GetComponentsInChildren<ParticleSystem> ();
			foreach (ParticleSystem particle in particles) {
				particle.Stop ();
			}
		}
		if (useEmitterVFX) {
			ParticleSystem[] particles = emitterVFX.GetComponentsInChildren<ParticleSystem> ();
			foreach (ParticleSystem particle in particles) {
				particle.Stop ();
			}
		}
	}

	IEnumerator shootZRootCoroutine (Vector3 originPosition, Vector3 direction) {
		zRootObject.gameObject.SetActive(true);

		float distance = 0;
		currentAngle = 0;
		bool isHit = false;
		ZRootInteractableObject interactable = null;
		Vector3 hitPos = originPosition + direction * zRootMaximumDistance;
		shakeOffset = new Vector2 (Random.Range (0f, 10f), Random.Range (0f, 10f));

		StartCoroutine (spawnGauntletVFX ());

		// Find object to attach
		RaycastHit[] hits;
		hits = Physics.RaycastAll (originPosition, direction, zRootMaximumDistance, ~characterLayerMask ^ Physics.IgnoreRaycastLayer);
		float closestDist = zRootMaximumDistance;
		Vector3 hitNormal = -direction;
		foreach(RaycastHit hit in hits){
			ZRootInteractableObject t = hit.collider.transform.GetComponent<ZRootInteractableObject>();
			float dist = float.MaxValue;
			if (t) {
				if (t.zRootGrabHandle) {
					dist = (t.zRootGrabHandle.position - originPosition).magnitude - t.zRootGrabOffset;
				}
				else {
					dist = (hit.point - originPosition).magnitude - t.zRootGrabOffset;
				}
			}
			else if(!hit.collider.isTrigger){
				dist = (hit.point - originPosition).magnitude;
			}
			if (dist < closestDist) {
				closestDist = dist;
				isHit = true;
				if (t) {
					interactable = t;
					if (t.zRootGrabHandle) {
						hitPos = t.zRootGrabHandle.position - direction * t.zRootGrabOffset;
					}
					else {
						hitPos = hit.point - direction * t.zRootGrabOffset;
						hitNormal = hit.normal;
					}
				}
				else {
					interactable = null;
					hitPos = hit.point;
				}
			}
		}

		// Animate ZRoot to reach object
		AkSoundEngine.PostEvent ("lev_zroot_shoot", gameObject);
		playVFX ();
		direction = (hitPos - originPosition).normalized;
		float timeToReach = closestDist / zRootSpeed;
		float currentTime = 0;
		int zRootShape = 0;
		if (interactable) {
			zRootShape = (int)interactable.zRootClawType;
		}
		zRootObject.GetComponent<Animator> ().SetInteger ("shape", zRootShape);
		zRootObject.GetComponent<Animator> ().SetTrigger ("shot");
		while (currentTime < timeToReach) {
			currentTime += Time.deltaTime;
			if (currentTime > timeToReach) {
				currentTime = timeToReach;
			}

			distance = zRootSpeed * currentTime;
			attachPoint = originPosition + direction * distance;
			currentAngle += angularSpeed * Time.deltaTime;

			updateVFX ();

			yield return null;
		}
		AkSoundEngine.PostEvent ("lev_zroot_stop", gameObject);
		stopVFX ();

		// Notify the object when it's hit
		AkSoundEngine.PostEvent ("lev_zroot_impact", gameObject);
		zRootObject.GetComponent<Animator> ().SetTrigger ("onImpact");
		if (isHit) {
			if(interactable){
				if (interactable.zRootClawType != ZRootClawType.Knuckle) {
					yield return new WaitForSeconds (0.05f);
					shakeTime = 0.35f;
					shakeMultiplier = 1;
					yield return new WaitForSeconds (0.35f);
					shakeTime = 0;
				}
				interactable.HitByZRoot(this,levController.GetComponent<Rigidbody> ());
				if (interactable.zRootClawType == ZRootClawType.Knuckle) {
					float t = 0;
					MainObjectSingleton.shared (MainObjectType.Camera).GetComponent<CameraController> ().shake (0.1f, 0.3f);
					while (currentTime + t < timeToReach + 0.2f) {
						t += Time.deltaTime;
						if (t > 0.2f) {
							t = 0.2f;
						}

						distance = zRootSpeed * currentTime + zRootSpeed / 10 * t;
						attachPoint = originPosition + direction * distance;
						currentAngle += angularSpeed * Time.deltaTime;

						yield return null;
					}
				}
				if (interactable.interactDefinition == ZRootInteractableObject.InteractType.Grapple) {
					yield return new WaitForSeconds (0.1f);
				}
			}
		}
		yield return null;

		// Animate ZRoot back to Lev's gauntlet
		AkSoundEngine.PostEvent ("lev_zroot_retract", gameObject);
		zRootObject.GetComponent<Animator> ().ResetTrigger ("onImpact");
		zRootObject.GetComponent<Animator> ().SetTrigger ("retracted");
		direction = attachPoint - transform.position;
		distance = direction.magnitude;
		direction.Normalize ();
		while (distance > 0.1f) {
			distance -= zRootSpeed * Time.deltaTime;
			if (distance < 0.1f) {
				distance = 0.1f;
			}
			attachPoint = transform.position + direction * distance;
			currentAngle -= angularSpeed * Time.deltaTime;

			yield return null;
		}
		zRootObject.GetComponent<Animator> ().ResetTrigger ("retracted");
			
		zRootObject.gameObject.SetActive(false);
	}

	private void attachZRoot () {
		transform.position = gauntletController.gauntletNozzle.position;
		Vector3 dir = attachPoint - transform.position;
		float dist = dir.magnitude;
		dir.Normalize ();
		transform.right = dir;

		zRootClaw.position = attachPoint;

		transform.Rotate (currentAngle, 0.0f, 0.0f, Space.Self);

		bool distributeLengthEvenly = dist >= modelLength;
		float interval = startingBoneDistance;
		float maxScale = 1;
		if (distributeLengthEvenly) {
			interval = dist / (trunkList.Count - 1);
			maxScale = modelLength / dist;
		}
		for(int i = trunkList.Count-1; i>=0; i--){
			if((dist > 0.0f || distributeLengthEvenly) && i != 0){
				trunkList[i].position = transform.position + dir * dist;
				if (shakeTime > 0 && i != trunkList.Count - 1 && dist > startingBoneDistance) {
					float yOffset = (Mathf.PerlinNoise (i + shakeOffset.x, shakeOffset.y) - 0.5f) * shakeTime * shakeMultiplier;
					float zOffset = (Mathf.PerlinNoise (i + shakeOffset.y, shakeOffset.x) - 0.5f) * shakeTime * shakeMultiplier;
					trunkList[i].localPosition += new Vector3 (0, yOffset, zOffset);
					shakeTime -= Time.deltaTime;
				}
				trunkList[i].localScale = Vector3.one * Mathf.Min (Mathf.Min (dist, interval) / interval + 0.3f, maxScale);

				dist -= interval;
			}else{
				trunkList[i].position = transform.position;
				trunkList[i].localScale = Vector3.zero;
			}
		}
	}

    public void GrappleForce(Vector3 direction,float forcePower){
		Vector2 dir = new Vector2 (0, direction.y);
		direction.y = 0;
		dir.x = direction.magnitude;
		gauntletController.jumpByZRoot (dir, forcePower);
    }

}
   
