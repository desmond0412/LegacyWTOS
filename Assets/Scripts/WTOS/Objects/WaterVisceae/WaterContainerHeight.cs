using UnityEngine;
using System.Collections;

public class WaterContainerHeight : MonoBehaviour {
    
    public Transform startingHeight,endHeight;

	public delegate void WaterContainerHeightDelegate (GameObject sender);
	public event WaterContainerHeightDelegate OnFillWaterUpdate;
	public event WaterContainerHeightDelegate OnFillWaterEnd;
    public event WaterContainerHeightDelegate OnAbsorbWaterUpdate;
    public event WaterContainerHeightDelegate OnAbsorbWaterEnd;

    void Start(){
        transform.position = startingHeight.position;
    }

    public void FillWater(float timeToFill){
        StartCoroutine(FillWaterCoroutine(timeToFill));
    }

    public void AbsorbWater(float timeToAbsorb){
        StartCoroutine(AbsorbWaterCoroutine(timeToAbsorb));
    }

    public IEnumerator FillWaterCoroutine(float timeToFill){
        for(float time = 0.0f;time < timeToFill;time+=Time.deltaTime){
            transform.position = new Vector3(transform.position.x,
                startingHeight.position.y + Mathf.Abs(endHeight.position.y - startingHeight.position.y) * (time / timeToFill),
                transform.position.z);

			if(OnFillWaterUpdate!=null)
				OnFillWaterUpdate(this.gameObject);
			
            yield return null;
        }

		if(OnFillWaterEnd !=null)
			OnFillWaterEnd(this.gameObject);
		
        transform.position = endHeight.position;
    }

    public IEnumerator AbsorbWaterCoroutine(float timeToAbsorb){
        for(float time = 0.0f;time < timeToAbsorb;time+=Time.deltaTime){
            transform.position = new Vector3(transform.position.x,
                startingHeight.position.y + Mathf.Abs(endHeight.position.y - startingHeight.position.y) * (1.0f - (time / timeToAbsorb)),
                transform.position.z);

            if(OnAbsorbWaterUpdate!=null)
                OnAbsorbWaterUpdate(this.gameObject);

            yield return null;
        }

        if(OnAbsorbWaterEnd !=null)
            OnAbsorbWaterEnd(this.gameObject);

        transform.position = startingHeight.position;
    }
}
