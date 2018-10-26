using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Artoncode.Core.CameraPlatformer {
	[CustomEditor(typeof(CameraPath))]
	public class CameraPathEditor : Editor {

		CameraPath cameraPath;

		SerializedProperty nodesProp;
		SerializedProperty edgesProp;

		private static bool showSelectedOnly = true;
		private static bool showNodes = true;
		private static bool showEdges = true;

		private CameraPathEdge selectedEdge;
		private CameraPathNode selectedNode;

		private bool isSimulating = false;
		private Vector3 proxyPosition = Vector3.zero;
		private CameraSetting simulatedCameraSetting = new CameraSetting ();

		void OnEnable () {
			Tools.hidden = true;

			cameraPath = (CameraPath)target; 

			nodesProp = serializedObject.FindProperty("nodes");
			edgesProp = serializedObject.FindProperty("edges");

			Undo.undoRedoPerformed += Repaint;
			Selection.selectionChanged += Repaint;
		}

		void OnDisable () {
			Tools.hidden = false;

			Undo.undoRedoPerformed -= Repaint;
			Selection.selectionChanged -= Repaint;
		}

		public override void OnInspectorGUI () {
			drawInspector ();
		}

		void drawInspector () {
			serializedObject.Update ();

			if (Selection.activeObject is GameObject) {
				selectedEdge = ((GameObject)Selection.activeObject).GetComponent<CameraPathEdge> ();
				selectedNode = ((GameObject)Selection.activeObject).GetComponent<CameraPathNode> ();
			}
			else {
				selectedEdge = null;
				selectedNode = null;
			}

			showSelectedOnly = EditorGUILayout.Toggle ("Show Selected Only", showSelectedOnly);

			showNodes = EditorGUILayout.Foldout (showNodes, "Nodes");
			if (showNodes) {
				int nodeCount = nodesProp.arraySize;

				for(int i = 0; i < nodeCount; i++) {
					drawNodeInspector (cameraPath[i], i);
				}

				if (GUILayout.Button ("Add Node")) {
					GameObject nodeObject = new GameObject ("Node " + nodesProp.arraySize);
					Undo.RegisterCreatedObjectUndo (nodeObject, "Add Node");
					nodeObject.transform.parent = cameraPath.transform;
					nodeObject.transform.localPosition = Vector3.zero;

					CameraPathNode newNode = nodeObject.AddComponent<CameraPathNode> ();
					newNode.cameraSetting = new CameraSetting ();
					newNode.boxSize = Vector3.one;

					nodesProp.InsertArrayElementAtIndex (nodesProp.arraySize);
					nodesProp.GetArrayElementAtIndex (nodesProp.arraySize - 1).objectReferenceValue = newNode;

					Selection.activeObject = nodeObject;
				}
			}

			EditorGUILayout.Space ();

			showEdges = EditorGUILayout.Foldout (showEdges, "Edges");
			if (showEdges) {
				int edgeCount = edgesProp.arraySize;

				for(int i = 0; i < edgeCount; i++) {
					drawEdgeInspector (cameraPath.getEdge (i), i);
				}

				if (GUILayout.Button ("Add Edge")) {
					GameObject edgeObject = new GameObject ("Edge " + edgesProp.arraySize);
					Undo.RegisterCreatedObjectUndo (edgeObject, "Add Edge");
					edgeObject.transform.parent = cameraPath.transform;
					edgeObject.transform.localPosition = Vector3.zero;
					edgeObject.hideFlags = HideFlags.HideInHierarchy;

					CameraPathEdge newEdge = edgeObject.AddComponent<CameraPathEdge> ();
					newEdge.weight = AnimationCurve.Linear (0, 0, 1, 1);

					edgesProp.InsertArrayElementAtIndex (edgesProp.arraySize);
					edgesProp.GetArrayElementAtIndex (edgesProp.arraySize - 1).objectReferenceValue = newEdge;

					Selection.activeObject = edgeObject;
				}
			}

			EditorGUILayout.BeginVertical ("Box");
			isSimulating = EditorGUILayout.BeginToggleGroup ("Simulate", isSimulating);
			if (isSimulating) {
				Vector3 newProxyPosition = EditorGUILayout.Vector3Field("Proxy Position", proxyPosition);
				if (newProxyPosition != proxyPosition) {
					proxyPosition = newProxyPosition;
					updateSimulation ();
				}

				EditorGUILayout.Space ();
				EditorGUILayout.Space ();

				EditorGUILayout.ObjectField ("Camera Target", simulatedCameraSetting.cameraTarget, typeof (Transform), false);
				EditorGUILayout.FloatField ("Smooth Time", simulatedCameraSetting.smoothTime);
				EditorGUILayout.FloatField ("Scale", simulatedCameraSetting.scale);
				EditorGUILayout.Vector3Field ("Position Offset", simulatedCameraSetting.positionOffset);
				EditorGUILayout.Vector3Field ("Rotation Offset", simulatedCameraSetting.rotationOffset);
				EditorGUILayout.Vector3Field ("Min", simulatedCameraSetting.min);
				EditorGUILayout.Vector3Field ("Max", simulatedCameraSetting.max);
			}
			EditorGUILayout.EndToggleGroup ();
			EditorGUILayout.EndVertical ();

			if (GUI.changed) {
				serializedObject.ApplyModifiedProperties ();
				EditorUtility.SetDirty (target);
			}

			SceneView.RepaintAll ();
		}

		void drawEdgeInspector (CameraPathEdge edge, int index) {
			if (!edge) {
				EditorGUILayout.BeginHorizontal();

				if(GUILayout.Button("X", GUILayout.Width(20))) {
					Undo.RecordObject (cameraPath, "Remove Edge");
					edgesProp.MoveArrayElement(index, cameraPath.nodeCount - 1);
					edgesProp.arraySize--;
					return;
				}

				EditorGUILayout.LabelField ("Missing Edge");

				EditorGUILayout.EndHorizontal();

				return;
			}

			SerializedObject serObj = new SerializedObject (edge);

			SerializedProperty n1Prop = serObj.FindProperty("n1");
			SerializedProperty n2Prop = serObj.FindProperty("n2");

			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button ("X", GUILayout.Width(20))) {
				Undo.RecordObject (cameraPath, "Remove Edge");
				edgesProp.MoveArrayElement (index, cameraPath.edgeCount - 1);
				edgesProp.arraySize--;
				Undo.DestroyObjectImmediate (edge.gameObject);
				return;
			}

			if (!edge.n1 || !edge.n2) {
				GUI.backgroundColor = Color.red;
			}
			EditorGUILayout.ObjectField (edge.gameObject, typeof(GameObject), true);
			GUI.backgroundColor = Color.white;

			if(index != 0 && GUILayout.Button(@"/\", GUILayout.Width(25)))
			{
				UnityEngine.Object other = edgesProp.GetArrayElementAtIndex(index - 1).objectReferenceValue;
				edgesProp.GetArrayElementAtIndex(index - 1).objectReferenceValue = edge;
				edgesProp.GetArrayElementAtIndex(index).objectReferenceValue = other;
			}

			if(index != edgesProp.arraySize - 1 && GUILayout.Button(@"\/", GUILayout.Width(25)))
			{
				UnityEngine.Object other = edgesProp.GetArrayElementAtIndex(index + 1).objectReferenceValue;
				edgesProp.GetArrayElementAtIndex(index + 1).objectReferenceValue = edge;
				edgesProp.GetArrayElementAtIndex(index).objectReferenceValue = other;
			}

			EditorGUILayout.EndHorizontal ();

			if (!showSelectedOnly || selectedEdge == edge) {
				EditorGUILayout.BeginHorizontal();

				EditorGUI.indentLevel++;
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField (n1Prop, GUIContent.none);
				EditorGUI.indentLevel--;
				EditorGUI.indentLevel--;	
				EditorGUILayout.PropertyField (n2Prop, GUIContent.none);

				EditorGUILayout.EndHorizontal ();

				if (edge.n1 && edge.n2) {
					EditorGUI.indentLevel++;
					EditorGUI.indentLevel++;
					edge.weight = EditorGUILayout.CurveField (edge.weight, Color.yellow, new Rect (0, 0, 1, 1));
					EditorGUI.indentLevel--;
					EditorGUI.indentLevel--;	
				}
			}

			if(GUI.changed)
			{
				serObj.ApplyModifiedProperties();
				EditorUtility.SetDirty(serObj.targetObject);
			}
		}

		void drawNodeInspector (CameraPathNode node, int index) {
			if (!node) {
				EditorGUILayout.BeginHorizontal();

				if(GUILayout.Button("X", GUILayout.Width(20))) {
					Undo.RecordObject (cameraPath, "Remove Node");
					nodesProp.MoveArrayElement(index, cameraPath.nodeCount - 1);
					nodesProp.arraySize--;
					return;
				}

				GameObject temp = (GameObject)EditorGUILayout.ObjectField (null, typeof(GameObject), true);
				if (temp) {
					nodesProp.GetArrayElementAtIndex (index).objectReferenceValue = temp.GetComponent<CameraPathNode> ();
				}

				EditorGUILayout.EndHorizontal();

				return;
			}

			SerializedObject serObj = new SerializedObject (node);

			SerializedProperty typeProp = serObj.FindProperty("type");
			SerializedProperty cameraSettingProp = serObj.FindProperty("cameraSetting");
			SerializedProperty neighborsProp = serObj.FindProperty("neighbors");

			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button ("X", GUILayout.Width (20))) {
				Undo.RecordObject (cameraPath, "Remove Node");
				nodesProp.MoveArrayElement (index, cameraPath.nodeCount - 1);
				nodesProp.arraySize--;
				Undo.DestroyObjectImmediate (node.gameObject);
				return;
			}

			EditorGUILayout.ObjectField (node.gameObject, typeof(GameObject), true);

			if(index != 0 && GUILayout.Button(@"/\", GUILayout.Width (25)))
			{
				UnityEngine.Object other = nodesProp.GetArrayElementAtIndex (index - 1).objectReferenceValue;
				nodesProp.GetArrayElementAtIndex (index - 1).objectReferenceValue = node;
				nodesProp.GetArrayElementAtIndex (index).objectReferenceValue = other;
			}

			if(index != nodesProp.arraySize - 1 && GUILayout.Button (@"\/", GUILayout.Width(25)))
			{
				UnityEngine.Object other = nodesProp.GetArrayElementAtIndex (index + 1).objectReferenceValue;
				nodesProp.GetArrayElementAtIndex (index + 1).objectReferenceValue = node;
				nodesProp.GetArrayElementAtIndex (index).objectReferenceValue = other;
			}

			if (GUILayout.Button (@"D", GUILayout.Width (25))) {
				GameObject nodeObject = new GameObject ("Node " + nodesProp.arraySize);
				Undo.RegisterCreatedObjectUndo (nodeObject, "Duplicate Node");
				nodeObject.transform.parent = cameraPath.transform;
				nodeObject.transform.localPosition = node.transform.localPosition;

				CameraPathNode newNode = nodeObject.AddComponent<CameraPathNode> ();
				newNode.cameraSetting = new CameraSetting (node.cameraSetting);
				newNode.priority = node.priority;
				newNode.type = node.type;
				newNode.boxSize = node.boxSize;

				nodesProp.InsertArrayElementAtIndex (nodesProp.arraySize);
				nodesProp.GetArrayElementAtIndex (nodesProp.arraySize - 1).objectReferenceValue = newNode;

				Selection.activeObject = newNode;
			}

			EditorGUILayout.EndHorizontal ();

			if (!showSelectedOnly || selectedNode == node) {
				EditorGUI.indentLevel++;
				EditorGUI.indentLevel++;

				// Type field
				int newType = (int)((object)EditorGUILayout.EnumPopup("Node Type", (CameraPathNode.CameraPathNodeType)typeProp.enumValueIndex));
				if(newType != typeProp.enumValueIndex) {
					typeProp.enumValueIndex = newType;
				}

				// Position field
				Vector3 newNodePos = EditorGUILayout.Vector3Field("Position", node.transform.localPosition);
				if(newNodePos != node.transform.localPosition) {
					Undo.RegisterUndo (node.transform, "Move Node");
					node.transform.localPosition = newNodePos;
				}

				// Rotation field
				Vector3 newNodeRot = EditorGUILayout.Vector3Field("Rotation", node.transform.eulerAngles);
				if(newNodeRot != node.transform.eulerAngles) {
					Undo.RegisterUndo (node.transform, "Rotate Node");
					node.transform.eulerAngles = newNodeRot;
				}

				if (node.type == CameraPathNode.CameraPathNodeType.Volume) {
					Vector3 newNodeSize = EditorGUILayout.Vector3Field("Size", node.boxSize);
					if(newNodeSize != node.boxSize) {
						Undo.RegisterUndo (node, "Resize Node");
						node.boxSize = newNodeSize;
					}

					int newPriority = EditorGUILayout.IntSlider ("Priority", node.priority, 0, 10);
					if (newPriority != node.priority) {
						Undo.RegisterUndo (node, "Change Node Priority");
						node.priority = newPriority;
					}
				}

				GUI.backgroundColor = Color.green;
				EditorGUILayout.PropertyField (cameraSettingProp, true);
				GUI.backgroundColor = Color.white;

				EditorGUI.indentLevel--;
				EditorGUI.indentLevel--;	
			}

			if(GUI.changed)
			{
				serObj.ApplyModifiedProperties();
				EditorUtility.SetDirty(serObj.targetObject);
			}
		}

		void OnSceneGUI() {
			if (showSelectedOnly && !selectedEdge && !selectedNode) {
				Tools.hidden = false;
			}
			else {
				Tools.hidden = true;
			}

			if (isSimulating) {
				drawProxy ();
			}
			for (int i=0; i<cameraPath.nodeCount; i++) {
				drawNodeSceneGUI(cameraPath[i]);
			}
			for (int i=0; i<cameraPath.edgeCount; i++) {
				drawEdgeSceneGUI(cameraPath.getEdge (i));
			}

			for (int i=0; i<cameraPath.nodeCount; i++) {
				drawNodeSceneGUIButton(cameraPath[i]);
			}
			for (int i=0; i<cameraPath.edgeCount; i++) {
				drawEdgeSceneGUIButton(cameraPath.getEdge (i));
			}
		}

		public void drawProxy () {
			Handles.color = Color.magenta;
			Handles.SphereCap(0, proxyPosition, Quaternion.identity, HandleUtility.GetHandleSize (proxyPosition) / 5);

			if (Tools.current == Tool.Move) {
				Vector3 newPosition = Handles.DoPositionHandle (proxyPosition, Quaternion.identity);

				if (newPosition != proxyPosition) {
					Undo.RecordObject (this, "Move Proxy");
					proxyPosition = newPosition;

					updateSimulation ();

					Repaint ();
				}
			}
		}

		private void updateSimulation () {
			CameraSetting nearestcameraSetting = cameraPath.getNearestCameraSetting (proxyPosition);
			if (nearestcameraSetting != null) {
				simulatedCameraSetting = nearestcameraSetting;
			}
		}

		public void drawEdgeSceneGUI (CameraPathEdge edge) {
			if (edge.n1 != null && edge.n2 != null) {
				Handles.color = Color.yellow;
				if (isSimulating && cameraPath.lastNearestEdge == edge) {
					Handles.color = Color.cyan;
				}
				Handles.DrawAAPolyLine (Texture2D.whiteTexture, 3, edge.n1.transform.position, edge.n2.transform.position);
				if (edge == selectedEdge) {
					Handles.color = Color.red;
				}
				else {
					Handles.color = Color.yellow;
				}
				if (isSimulating && cameraPath.lastNearestEdge == edge) {
					Handles.color = Color.cyan;
				}
				Handles.DrawAAPolyLine (Texture2D.whiteTexture, 1, edge.n1.transform.position, edge.n2.transform.position);


				if (!showSelectedOnly || selectedEdge == edge) {
					Handles.color = new Color (1, 1, 1);
					Vector3 edgeDir = edge.n2.transform.position - edge.n1.transform.position;
					float size = (HandleUtility.GetHandleSize (edge.n1.transform.position) + HandleUtility.GetHandleSize (edge.n2.transform.position)) / 2;
					Vector3[] curveVerts = new Vector3[101];
					float interval = edgeDir.magnitude / (curveVerts.Length - 1);
					edgeDir.Normalize ();
					for (int i=0; i<curveVerts.Length; i++) {
						curveVerts[i] = edge.n1.transform.position + edgeDir * interval * i;
						curveVerts[i].y += size * edge.weight.Evaluate (((float)i) / (curveVerts.Length - 1));
					}

					for (int i=0; i<curveVerts.Length-1; i++) {
						float colorValue = ((float)i) / (curveVerts.Length - 1);
						Handles.DrawSolidRectangleWithOutline (new Vector3[] {
							edge.n1.transform.position + edgeDir * interval * i + Vector3.up * size,
							edge.n1.transform.position + edgeDir * interval * (i + 1) + Vector3.up * size,
							curveVerts[i+1],
							curveVerts[i]
						}, new Color (1-colorValue, 1-colorValue, 0, 0.3f), Color.clear);

						Handles.DrawSolidRectangleWithOutline (new Vector3[] {
							edge.n1.transform.position + edgeDir * interval * i,
							edge.n1.transform.position + edgeDir * interval * (i + 1),
							curveVerts[i+1],
							curveVerts[i]
						}, new Color (0, colorValue, 0, 0.3f), Color.clear);
					}

					Handles.DrawAAPolyLine (Texture2D.whiteTexture, 1, curveVerts);
				}
			}
		}

		public void drawEdgeSceneGUIButton (CameraPathEdge edge) {
			if (edge.n1 != null && edge.n2 != null) {
				if (showSelectedOnly && selectedEdge != edge) {
					Handles.BeginGUI ();
					Vector2 pos = HandleUtility.WorldToGUIPoint ((edge.n1.transform.position + edge.n2.transform.position) / 2);
					if (GUI.Button (new Rect (pos.x-10, pos.y-10, 20, 20), "E")) {
						Selection.activeObject = edge.gameObject;
					}
					Handles.EndGUI ();
				}
			}
		}

		public void drawNodeSceneGUI (CameraPathNode node) {
			if (node) {
				if (node == selectedNode || (selectedEdge != null && (selectedEdge.n1 == node || selectedEdge.n2 == node))) {
					Handles.color = Color.red;
				}
				else {
					Handles.color = Color.yellow;
				}
				if (isSimulating && cameraPath.lastNearestNode == node) {
					Handles.color = Color.cyan;
				}

				float size = HandleUtility.GetHandleSize (node.transform.position) / 5;
				if (node.type == CameraPathNode.CameraPathNodeType.Point) {
					Handles.SphereCap(0, node.transform.position, node.transform.rotation, size);
				}
				else {
					drawBox (node, Handles.color);
				}

				if (!showSelectedOnly || selectedNode == node) {
					if (Tools.current == Tool.Move) {
						Vector3 newPosition = Handles.DoPositionHandle (node.transform.position, node.transform.rotation);

						if (newPosition != node.transform.position) {
							Undo.RecordObject (node.transform, "Move Node");
							node.transform.position = newPosition;
							Repaint ();
						}
					}
					else if (Tools.current == Tool.Rotate) {
						Quaternion newRotation = Handles.DoRotationHandle (node.transform.rotation, node.transform.position);
						if (newRotation != node.transform.rotation) {
							Undo.RecordObject (node.transform, "Rotate Node");
							node.transform.rotation = newRotation;
							Repaint ();
						}
					}
					else if (Tools.current == Tool.Scale) {
						if (node.type == CameraPathNode.CameraPathNodeType.Volume) {
							Vector3 newScale = Handles.ScaleHandle (node.boxSize, node.transform.position, node.transform.rotation, HandleUtility.GetHandleSize (node.transform.position));
							if (newScale != node.boxSize) {
								Undo.RecordObject (node, "Resize Node");
								node.boxSize = newScale;
								Repaint ();
							}
						}
					}
				}
			}
		}

		public void drawNodeSceneGUIButton (CameraPathNode node) {
			if (node) {
				Vector2 pos = HandleUtility.WorldToGUIPoint (node.transform.position);

				Handles.BeginGUI ();
				if (showSelectedOnly && selectedNode != node) {
					string s = node.type == CameraPathNode.CameraPathNodeType.Point ? "P" : "V";
					if (GUI.Button (new Rect (pos.x-10, pos.y+5, 20, 20), s)) {
						Selection.activeObject = node.gameObject;
					}
				}

				GUIStyle labelStyle = GUI.skin.GetStyle ("Label");
				labelStyle.alignment = TextAnchor.MiddleCenter;
				labelStyle.fontStyle = FontStyle.Bold;
				GUI.Label (new Rect (pos.x-50, pos.y-30, 100, 20), node.gameObject.name, labelStyle);

				Handles.EndGUI ();				
			}
		}

		static public void drawBox (CameraPathNode node, Color color) {
			Transform box = node.transform;
			Vector3 half = node.boxSize / 2;
			Vector3[] verts = new Vector3[] {
				box.TransformPoint (new Vector3 (half.x, half.y, half.z)),
				box.TransformPoint (new Vector3 (half.x, -half.y, half.z)),
				box.TransformPoint (new Vector3 (-half.x, -half.y, half.z)),
				box.TransformPoint (new Vector3 (-half.x, half.y, half.z)),
				box.TransformPoint (new Vector3 (half.x, half.y, -half.z)),
				box.TransformPoint (new Vector3 (half.x, -half.y, -half.z)),
				box.TransformPoint (new Vector3 (-half.x, -half.y, -half.z)),
				box.TransformPoint (new Vector3 (-half.x, half.y, -half.z)),
			};
			Handles.color = Color.white;
			Color faceColor = new Color (color.r, color.g, color.b, 0.1f);
			Color outlineColor = new Color (1, 1, 0, 0.4f);
			Handles.DrawSolidRectangleWithOutline (new Vector3[] {verts[0], verts[1], verts[2], verts[3]}, faceColor, outlineColor);
			Handles.DrawSolidRectangleWithOutline (new Vector3[] {verts[4], verts[5], verts[6], verts[7]}, faceColor, outlineColor);
			Handles.DrawSolidRectangleWithOutline (new Vector3[] {verts[0], verts[1], verts[5], verts[4]}, faceColor, outlineColor);
			Handles.DrawSolidRectangleWithOutline (new Vector3[] {verts[3], verts[2], verts[6], verts[7]}, faceColor, outlineColor);
			Handles.DrawSolidRectangleWithOutline (new Vector3[] {verts[0], verts[4], verts[7], verts[3]}, faceColor, outlineColor);
			Handles.DrawSolidRectangleWithOutline (new Vector3[] {verts[1], verts[5], verts[6], verts[2]}, faceColor, outlineColor);
		}

		[MenuItem("GameObject/Create Other/Camera Path")]
		public static void CreateCameraPath (MenuCommand command) {
			GameObject cameraPathObject = new GameObject("CameraPath");
			Undo.RegisterUndo(cameraPathObject, "Undo Create CameraPath");
			cameraPathObject.AddComponent<CameraPath>();
			cameraPathObject.transform.position = Vector3.zero;
		}
	}
}