using UnityEngine;
using System.Collections;

using Artoncode.Core.Utility;
using Artoncode.WTOS;
using RootMotion.FinalIK;


public class LightEaterMovement : MonoBehaviour {
    public Transform targetTransform,targetHeadIK;

    Animator anim;
    public AnimationClip movementAnimation;
    public AnimationClip retractAnimation;

    int appearHashTrigger;
    int retractHashTrigger;
    int eatingHashTrigger;

    [SerializeField]
    GameObject targetMoluvusha = null;

    CCDIK ikScript;
    LightEaterMaterial leMatScript;

    [System.Serializable]
    public enum LightEaterState{
        hide = 0,
        appear = 1,
        chasing = 2,
        eating = 3,
        retract = 4
    }

    private LightEaterState _currentState;
    public LightEaterState currentState{
        get{
            return _currentState;
        }
        set{
            _currentState = value;
        }
    }

    bool firstChaseFlag,firstRetractFlag,firstEatingFlag;
    float movementTimeFlag,retractTimeFlag,eatingTimeFlag;

    float currPosWeight = 0.0f,currEndBonesWeight = 0.0f,currMidBonesWeight = 0.0f;
    float maxPosWeight = 0.25f,maxEndBonesWeight = 1.0f,maxMidBonesWeight = 0.25f;

    public float eatingTime = 10.0f;

    void Awake(){
        appearHashTrigger = Animator.StringToHash("AppearTrigger");
        retractHashTrigger = Animator.StringToHash("RetractTrigger");
        eatingHashTrigger = Animator.StringToHash("EatingTrigger");
    }

    void Start(){
        anim = GetComponent<Animator>();
        ikScript = GetComponentInChildren<CCDIK>();
        leMatScript = GetComponent<LightEaterMaterial>();
    }

    void Update(){
        if(_currentState == LightEaterState.hide || _currentState == LightEaterState.chasing){
            targetTransform.position = targetMoluvusha ? targetMoluvusha.transform.position : targetTransform.position;
        }

        SimulateLightEater();
    }

    void SimulateLightEater(){
        if(_currentState == LightEaterState.retract){
            if(firstRetractFlag){
                firstRetractFlag = false;
                retractTimeFlag = 0.0f;
                currPosWeight = ikScript.solver.IKPositionWeight;
                currEndBonesWeight = ikScript.solver.bones[11].weight;
                currMidBonesWeight = ikScript.solver.bones[1].weight;
            }

            ikScript.solver.IKPositionWeight = Mathf.Lerp(currPosWeight,0.0f,retractTimeFlag/retractAnimation.length);
            for(int i=0;i<ikScript.solver.bones.Length;i++){
                if(i == ikScript.solver.bones.Length - 1){
                    ikScript.solver.bones[i].weight = Mathf.Lerp(currEndBonesWeight,0.0f,retractTimeFlag/retractAnimation.length);;
                }else{
                    ikScript.solver.bones[i].weight = Mathf.Lerp(currMidBonesWeight,0.0f,retractTimeFlag/retractAnimation.length);;
                }
            }

            retractTimeFlag += Time.fixedDeltaTime;
        }else if(!targetMoluvusha){
            if(!(_currentState == LightEaterState.hide || _currentState == LightEaterState.retract || _currentState == LightEaterState.eating)){
                anim.SetTrigger(retractHashTrigger);
            }else if(_currentState == LightEaterState.eating){
                if(firstEatingFlag){
                    firstEatingFlag = false;
                    eatingTimeFlag = 0.0f;

                    if(leMatScript){
                        leMatScript.SetGlowIntensity(1.0f);
                    }
                }

                if(eatingTimeFlag < eatingTime){
                    ikScript.solver.IKPositionWeight = GameUtility.changeTowards(ikScript.solver.IKPositionWeight,1.0f,0.03f,Time.fixedDeltaTime);
                    for(int i=0;i<ikScript.solver.bones.Length;i++){
                        if(i == ikScript.solver.bones.Length - 1){
                            ikScript.solver.bones[i].weight = GameUtility.changeTowards(ikScript.solver.bones[i].weight,maxEndBonesWeight,0.03f,Time.fixedDeltaTime);
                        }else{
                            ikScript.solver.bones[i].weight = GameUtility.changeTowards(ikScript.solver.bones[i].weight,maxMidBonesWeight,0.03f,Time.fixedDeltaTime);
                        }
                    }

                    eatingTimeFlag += Time.fixedDeltaTime;
                }else{
                    anim.SetTrigger(retractHashTrigger);
                }
            }
        }else{
            firstRetractFlag = true;

            if(_currentState == LightEaterState.hide){
                anim.SetTrigger(appearHashTrigger);
            }else if(_currentState == LightEaterState.appear){
                firstChaseFlag = true;
                firstEatingFlag = true;
            }else if(_currentState == LightEaterState.chasing){
                if(firstChaseFlag){
                    firstChaseFlag = false;
                    movementTimeFlag = 0.0f;
                }

                ikScript.solver.IKPositionWeight = Mathf.Lerp(0.0f,maxPosWeight,movementTimeFlag/movementAnimation.length);
                for(int i=0;i<ikScript.solver.bones.Length;i++){
                    if(i==0 || i == ikScript.solver.bones.Length - 1){
                        ikScript.solver.bones[i].weight = Mathf.Lerp(0.0f,maxEndBonesWeight,movementTimeFlag/movementAnimation.length);
                    }else{
                        ikScript.solver.bones[i].weight = Mathf.Lerp(0.0f,maxMidBonesWeight,movementTimeFlag/movementAnimation.length);
                    }
                }

                movementTimeFlag += Time.fixedDeltaTime;

                if((targetHeadIK.position - targetMoluvusha.transform.position).magnitude < 0.75f){
                    HandleEatingState(targetMoluvusha.transform);
                }
            }else if(_currentState == LightEaterState.eating){
                if(firstEatingFlag){
                    firstEatingFlag = false;
                    eatingTimeFlag = 0.0f;

                    if(leMatScript){
                        leMatScript.SetGlowIntensity(1.0f);
                    }
                }

                if(eatingTimeFlag < eatingTime){
                    ikScript.solver.IKPositionWeight = GameUtility.changeTowards(ikScript.solver.IKPositionWeight,1.0f,0.03f,Time.fixedDeltaTime);
                    for(int i=0;i<ikScript.solver.bones.Length;i++){
                        if(i == ikScript.solver.bones.Length - 1){
                            ikScript.solver.bones[i].weight = GameUtility.changeTowards(ikScript.solver.bones[i].weight,maxEndBonesWeight,0.03f,Time.fixedDeltaTime);
                        }else{
                            ikScript.solver.bones[i].weight = GameUtility.changeTowards(ikScript.solver.bones[i].weight,maxMidBonesWeight,0.03f,Time.fixedDeltaTime);
                        }
                    }

                    eatingTimeFlag += Time.fixedDeltaTime;
                }else{
                    anim.SetTrigger(retractHashTrigger);
                }
            }
        }
    }

    public void SetLightEaterMoluvusha(GameObject g){
        if(_currentState != LightEaterState.eating){
            targetMoluvusha = g;
        }
    }

    void HandleEatingState(Transform molPosition){
        _currentState = LightEaterState.eating;
        molPosition.GetComponent<Moluvusha>().EatMoluvusha(eatingTime);

        anim.SetTrigger(eatingHashTrigger);
    }
}
