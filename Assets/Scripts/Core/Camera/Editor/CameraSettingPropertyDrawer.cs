using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Artoncode.Core.CameraPlatformer {
	[CustomPropertyDrawer(typeof(CameraSetting))]
	public class CameraSettingPropertyDrawer : PropertyDrawer {

		private static bool showProperty = true;

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
			if (showProperty) {
				SerializedProperty typeProp = property.FindPropertyRelative ("cameraTargetType");
				if (typeProp.enumValueIndex == (int)CameraTargetGrabber.CameraTargetType.SceneObject) {
					return 18 * property.CountInProperty ();
				}
				return 18 * (property.CountInProperty () - 1);
			}
			return base.GetPropertyHeight (property, label);
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
			if(property.hasMultipleDifferentValues) return;

			SerializedProperty typeProp = property.FindPropertyRelative ("cameraTargetType");

			position.size = new Vector2 (position.size.x, 16);
			Rect childPos = new Rect (position);
			nextRow (ref childPos);

			EditorGUI.BeginProperty (position, label, property);

			showProperty = EditorGUI.Foldout (position, showProperty, label.text);

			if (showProperty) {
				EditorGUI.indentLevel ++;
				EditorGUI.PropertyField (childPos, typeProp); nextRow (ref childPos);
				if (typeProp.enumValueIndex == (int)CameraTargetGrabber.CameraTargetType.SceneObject) {
					EditorGUI.PropertyField (childPos, property.FindPropertyRelative ("cameraTarget")); nextRow (ref childPos);
				}
				EditorGUI.PropertyField (childPos, property.FindPropertyRelative ("smoothTime")); nextRow (ref childPos);
				EditorGUI.PropertyField (childPos, property.FindPropertyRelative ("scale")); nextRow (ref childPos);
				EditorGUI.PropertyField (childPos, property.FindPropertyRelative ("positionOffset")); nextRow (ref childPos);
				EditorGUI.PropertyField (childPos, property.FindPropertyRelative ("rotationOffset")); nextRow (ref childPos);
				EditorGUI.PropertyField (childPos, property.FindPropertyRelative ("min")); nextRow (ref childPos);
				EditorGUI.PropertyField (childPos, property.FindPropertyRelative ("max")); nextRow (ref childPos);
				EditorGUI.indentLevel --;
			}

			EditorGUI.EndProperty();
		}

		private void nextRow (ref Rect rect) {
			rect.position = new Vector2 (rect.position.x, rect.position.y + 18);
		}
	}
}