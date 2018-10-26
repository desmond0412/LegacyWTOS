using UnityEngine;
using System.Collections;

public class RotateTransformHandler : MonoBehaviour {

	public Vector3 Rotation = new Vector3(360,0);
	public float duration = 1f;

	float waktu = 0f;

	void Start(){
		
	}

	void Update(){
		transform.Rotate (Rotation * (Time.deltaTime / duration));
	}
}
