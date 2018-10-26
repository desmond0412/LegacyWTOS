using UnityEngine;
using System.Collections;

public class ParticleBoundChanger : MonoBehaviour {

	public Vector3 center = Vector3.zero;
	public Vector3 size = Vector3.one;

	void Start () {
		Renderer[] renderers = GetComponentsInChildren<Renderer> ();
		foreach (Renderer r in renderers) {
			if (r is ParticleSystemRenderer) {
				ParticleSystemRenderer pr = ((ParticleSystemRenderer)r);
				if (pr.mesh) {
					pr.mesh.bounds = new Bounds (center, size);
				}
			}
		}
	}

	void OnDrawGizmosSelected () {
		Gizmos.color = Color.cyan;
		Gizmos.matrix = Matrix4x4.TRS (transform.position, Quaternion.identity, transform.lossyScale);
		Gizmos.DrawWireCube (center, size);
	}
}
