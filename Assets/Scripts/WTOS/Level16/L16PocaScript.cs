using UnityEngine;
using System.Collections;

public class L16PocaScript : ZRootInteractableObject {
    [SerializeField]
    Poca pocaScript;

    public override void UseObject()
    {
        base.UseObject();

        if(pocaScript){
            pocaScript.UseFunction();
        }
    }
}
