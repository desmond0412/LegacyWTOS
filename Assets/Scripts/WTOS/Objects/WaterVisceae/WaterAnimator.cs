using UnityEngine;
using System.Collections;

public class WaterAnimator : MonoBehaviour {
    public float speedMultiplier = 0.1f;

    MeshRenderer rend;
    Material mat;

	void Start () {
        rend = GetComponent<MeshRenderer>();

        if(rend){
            mat = rend.material;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if(mat!=null){
            mat.mainTextureOffset += new Vector2(Time.deltaTime * speedMultiplier,0);
        }
	}
}
