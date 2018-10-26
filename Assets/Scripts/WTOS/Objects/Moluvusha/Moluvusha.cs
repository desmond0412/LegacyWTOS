using UnityEngine;
using System.Collections;

public class Moluvusha : MonoBehaviour{
    public float moluvushaRadius = 1.0f;
    public float moluvushaBirthTime = 10.0f;
    public float moluvushaLifeTime = 10.0f;

    public bool isAttached = true,isBeingEaten = false;
    MoluvushaHolder molHolderScript;
    MoluvushaZRootInteractable molZRootScript;

    public Collider pickableCollider;
    public SphereCollider eatableCollider;
    public SphereCollider zrootCollider;

    [SerializeField]
    Transform trashPool;
    [SerializeField]
    Transform[] billboardGO;
    [SerializeField]
    AnimationClip birthAnimation,dyingAnimation,molHolderOpenAnimation;

    FixedJoint fJoint;
    int birthSpeedFloat,dyingSpeedFloat;

    void Awake(){
        birthSpeedFloat = Animator.StringToHash("BirthSpeed");
        dyingSpeedFloat = Animator.StringToHash("DyingSpeed");
    }

    void Start(){
        molZRootScript = GetComponentInChildren<MoluvushaZRootInteractable>();

        if(GetComponentInParent<MoluvushaHolder>()){
            GetComponent<Rigidbody>().isKinematic = true;
            isAttached = true;
            molHolderScript = GetComponentInParent<MoluvushaHolder>();
        }else{
            isAttached = false;
        }

        molZRootScript.isEnable = false;
    }

    public void StartMoluvusha(){
        GetComponent<Animator>().SetFloat(birthSpeedFloat,(birthAnimation.length/moluvushaBirthTime));
        GetComponent<Animator>().SetFloat(dyingSpeedFloat,(dyingAnimation.length/moluvushaLifeTime));

        pickableCollider.enabled = false;
    }

    void Update(){
        if(eatableCollider.radius != moluvushaRadius){
            eatableCollider.radius = moluvushaRadius;
        }

        foreach(Transform tr in billboardGO){
			if(Camera.main !=null)
            	tr.LookAt(Camera.main.transform);
        }
    }

    public void EatMoluvusha(float timeEaten){
        isBeingEaten = true;
        molZRootScript.isEnable = false;
        pickableCollider.enabled = false;

        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Animator>().Stop();

        iTween.ScaleTo(this.gameObject,iTween.Hash("scale",Vector3.zero,
            "easetype",iTween.EaseType.easeInQuad,
            "time",timeEaten));

        Destroy(this.gameObject,timeEaten);
    }

    public void ConnectBody(Rigidbody rb){
        fJoint.connectedBody = rb;
    }

    public void AnimatorMoluvushaEnable(){
        eatableCollider.enabled = true;

        if(molHolderScript){
            molHolderScript.OpenMoluvusha();
        }

        Invoke("EnablePick",molHolderOpenAnimation.length);
    }

    public void AnimatorMoluvushaDisable(){
        molZRootScript.isEnable = false;

        eatableCollider.enabled = false;
    }

    public void AnimatorMoluvushaDied(){
        Destroy(this.gameObject);
    }

    public void EnablePick(){
        if(pickableCollider){
            pickableCollider.enabled = true;
        }
       
        molZRootScript.isEnable = true;
    }

    public void PullMoluvusha(){
        molZRootScript.PullForce(Vector3.zero,0.0f);
    }
}
