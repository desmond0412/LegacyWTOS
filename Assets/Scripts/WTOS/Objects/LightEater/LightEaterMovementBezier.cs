using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Artoncode.Core.Utility;

public class LightEaterMovementBezier : MonoBehaviour {
    public Transform homePoint;
    public bool isEnableChasing = true;

    public BezierPoint startPoint;
    public BezierPoint targetPoint;

    [Range(0.01f,0.5f)]
    public float horizontalGap;
    public float angleHandle = 45.0f;

    [Range(0.0f,1.0f)]
    public float timeInterpolate;

    BezierCurve bezierScript;
    public float startingBoneDistance;
    public float accelerationSpeed = 0.25f;
    public float timeToEat = 10.0f;
    public GameObject diedVFX;
    public GameObject outVFX;

    float eatFlag = 0.0f;
    LightEaterMaterial ltMat;

    Animator anim;
    int eatTrigger,moveTrigger,hideTrigger,retractTrigger,moluvushaSize;

    [SerializeField]
    List<Transform> IKBones;

    [SerializeField]
    GameObject targetMoluvusha = null;

    public GameObject lightEaterHome;

    public enum LightEaterState{
        hide,
        chasing,
        eating,
        retract
    }

    [SerializeField]
    private LightEaterState _currentState = LightEaterState.hide;
    public LightEaterState currentState{
        get{
            return _currentState;
        }
        set{
            _currentState = value;
        }
    }

    void Awake(){
        startingBoneDistance = (IKBones[2].position - IKBones[1].position).magnitude;
        eatTrigger = Animator.StringToHash("EatingTrigger");
        moveTrigger = Animator.StringToHash("MovingTrigger");
        hideTrigger = Animator.StringToHash("HideTrigger");
        retractTrigger = Animator.StringToHash("RetractTrigger");
        moluvushaSize = Animator.StringToHash("MoluvushaSize");
    }

	void Start () {
        bezierScript = GetComponent<BezierCurve>();
        ltMat = GetComponent<LightEaterMaterial>();
        anim = GetComponent<Animator>();
        startPoint.position = transform.position;
//
//        if(homePoint){
//            transform.position = homePoint.position - (Vector3.up * 2.0f);
//            startPoint.position = homePoint.position;
//        }
	}
	
	void LateUpdate () {
        switch(currentState){
            case LightEaterState.hide:
                HandleHide();
                break;
            case LightEaterState.chasing:
                HandleChasing();
                break;
            case LightEaterState.eating:
                HandleEating();
                break;
            case LightEaterState.retract:
                HandleRetract();
                break;
            default:
                HandleHide();
                break;
        }

        SimulateLightEater();
	}

    void HandleHide(){
        if(targetMoluvusha && isEnableChasing){
            currentState = LightEaterState.chasing;

            targetPoint.transform.position = targetMoluvusha.transform.position;

            outVFX.SetActive(true);

            if(anim){
                anim.SetTrigger(moveTrigger);
            }
        }else{
            timeInterpolate = GameUtility.changeTowards(timeInterpolate,0.0f,accelerationSpeed,Time.fixedDeltaTime);
        }
    }

    void HandleChasing(){
        if(targetMoluvusha){
//            Vector3 reverseTanNorm = -1.0f * bezierScript.GetTangentAt(0.05f).normalized;
//            Vector3 reverseTan = new Vector3(-2.0f * reverseTanNorm.x / reverseTanNorm.y,-2.0f,0.0f);
//            transform.position = startPoint.transform.position + reverseTan;

            targetPoint.transform.position = targetMoluvusha.transform.position;

            timeInterpolate = GameUtility.changeTowards(timeInterpolate,1.0f,accelerationSpeed,Time.fixedDeltaTime);
            if(timeInterpolate >= 0.95f){
                eatFlag = Time.time;
                currentState = LightEaterState.eating;

                targetMoluvusha.GetComponent<Moluvusha>().EatMoluvusha(timeToEat);

                if(ltMat){
                    ltMat.SetGlowIntensity(1.0f);
                }
                if(anim){
                    anim.SetTrigger(eatTrigger);
                }
            }
        }else{
            currentState = LightEaterState.retract;
            if(anim){
                anim.SetTrigger(retractTrigger);
            }
        }
    }

    void HandleEating(){
        if(Time.time > eatFlag + timeToEat){
            currentState = LightEaterState.retract;
            if(anim){
                anim.SetTrigger(retractTrigger);
            }
        }else{
            if(targetMoluvusha){
                targetPoint.transform.position = targetMoluvusha.transform.position;
                anim.SetFloat(moluvushaSize,Mathf.Clamp01(targetMoluvusha.transform.lossyScale.x));
            }
        }
    }

