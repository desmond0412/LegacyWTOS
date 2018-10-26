using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Artoncode.Core.Utility;
using Artoncode.WTOS;
using RootMotion.FinalIK;

public class LightEaterAnimator : MonoBehaviour {
    [System.Serializable]
    public enum LightEaterState{
        peek,
        chasing,
        eating
    }

    [SerializeField]
    Transform targetHead,skeletonObject;

    [SerializeField]
    List<Transform> boneList;

    [SerializeField]
    float accelerationSpeed = 5.0f;

    Material ltMat;

    [SerializeField]
    private Transform _closestMoluvusha;

    private LightEaterState _currentState;
    public LightEaterState currentState{
        get{
            return _currentState;
        }
        set{
            _currentState = value;

            if(value == LightEaterState.peek){
                _closestMoluvusha = null;
            }
        }
    }

    public float startingHeight = 2.0f;
    public float peekHeightPercentage = 0.2f;
    public float normalHeightPercentage = 1.0f;
    public float eatingTime = 10.0f;

    float startingDistance,eatTimeFlag,boneStartingLength,glowIntensity;
    Vector3 fixedEatingPosition;

    void Awake(){
        boneStartingLength = boneList[1].position.x - boneList[0].position.x;
        glowIntensity = 0.0f;
        fixedEatingPosition = Vector3.zero;
        startingDistance = (skeletonObject.position - targetHead.position).magnitude;
        targetHead.localPosition = new Vector3(targetHead.localPosition.x,
            startingHeight,
            targetHead.localPosition.z);
    }

    void Start(){
        _currentState = LightEaterState.peek;
        GetComponentInChildren<Renderer>().sharedMaterial = GetComponentInChildren<Renderer>().material;
        ltMat = GetComponentInChildren<Renderer>().sharedMaterial;
        ltMat.SetFloat("_GlowIntensity",glowIntensity);
    }

    void FixedUpdate(){
        switch(_currentState){
            case LightEaterState.peek:
                targetHead.localPosition = new Vector3(GameUtility.changeTowards(targetHead.localPosition.x,0.0f,Mathf.Abs(accelerationSpeed),Time.fixedDeltaTime),
                    GameUtility.changeTowards(targetHead.localPosition.y,peekHeightPercentage * startingDistance,Mathf.Abs(accelerationSpeed),Time.fixedDeltaTime),
                    GameUtility.changeTowards(targetHead.localPosition.z,0.0f,Mathf.Abs(accelerationSpeed),Time.fixedDeltaTime));

                break;
            case LightEaterState.chasing:
                Vector3 dirNormal = (targetHead.position - _closestMoluvusha.position).normalized;

                targetHead.position = new Vector3(GameUtility.changeTowards(targetHead.position.x,_closestMoluvusha.position.x,Mathf.Abs(accelerationSpeed * dirNormal.x),Time.fixedDeltaTime),
                    GameUtility.changeTowards(targetHead.position.y,_closestMoluvusha.position.y,Mathf.Abs(accelerationSpeed * dirNormal.y),Time.fixedDeltaTime),
                    GameUtility.changeTowards(targetHead.position.z,_closestMoluvusha.position.z,Mathf.Abs(accelerationSpeed * dirNormal.z),Time.fixedDeltaTime));

                if((_closestMoluvusha.position - targetHead.position).magnitude < 1.0f){
                    HandleEatingState(_closestMoluvusha);
                }

                break;
            case LightEaterState.eating:
                Vector3 dirNormal2 = (targetHead.position - fixedEatingPosition).normalized;

                if(Time.fixedTime > eatTimeFlag){
                    _currentState = LightEaterState.peek;
                }else{
                    targetHead.position = new Vector3(GameUtility.changeTowards(targetHead.position.x,fixedEatingPosition.x,Mathf.Abs(accelerationSpeed * dirNormal2.x),Time.fixedDeltaTime),
                        GameUtility.changeTowards(targetHead.position.y,fixedEatingPosition.y,Mathf.Abs(accelerationSpeed * dirNormal2.y),Time.fixedDeltaTime),
                        GameUtility.changeTowards(targetHead.position.z,fixedEatingPosition.z,Mathf.Abs(accelerationSpeed * dirNormal2.z),Time.fixedDeltaTime));
                }
                break;
            default:
                break;
        }

//        ResimulateBonePosition((targetHead.position - skeletonObject.position).magnitude);
        skeletonObject.localScale = new Vector3(skeletonObject.localScale.x, Mathf.Min(1.0f,(skeletonObject.position - targetHead.position).magnitude / startingDistance), skeletonObject.localScale.z);
//        ResimulateBoneScale((targetHead.position - skeletonObject.position).magnitude);

        glowIntensity = GameUtility.changeTowards(glowIntensity,0.0f,1.0f/eatingTime,Time.fixedDeltaTime);
        ltMat.SetFloat("_GlowIntensity",glowIntensity);
    }
        
