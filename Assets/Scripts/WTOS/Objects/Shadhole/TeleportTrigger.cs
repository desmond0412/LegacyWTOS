using UnityEngine;
using System.Collections;

public class TeleportTrigger : MonoBehaviour {

	public GameObject Destination;
	public float delay = 1;

	virtual protected void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			MainObjectSingleton.shared(MainObjectType.Player).GetComponent<LevController>().StartCoroutine (Teleport (MainObjectSingleton.shared(MainObjectType.Player).gameObject));
		}
	}

	private IEnumerator Teleport(GameObject player)
	{
		player.GetComponent<Rigidbody>().isKinematic = true;
		player.GetComponent<LevController> ().stopMoving ();
		player.GetComponent<LevController> ().setInputEnable (false);
		yield return new WaitForSeconds(delay);
		player.transform.position = Destination.transform.position;
		yield return new WaitForSeconds(delay);
		player.GetComponent<Rigidbody>().isKinematic = false;
		yield return new WaitForSeconds(delay);
		player.GetComponent<LevController> ().setInputEnable (true);
	}

}
