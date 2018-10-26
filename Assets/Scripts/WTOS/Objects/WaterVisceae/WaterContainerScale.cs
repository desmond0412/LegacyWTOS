using UnityEngine;
using System.Collections;

public class WaterContainerScale : MonoBehaviour {
    [SerializeField]
    Transform anchorPoint;

    void Start(){
        anchorPoint.localScale = new Vector3(anchorPoint.localScale.x,
                                            0.0f,
                                            anchorPoint.localScale.z);
    }

    public void FillWater(float timeToFill,float waterHeight){
        StartCoroutine(FillWaterCoroutine(timeToFill,waterHeight));
    }

    public IEnumerator FillWaterCoroutine(float timeToFill,float waterHeight){
        for(float time = 0.0f;time < timeToFill;time+=Time.deltaTime){
            anchorPoint.localScale = new Vector3(anchorPoint.localScale.x,
                                                waterHeight * (time / timeToFill),
                anchorPoint.localScale.z);

            yield return null;
        }
        anchorPoint.localScale = new Vector3(anchorPoint.localScale.x,
                                            waterHeight,
            anchorPoint.localScale.z);
    }
}
