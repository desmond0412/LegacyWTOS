using UnityEngine;
using System.Collections;

public class MoluvushaVFXHandler : MonoBehaviour {
    void Update(){
        transform.Rotate(Vector3.one * -4.5f * Time.deltaTime);
    }
}
