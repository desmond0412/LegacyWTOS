using UnityEngine;
using System.Collections;

public class L16NaivisScript : ZRootInteractableObject {
    [SerializeField]
    Naivis naivisScript;

    void Awake(){

    }


    public override void UseObject()
    {
        base.UseObject();

        if(naivisScript){
            naivisScript.UseFunction();
        }
    }
}
