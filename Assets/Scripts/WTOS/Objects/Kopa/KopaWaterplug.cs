using UnityEngine;
using System.Collections;

public class KopaWaterplug : ZRootInteractableObject {
    public override void UseObject()
    {
        base.UseObject();
		if(isEnable)
        	GetComponentInParent<KopaScript>().SetPlug(false);
    }
}
