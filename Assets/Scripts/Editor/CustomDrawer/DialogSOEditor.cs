using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(DialogSO))]
public class DialogSOEditor : Editor {
	


	public override void OnInspectorGUI ()
	{
//		base.OnInspectorGUI ();
		serializedObject.Update();

		SerializedProperty key			 		= serializedObject.FindProperty("objectID");
		SerializedProperty actor 				= serializedObject.FindProperty("actor");
		SerializedProperty lifetime 			= serializedObject.FindProperty("lifetime");
		SerializedProperty fadeIntime 			= serializedObject.FindProperty("fadeInTime");
		SerializedProperty fadeOuttime 			= serializedObject.FindProperty("fadeOutTime");
		SerializedProperty extraOffset 			= serializedObject.FindProperty("extraOffset");


		SerializedProperty alignment 			= serializedObject.FindProperty("alignment");
		SerializedProperty fontSize 			= serializedObject.FindProperty("fontSize");

		SerializedProperty isWithNext			= serializedObject.FindProperty("isWithNext");

		SerializedProperty isTriggerAnimation 	= serializedObject.FindProperty("isTriggerAnimation");
		SerializedProperty animationName 		= serializedObject.FindProperty("animationName");

		SerializedProperty nextDialog			= serializedObject.FindProperty("nextDialog");



		EditorGUILayout.PropertyField(key);

//		EditorGUILayout.BeginHorizontal();
//		EditorGUILayout.PrefixLabel("Actor");
//
//		selectedIdx = EditorGUILayout.Popup(selectedIdx,System.Enum.GetNames(typeof(ActorType)));
//		actor.stringValue = System.Enum.GetNames(typeof(ActorType))[selectedIdx];
//		EditorGUILayout.EndHorizontal();

		EditorGUILayout.PropertyField(actor);
		EditorGUILayout.PropertyField(alignment);
		EditorGUILayout.PropertyField(fontSize);
		EditorGUILayout.PropertyField(lifetime);

		EditorGUILayout.PropertyField(fadeIntime);
		EditorGUILayout.PropertyField(fadeOuttime);
		EditorGUILayout.PropertyField(extraOffset);


		//TRIGGER ANIMATION
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(isTriggerAnimation);
		if(isTriggerAnimation.boolValue == true)
			animationName.stringValue = EditorGUILayout.TextField(animationName.stringValue);
//			EditorGUILayout.PropertyField(animationName);
		EditorGUILayout.EndHorizontal();
		//TRIGGER ANIMATION

		EditorGUILayout.PropertyField(isWithNext);



		EditorGUILayout.PropertyField(nextDialog);

		serializedObject.ApplyModifiedProperties();
//		EditorUtility.SetDirty(this);
	}

}
