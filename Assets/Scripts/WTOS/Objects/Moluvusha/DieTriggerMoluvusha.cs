using UnityEngine;
using System.Collections;

public class DieTriggerMoluvusha : DieTrigger {
    public float dieIdleTime = 10.0f;
    public float currIdleTime = 0.0f;
    public float defensiveTime = 5.0f;
    public float currDefensiveTime = 0.0f;

    Animator anim;
    public string eatLevAnimation = "AM_Moluvusha_Eat_Lev";
    public string moluvushaDefensiveAnimation = "AM_Moluvusha_Idle_Defensive";

    bool isDead = false,calledOnce = false,isLevIn = false;

    int isLevInHash,defensiveHash;

    void Awake(){
        isLevInHash = Animator.StringToHash("IsLevIn");
        defensiveHash = Animator.StringToHash("DefensivePercentage");
    }

    public override void Start()
    {
        base.Start();
        causes = DieCausesType.Moluvusha;
        isDead = false;

        anim = GetComponent<Animator>();
    }

    void FixedUpdate(){
        calledOnce = true;
    }

    void LateUpdate(){
        if(calledOnce){
            anim.SetBool(isLevInHash,isLevIn);

            if(isLevIn){
                currDefensiveTime = 0.0f; 
            }else{
                currDefensiveTime += Time.deltaTime;
            }

            anim.SetFloat(defensiveHash,currDefensiveTime / defensiveTime);
        }

        calledOnce = false;
        isLevIn = false;
    }

    public override void Trigger(bool isCulpritIncluded = true)
    {
        isDead = true;

        base.Trigger(isCulpritIncluded);

        anim.CrossFade(eatLevAnimation,0.0f,0,0.0f);
    }
        
    protected override void CameraStartFocusOnLev()
    {
        base.CameraStartFocusOnLev();
    }

    public void SnapLevPosition(){
        Vector3 snapPosition = new Vector3(0.0f,2.62291f,0.69556f);

        MainObjectSingleton.shared(MainObjectType.Player).transform.position = transform.position - snapPosition;
		MainObjectSingleton.shared(MainObjectType.Player).GetComponent<LevController>().FacingDirectionInstant = LevController.Direction.Right;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player")){
            currIdleTime = 0.0f;
        }
    }

    void OnTriggerStay(Collider col){
        if(col.tag.Equals("Player")){
            if(anim.GetCurrentAnimatorStateInfo(0).shortNameHash == Animator.StringToHash(moluvushaDefensiveAnimation)){
                currIdleTime += Time.fixedDeltaTime;
            }

            isLevIn = true;

            camPosOffset = new Vector3(transform.position.x - col.gameObject.transform.position.x,
                0.0f,
                -13.0f);
            camLookAtOffset = new Vector3(transform.position.x - col.gameObject.transform.position.x,
                2.0f,
                0.0f);

            if(currIdleTime > dieIdleTime && !isDead){
                Trigger(true);
            }
        }
    }

    void OnTriggerExit(Collider col){
        if(col.tag.Equals("Player")){
            currIdleTime = 0.0f;
        }
    }
}
