using UnityEngine;
using System.Collections;

public class Atataraina : MonoBehaviour 
{
	public Animator atatarAnim;
	public Animator rainaAnim;
	public GameObject particleRainayam;

	public GameObject bloodSplats;
	public GameObject bloodBursts;
	public GameObject mainCam{ private get; set;}
	public bool isStillHauntingLev{ private get; set;}
	public bool FinishBlendToAtatar{ private get; set;}

	float delayTransitionBlendshapes = .2f;
	float fixAnimTransition = .2f;
	LevController Lev;
	L02DieAtatarTrigger dieTrig;

	float delayRainaBeforeFirstAttack = 3f;

	public float maxDistanceToLev = 10f;
	float distanceLevDie = 3.6f;
	float atatarMspd = .7f;
	float multiplier = 10f;

	string identifierShaderColorValue = "_ColorValue";

	public bool isLevDied{ get; private set;}

	//attack miss finished
	public void EndAttackMiss(){
		ToggleAtataraina (true);
		StartCoroutine (StartHauntingLev ());
	}

	IEnumerator Start(){
		isLevDied = false;
		isStillHauntingLev = true;
		FinishBlendToAtatar = false;
		dieTrig = this.GetComponent<L02DieAtatarTrigger> ();
		Lev = FindObjectOfType<LevController> ();
		while (Lev == null) {
			Lev = FindObjectOfType<LevController> ();
			yield return null;
		}
		StartCoroutine(AttackMiss ());
	}

	//cutscene 1st missed attack start from raina walk
	IEnumerator AttackMiss(){
		float waktu = delayRainaBeforeFirstAttack;
		while (waktu > 0f) {
			transform.Translate (new Vector3 (Time.deltaTime*atatarMspd,0f),Space.World);
			waktu -= Time.deltaTime;
			yield return null;
		}
		yield return StartCoroutine (ChangeToAtatar ());
		atatarAnim.SetTrigger("AttackMiss");

		waktu = .5f;
		while (waktu > 0f) {
			transform.Translate (new Vector3 (Time.deltaTime*atatarMspd*2.5f,0f),Space.World);
			waktu -= Time.deltaTime;
			yield return null;
		}
	}

	//start kill lev animation
	void KillLev(){
		isLevDied = true;
		atatarAnim.SetTrigger ("AttackKill");
		GetComponent<Rigidbody>().isKinematic = true;
		StartCoroutine (ChangeLevDistance ());
		StartCoroutine (BloodSplatAnimation ());
	}
	//for fixing atatar position at the top of Lev
	IEnumerator ChangeLevDistance(){
		float delayChange = .02f;
		float multiply = 5f;
		yield return new WaitForSeconds (0.3f);
		float tempDistance = distanceLevDie - 0.9f - Vector3.Distance (transform.position, Lev.transform.position);
		if(tempDistance > 0f) {
			tempDistance = distanceLevDie - 0.9f;
			while (Vector3.Distance (transform.position, Lev.transform.position) < tempDistance) {
				transform.Translate (new Vector3 (-delayChange *multiply, 0), Space.World);
				yield return new WaitForSeconds (delayChange);
			}
		} else if(tempDistance < 0f) {
			tempDistance = distanceLevDie - 0.9f;
			while (Vector3.Distance (transform.position, Lev.transform.position) > tempDistance) {
				transform.Translate (new Vector3 (delayChange *multiply, 0), Space.World);
				yield return new WaitForSeconds (delayChange);
			}
		}

	}
	IEnumerator BloodSplatAnimation(){
		yield return new WaitForSeconds (2f);

		AtatarAnimation temp = atatarAnim.GetComponent<AtatarAnimation> ();

		foreach(Transform child in bloodBursts.transform){
			child.gameObject.SetActive (true);
			child.transform.position = temp.atatarJawEnd.transform.position;
		}

		foreach(Transform child in bloodSplats.transform){
			child.gameObject.SetActive (true);
			child.transform.position = temp.atatarJawEnd.transform.position;
		}
	}

	void ToggleAtataraina(bool isRaina){
		rainaAnim.gameObject.SetActive (isRaina);
		atatarAnim.gameObject.SetActive (!isRaina);
	}

	//for speeding up raina
	public void SpeedUp(){
		StartCoroutine (SpeedUpToKillLev ());
	}
	IEnumerator SpeedUpToKillLev(){
		float tempAtatarMspd = atatarMspd;
		atatarMspd = 0f;
		yield return StartCoroutine(ChangeToAtatar());
		atatarMspd = tempAtatarMspd*multiplier;
		atatarAnim.SetTrigger ("Run");
	}

	IEnumerator StartHauntingLev(){
		while (true) {
			float distance = Vector3.Distance (transform.position, Lev.transform.position);
			if (distance < distanceLevDie && isStillHauntingLev){
				if (!atatarAnim.gameObject.activeInHierarchy) {
//					atatarMspd = 1f;
//					rainaAnim.speed = 1f;
					dieTrig.Trigger ();
					yield return StartCoroutine (ChangeToAtatar ());
				} else {
					dieTrig.animationDelay -= (fixAnimTransition + delayTransitionBlendshapes);
					dieTrig.Trigger ();
				}
				KillLev ();
				yield break;
			} else {
				if(distance > 1.0f)
					transform.Translate (new Vector3 ((distance > maxDistanceToLev) ? Time.deltaTime * atatarMspd + (distance - maxDistanceToLev) : Time.deltaTime * atatarMspd, 0, 0), Space.World);
			}
			yield return null;
		}
		rainaAnim.Stop ();
		Rigidbody temp = rainaAnim.GetComponent<Rigidbody> ();
		if (temp != null)
			temp.isKinematic = true;
	}

	//blendshape
	IEnumerator ChangeToAtatar(){
		AkSoundEngine.PostEvent ("raina_transform", rainaAnim.gameObject);

		GameObject effectRainayam = Instantiate(particleRainayam,transform.position + new Vector3(0.5f,0.5f,-1f),transform.rotation) as GameObject;
		ParticleSystem prtss = effectRainayam.GetComponent<ParticleSystem> ();

		float waktu = delayTransitionBlendshapes;
		SkinnedMeshRenderer[] skmr = rainaAnim.GetComponentsInChildren<SkinnedMeshRenderer> ();
		while (waktu > 0f) {
			transform.Translate (new Vector3 (Time.deltaTime*atatarMspd,0f),Space.World);
			foreach (SkinnedMeshRenderer sk in skmr) {
				sk.SetBlendShapeWeight (0, (1- (waktu / delayTransitionBlendshapes)) * 100f);
				sk.material.SetFloat (identifierShaderColorValue,1f - (waktu / delayTransitionBlendshapes));
			}
			waktu -= Time.deltaTime;
			yield return null;
		}

		SkinnedMeshRenderer atatarSkmr = atatarAnim.GetComponentInChildren<SkinnedMeshRenderer> ();
		atatarSkmr.material.color = Color.black;
		ToggleAtataraina (false);
		foreach (SkinnedMeshRenderer sk in skmr) {
			sk.SetBlendShapeWeight (0, 0f);
			sk.material.SetFloat (identifierShaderColorValue,0f);
		}
		
		waktu = 0f;
		while (waktu < fixAnimTransition) {
			transform.Translate (new Vector3 (Time.deltaTime*atatarMspd,0f),Space.World);
			atatarSkmr.material.SetFloat (identifierShaderColorValue,1f - (waktu / fixAnimTransition));
			waktu += Time.deltaTime;
			yield return null;
		}

		prtss.Stop (true);
		Destroy (effectRainayam,prtss.startLifetime);
	}
}