    void HandleRetract(){
        timeInterpolate = GameUtility.changeTowards(timeInterpolate,0.0f,accelerationSpeed,Time.fixedDeltaTime);
        if(timeInterpolate < 0.05f){
            outVFX.SetActive(false);
            currentState = LightEaterState.hide;
            if(anim){
                anim.SetTrigger(hideTrigger);
            }
        }
    }

    void SimulateLightEater(){
        Vector3 handle1Position = startPoint.transform.position + 
            Quaternion.AngleAxis((targetPoint.transform.position.x > startPoint.transform.position.x ? angleHandle : 180.0f - angleHandle),Vector3.forward) * (targetPoint.transform.position - startPoint.transform.position).normalized * (targetPoint.transform.position - startPoint.transform.position).magnitude * horizontalGap;
        Vector3 handle2Position = targetPoint.transform.position + 
            Quaternion.AngleAxis((targetPoint.transform.position.x > startPoint.transform.position.x ? 180.0f - angleHandle : angleHandle),Vector3.forward) * (startPoint.transform.position - targetPoint.transform.position).normalized * (startPoint.transform.position - targetPoint.transform.position).magnitude * horizontalGap;

        if(targetPoint.transform.position.x < startPoint.transform.position.x){
            startPoint.globalHandle1 = handle1Position;
            targetPoint.globalHandle1 = handle2Position;
        }else{
            startPoint.globalHandle2 = handle1Position;
            targetPoint.globalHandle2 = handle2Position;
        }

        Vector3[] positionList = new Vector3[IKBones.Count];
        Vector3[] tangentList = new Vector3[IKBones.Count];
        Vector3[] scaleList = new Vector3[IKBones.Count];
//        float homeLength = (homePoint.position - transform.position).magnitude;
//        float lengthLeft = (bezierScript.length + homeLength) * timeInterpolate;
//
//        for(int i = IKBones.Count-1 ; i >= 0 ; i--){
//            if(lengthLeft > startingBoneDistance + homeLength){
//                positionList[i] = bezierScript.GetLengthToPoint(lengthLeft - homeLength);
//                tangentList[i] = bezierScript.GetLengthToTangent(lengthLeft - homeLength);
//
//                lengthLeft -= startingBoneDistance;
//            }else if(lengthLeft > homeLength){
//                positionList[i] = bezierScript.GetLengthToPoint(lengthLeft - homeLength);
//                tangentList[i] = bezierScript.GetLengthToTangent(lengthLeft - homeLength);
//
//                lengthLeft -= startingBoneDistance;
//            }else if(lengthLeft > 0.0f){
//                positionList[i] = Vector3.Lerp(transform.position,homePoint.position,(lengthLeft / homeLength));
//                if(i < IKBones.Count - 1)
//                {
//                    tangentList[i] = Vector3.Lerp((homePoint.position - transform.position).normalized,tangentList[i+1],(lengthLeft / homeLength));
//                }else{
//                    tangentList[i] = (homePoint.position - transform.position).normalized;
//                }
//
//                lengthLeft -= startingBoneDistance;
//            }else{
//                positionList[i] = transform.position;
//                tangentList[i] = (homePoint.position - transform.position).normalized;
//            }
//        }

        float lengthLeft = (bezierScript.length) * timeInterpolate;

        for(int i = IKBones.Count-1 ; i >= 0 ; i--){
            if(lengthLeft > 0.0f){
                positionList[i] = bezierScript.GetLengthToPoint(lengthLeft);
                tangentList[i] = bezierScript.GetLengthToTangent(lengthLeft);
                scaleList[i] = Vector3.one * Mathf.Clamp01(lengthLeft/startingBoneDistance);

                lengthLeft -= startingBoneDistance;
            }else{
                positionList[i] = transform.position;
                tangentList[i] = bezierScript.GetTangentAt(0.03f);
                scaleList[i] = new Vector3(0.001f,0.001f,0.001f);
            }
        }

        for(int i=0;i<IKBones.Count;i++){
            IKBones[i].position = positionList[i];
            IKBones[i].transform.right = (-1.0f * tangentList[i]);
            IKBones[i].transform.RotateAround(IKBones[i].transform.position,IKBones[i].transform.right,90.0f);
            IKBones[i].localScale = scaleList[i];
        }
    }

    public void SetLightEaterMoluvusha(GameObject g){
        if(_currentState != LightEaterState.eating){
            targetMoluvusha = g;
        }
    }
}
