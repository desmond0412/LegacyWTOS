using UnityEngine;
using System.Collections;

public class AntiShadeHole : MonoBehaviour {

	private GameObject shadeHoleObject;
	private bool isStay = false;

    void OnTriggerStay(Collider col){
		ShadeHole shadeHole = col.GetComponent<ShadeHole>();

		if(shadeHole !=null ){
			shadeHoleObject = col.gameObject;
			if(isStay && shadeHole.state)
				isStay = false;

			if(!isStay)
			{
				isStay = true;
				shadeHole.TriggerShadhole(false,0.0f);	
			}
        }
    }

    void OnTriggerExit(Collider col){
		ShadeHole shadeHole = col.GetComponent<ShadeHole>();
		if(shadeHole !=null ){
			shadeHoleObject = col.gameObject;
			shadeHole.TriggerShadhole(true,0.0f);
        }
 
	}

	void OnDestroy()
	{
		if(shadeHoleObject != null)
		{
			ShadeHole shadeHole = shadeHoleObject.GetComponent<ShadeHole>();
			if(shadeHole !=null ){
				shadeHole.TriggerShadhole(true,0.0f);
			}
		}
			
	}
}
