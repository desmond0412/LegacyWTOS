using UnityEngine;
using System.Collections;

public class L16DebrisScript : MonoBehaviour {
    ParticleSystem debrisParticle;
    ParticleSystem.EmissionModule partEmission;

    [SerializeField]
    GameObject shadholePlace;
	DieTrigger dieTrigger;

    bool isEnable;
	bool isTriggered;

//	ParticleCollisionEvent[] collisionEvents;

    void Awake(){
		debrisParticle =this.GetComponent<ParticleSystem>();
		dieTrigger = this.GetComponent<DieTrigger>();
		isTriggered = false;
//		collisionEvents = new ParticleCollisionEvent[16];
    }
        
    void Start(){
        if(shadholePlace && debrisParticle){
            isEnable = true;
            partEmission = debrisParticle.emission;
            partEmission.enabled = true;
        }
    }

	void Update () {
        if(isEnable && !shadholePlace.activeInHierarchy){
            isEnable = false;
            partEmission.enabled = false;
        }else if(!isEnable && shadholePlace.activeInHierarchy){
            isEnable = true;
            partEmission.enabled = true;
        }
	}

    void OnParticleCollision(GameObject other) {

		if(isTriggered) return;

		if(other.CompareTag("Player"))
		{

//			int safeLength = debrisParticle.GetSafeCollisionEventSize();
//			if(collisionEvents.Length < safeLength)
//				collisionEvents = new ParticleCollisionEvent[safeLength];
//
//			int numCollisionEvents = debrisParticle.GetCollisionEvents(other,collisionEvents);
//			print("in : " + numCollisionEvents);
//			return;


			isTriggered = true;
			if(!MainObjectSingleton.shared(MainObjectType.Player).GetComponent<LevController>().isGrounded)
			{
				MainObjectSingleton.shared(MainObjectType.Player).GetComponent<LevController>().setInputEnable(false);
				MainObjectSingleton.shared(MainObjectType.Player).GetComponent<LevController>().OnLanding += OnLanding;
			}
			else
			{
				dieTrigger.Trigger(false);		
			}
				
		}
    }

    void OnLanding ()
    {
		MainObjectSingleton.shared(MainObjectType.Player).GetComponent<LevController>().OnLanding -= OnLanding;
        MainObjectSingleton.shared(MainObjectType.Player).GetComponent<Rigidbody>().isKinematic = true;
        dieTrigger.Trigger(false);      
    }

}
