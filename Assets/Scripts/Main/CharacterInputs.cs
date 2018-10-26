using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class CharacterInputs : MonoBehaviour {

	CharacterController cont;

	public bool xAxis = true;
	public bool zAxis = true;


	// Use this for initialization
	float speed;

	void Start () {
		cont = this.GetComponent<CharacterController>();
		speed = 500;
	}
	
	// Update is called once per frame
	void Update () {
		float h = Input.GetAxis("Horizontal") * Time.deltaTime * speed  * (xAxis ? 1 : 0);
		float v = Input.GetAxis("Vertical") * Time.deltaTime * speed * (zAxis ? 1 : 0);

		cont.SimpleMove(new Vector3(h,0,v));
	}

	void OnGUI()
	{
	}
}
