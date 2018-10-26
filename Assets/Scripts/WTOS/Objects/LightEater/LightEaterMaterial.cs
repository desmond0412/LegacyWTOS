using UnityEngine;
using System.Collections;

using Artoncode.Core.Utility;
using Artoncode.WTOS;

public class LightEaterMaterial : MonoBehaviour {
    Material ltMat;

    float glowIntensity = 0.0f;
    float eatingTime = 10.0f;

    void Awake(){
        glowIntensity = 0.0f;
    }

	void Start () {
        eatingTime = GetComponent<LightEaterMovementBezier>().timeToEat;

        GetComponentInChildren<Renderer>().sharedMaterial = GetComponentInChildren<Renderer>().material;
        ltMat = GetComponentInChildren<Renderer>().sharedMaterial;
        ltMat.SetFloat("_GlowIntensity",glowIntensity);
	}
	
    void FixedUpdate () {
        glowIntensity = GameUtility.changeTowards(glowIntensity,0.0f,1.0f/eatingTime,Time.fixedDeltaTime);
        ltMat.SetFloat("_GlowIntensity",glowIntensity);
	}

    public void SetGlowIntensity(float to){
        StartCoroutine(IntensityCoroutine(to,0.5f));
    }

    IEnumerator IntensityCoroutine(float to,float time){
        for(float t = 0.0f;t < time;t+=Time.fixedDeltaTime){
            glowIntensity = (t/time) * to;
            yield return null;
        }

        glowIntensity = to;
    }
}