    public void SetLightEaterMoluvusha(Transform closestMoluvusha){
        if(currentState != LightEaterState.eating){
            _closestMoluvusha = closestMoluvusha;

            currentState = _closestMoluvusha ?  LightEaterState.chasing : LightEaterState.peek;
        }
    }

    void HandleEatingState(Transform molPosition){
        _currentState = LightEaterState.eating;
        molPosition.GetComponent<Moluvusha>().EatMoluvusha(eatingTime);

        fixedEatingPosition = molPosition.position;

        eatTimeFlag = Time.fixedTime + eatingTime;
        StartCoroutine(IntensityCoroutine(1.0f,0.1f));
    }

    void ResimulateBonePosition(float totalLength){
        float lengthLeft = totalLength;
        for(int boneInd = boneList.Count-1;boneInd>=0;boneInd--){
            if(lengthLeft > 0.0f){
                boneList[boneInd].localPosition = new Vector3(-1.0f * Mathf.Min(Mathf.Abs(boneStartingLength),lengthLeft),
                    boneList[boneInd].localPosition.y,
                    boneList[boneInd].localPosition.z);
                lengthLeft -= Mathf.Min(Mathf.Abs(boneStartingLength),lengthLeft);
            }else{
                boneList[boneInd].localPosition = new Vector3(0.0f,
                    boneList[boneInd].localPosition.y,
                    boneList[boneInd].localPosition.z);
            }
        }
    }

    void ResimulateBoneScale(float totalLength){
        float lengthLeft = totalLength;
        for(int boneInd = boneList.Count-1;boneInd>=0;boneInd--){
            if(lengthLeft > Mathf.Abs(boneStartingLength)){
                lengthLeft -= Mathf.Abs(boneStartingLength);
                boneList[boneInd].localScale = Vector3.one;
            }else if(lengthLeft > 0.0f){
                boneList[boneInd].localScale = new Vector3(1.0f * (lengthLeft/Mathf.Abs(boneStartingLength)) / skeletonObject.localScale.y,
                    boneList[boneInd].localScale.y,
                    boneList[boneInd].localScale.z);

                boneList[boneInd+1].localScale = new Vector3(1.0f / skeletonObject.localScale.y / boneList[boneInd].localScale.x,
                    boneList[boneInd+1].localScale.y,
                    boneList[boneInd+1].localScale.z);

                lengthLeft -= Mathf.Abs(boneStartingLength);
            }else{
                boneList[boneInd].localScale = Vector3.one;
            }
        }

        if(lengthLeft>0.0f){
            boneList[0].localScale = new Vector3(1.0f/skeletonObject.localScale.y,1.0f,1.0f);
        }
    }

    IEnumerator IntensityCoroutine(float to,float time){
        for(float t = 0.0f;t < time;t+=Time.fixedDeltaTime){
            glowIntensity = (t/time) * to;
            yield return null;
        }

        glowIntensity = to;
    }
}
