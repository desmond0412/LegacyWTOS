using UnityEngine;
using System.Collections;

namespace Artoncode.Core.CameraPlatformer {
	public class CameraPathEdge : MonoBehaviour {
		public CameraPathNode n1;
		public CameraPathNode n2;
		public AnimationCurve weight;

		public bool project (Vector3 p, out float ratio, out float distance) {
			ratio = float.PositiveInfinity;
			distance = float.PositiveInfinity;
			if (n1 == null || n2 == null) {
				return false;
			}

			Vector3 d = p - n1.transform.position;
			Vector3 s = n2.transform.position - n1.transform.position;
			Vector3 sN = s.normalized;
			float sM = s.magnitude;

			float dsScalar = Vector3.Dot (d, sN);
			if (dsScalar < 0 || dsScalar > sM) {
				return false;
			}

			ratio = dsScalar / sM;
			Vector3 ds = dsScalar * sN;
			distance = (d - ds).magnitude;
			return true;
		}
	}
}