using UnityEngine;
using System.Collections;

[RequireComponent(typeof (MeshRenderer))]
public class FogBezier : MonoBehaviour {
	[HideInInspector]
	public BezierCurve bc;

	//public properties
	public float pHeight = 1.0f;
	public Gradient colorOvertime;

	float lastPHeight = 0.0f;
	MeshFilter mf;
	Renderer rend;
	FogMaterial fm;

	//Mesh Components
	int numOfBezierSegment;
	int numOfLine;
	int numOfSegment;
	int numOfVertices;
	int numOfTris;
	int numOfTrisIndices;

	Vector3[] vertices;
	Vector3[] normals;
	Vector2[] uvs;
	Color[] colors;
	int[] triangleIndices;

	Mesh mesh;

	[Header("DEBUG")]
	public bool drawVertices;
	public bool drawLines;
	public bool drawMesh;
//	public bool drawTexture;
//	public bool animateTexture;

	void Start(){
		if (bc == null) {
			bc.GetComponent<BezierCurve> ();
		}

		mesh = CreateMesh();
		rend = GetComponent<Renderer> ();
		fm = GetComponent<FogMaterial> ();

		numOfBezierSegment = bc.pointCount - 1;
		numOfLine = bc.smoothness * numOfBezierSegment + 1;
		numOfSegment = numOfLine - 1;
		numOfVertices = numOfLine * 2;
		numOfTris = numOfSegment * 2;
		numOfTrisIndices = numOfTris * 3;

		vertices = new Vector3[numOfVertices];
		normals = new Vector3[numOfVertices];
		uvs = new Vector2[numOfVertices];
		triangleIndices = new int[numOfTrisIndices];
		colors = new Color[numOfVertices];
	}

	void Update(){
		UpdateMesh ();
	}

	private Mesh CreateMesh(){
		if (GetComponent<MeshFilter> () == null) {
			gameObject.AddComponent<MeshFilter> ();
		}

		mf = GetComponent<MeshFilter> ();
		if (mf.sharedMesh == null) {
			mf.sharedMesh = new Mesh ();
		}

		return mf.sharedMesh;
	}

	private void UpdateMesh(){
		if (bc.completeCurve || !Mathf.Approximately (pHeight, lastPHeight)) {
			for (int i = 0; i < numOfLine; i++) {
				Vector3 currPoint = bc.GetPointAt (Mathf.Clamp ((float)i / (float)(numOfLine - 1), 0.01f, 0.99f));
				Vector3 currTangent = bc.GetTangentAt (Mathf.Clamp ((float)i / (float)(numOfLine - 1), 0.01f, 0.99f));
				Vector3 bidir = Vector3.Cross (Vector3.up, currTangent);
				Vector3 upLine = Vector3.Cross (currTangent, bidir);

				Vector3 point1 = transform.InverseTransformPoint (currPoint + upLine.normalized * pHeight / 2);
				Vector3 point2 = transform.InverseTransformPoint (currPoint - upLine.normalized * pHeight / 2);

				vertices [i * 2] = point1;
				vertices [i * 2 + 1] = point2;

				normals [i * 2] = bidir;
				normals [i * 2 + 1] = bidir;

				uvs [i * 2] = new Vector2 ((float)i / (float)(numOfLine - 1), 1.0f);
				uvs [i * 2 + 1] = new Vector2 ((float)i / (float)(numOfLine - 1), 0.0f);

				if (i > 0) {
					triangleIndices [(i - 1) * 6] = (i - 1) * 2;
					triangleIndices [(i - 1) * 6 + 1] = i * 2;
					triangleIndices [(i - 1) * 6 + 2] = i * 2 + 1;
					triangleIndices [(i - 1) * 6 + 3] = i * 2 + 1;
					triangleIndices [(i - 1) * 6 + 4] = (i - 1) * 2 + 1;
					triangleIndices [(i - 1) * 6 + 5] = (i - 1) * 2;
				}

				colors [i * 2] = colorOvertime.Evaluate ((float)i / (float)(numOfLine - 1));
				colors [i * 2 + 1] = colorOvertime.Evaluate ((float)i / (float)(numOfLine - 1));
			}

			mesh.Clear ();
			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.uv = uvs;
			mesh.triangles = triangleIndices;
			mesh.colors = colors;
		}

		lastPHeight = pHeight;

//		if (drawVertices || drawLines || drawMesh || drawTexture || animateTexture) {
//			if (drawTexture && !rend.enabled) {
//				rend.enabled = true;
//			} else if(!drawTexture && rend.enabled){
//				rend.enabled = false;
//			}
//
//			if (animateTexture && !fm.enabled) {
//				fm.enabled = true;
//			} else if(!animateTexture && fm.enabled){
//				fm.enabled = false;
//			}
//		} else if(!(drawVertices || drawLines || drawMesh || drawTexture || animateTexture)){
//			rend.enabled = true;
//			fm.enabled = true;
//		}
	}

	void OnDrawGizmosSelected(){
		if (drawVertices) {
			foreach(Vector3 v in vertices){
				Gizmos.DrawSphere (transform.TransformPoint(v), 0.1f);
			}
		}

		if (drawLines) {
			for (int i = 0; i < numOfLine; i++) {
				Debug.DrawLine (transform.TransformPoint(vertices [i * 2]), 
					transform.TransformPoint(vertices [i * 2 + 1])
					,Color.green);
			}
		}

		if (drawMesh) {
			for (int i = 0; i < numOfLine - 1; i++) {
				Debug.DrawLine (transform.TransformPoint(vertices [i * 2]), 
					transform.TransformPoint(vertices [(i + 1) * 2])
					,Color.green);

				Debug.DrawLine (transform.TransformPoint(vertices [i * 2 + 1]), 
					transform.TransformPoint(vertices [(i + 1) * 2 + 1])
					,Color.green);

				Debug.DrawLine (transform.TransformPoint(vertices [i * 2]), 
					transform.TransformPoint(vertices [(i + 1) * 2 + 1])
					,Color.green);
			}
		}
	}
}
