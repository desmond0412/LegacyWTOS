using UnityEngine;
using System.Collections;

public class DieTriggerFall : DieTrigger
{
	public override void Start ()
	{
		base.Start ();
		causes = DieCausesType.Fall;
	}

	public override void DieAnimationFlow ()
	{
		camTransition = 0;
		camPosOffset = new Vector3 (0, 2, -13);
		camLookAtOffset = new Vector3 (0, 1, 0);
		CameraStopFollow ();

		//TODO call fading 
		StartDieFX ();
	}

	protected override void GameoverCameraObject_OnBlackoutEnded (GameObject sender)
	{
		gameoverCameraObject.OnBlackoutEnded -= GameoverCameraObject_OnBlackoutEnded;
		CreateEmptyPlane ();
		MainObjectSingleton.shared (MainObjectType.Player).GetComponent<Rigidbody> ().isKinematic = true;	
		LevPlayCustomAnimation (causes);
		MainObjectSingleton.shared (MainObjectType.Player).transform.position = this.gameObject.transform.position;
		MainObjectSingleton.shared (MainObjectType.Player).GetComponent<LevController> ().FacingDirectionInstant = LevController.Direction.Right;	
		CameraStartFocusOnLev ();

		gameoverCameraObject.OnTextComesOutEnded += GameoverCameraObject_OnTextComesOutEnded;
		sender.GetComponent<GameoverCameraManager> ().ShowText (dieLastWord);
	}


	protected override void CameraStartFocusOnLev ()
	{
		base.CameraStartFocusOnLev ();
		MainObjectSingleton.shared (MainObjectType.Camera).GetComponent<Artoncode.Core.SmoothFollow> ().target = this.gameObject.transform;
	}


	protected Mesh GetScriptedPlaneMesh (float width, float height)
	{
		Mesh m = new Mesh ();
		m.name = "ScriptedMesh";
		m.vertices = new Vector3[] {
			new Vector3 (-width, 0.01f, -height),
			new Vector3 (width, 0.01f, -height),
			new Vector3 (width, 0.01f, height),
			new Vector3 (-width, 0.01f, height)
		};
		m.uv = new Vector2[] {
			new Vector2 (0, 0),
			new Vector2 (0, 1),
			new Vector2 (1, 1),
			new Vector2 (1, 0)
		};
		m.triangles = new int[] { 2, 1, 0, 3, 2, 0 };
		m.RecalculateNormals ();

		return m;

	}

	protected void CreateEmptyPlane ()
	{
		GameObject planeGO = new GameObject ();
		planeGO.name = "DiePlane";
		planeGO.transform.SetParent (this.transform, false);
		MeshFilter MF = planeGO.AddComponent<MeshFilter> ();
		MF.mesh = GetScriptedPlaneMesh(3,3);
		MeshRenderer MR = planeGO.AddComponent<MeshRenderer> ();
		MR.material.shader = Shader.Find ("Unlit/Color");
		MR.material.color = Color.black;

		planeGO.layer = LayerMask.NameToLayer ("GameoverLayerUI");
	}

}
