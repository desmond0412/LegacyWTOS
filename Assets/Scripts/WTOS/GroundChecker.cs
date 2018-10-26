using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Artoncode.WTOS {
	public interface IGroundCheckerDelegate {
		void groundCheckUpdate (bool isOnGround, Collider firstCollider);
	}

	public class GroundChecker : MonoBehaviour {

		public GameObject receiver;
		private IGroundCheckerDelegate d;
		private List <Collider> cols;

		void Start () {
			cols = new List<Collider> ();
			if (!receiver) {
				Debug.LogWarning ("No receiver found");
				enabled = false;
				return;
			}
			MonoBehaviour[] components = receiver.GetComponents<MonoBehaviour> ();
			foreach (MonoBehaviour component in components) {
				if (component is IGroundCheckerDelegate) {
					if (component.enabled) {
						d = component as IGroundCheckerDelegate;
					}
				}
			}
			if (d==null) {
				Debug.LogWarning ("Receiver should have one component which implementing IGroundCheckerDelegate");
				enabled = false;
				return;
			}
		}

		void FixedUpdate () {
			for (int i=cols.Count-1; i>=0; i--) {
				Collider c = cols[i];
				if (c == null || !c.gameObject.activeInHierarchy) {
					cols.Remove(c);
				}
			}
			d.groundCheckUpdate (cols.Count > 0, cols.Count > 0 ? cols[0] : null);
		}

		void OnTriggerEnter (Collider c) {
			if (!cols.Contains (c)) {
				if (!c.isTrigger) {
					cols.Add (c);
				}
			}
		}

		void OnTriggerExit (Collider c) {
			if (cols.Contains (c)) {
				cols.Remove (c);
			}
		}

		public Collider getFirstCollider () {
			if (cols.Count > 0) {
				return cols[0];
			}
			return null;
		}
	}

}