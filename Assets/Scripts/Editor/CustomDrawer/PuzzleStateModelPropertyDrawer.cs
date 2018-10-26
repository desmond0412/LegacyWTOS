using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Linq;

[CustomPropertyDrawer(typeof(PuzzleStateModel))]
public class PuzzleStateModelPropertyDrawer : PropertyDrawer {

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
        if(property.hasMultipleDifferentValues) return;

		EditorGUI.BeginProperty (position, label, property);
		

		SerializedProperty SPState = property.FindPropertyRelative("state");
		SerializedProperty SPValue= property.FindPropertyRelative("value");


		Rect rectState = new Rect(position.x,position.y,position.width-20,position.height);
        Rect rectValue = new Rect(position.width-30 + position.x,position.y,30,position.height);

        EditorGUI.PropertyField(rectState,SPState);
        SPValue.boolValue = EditorGUI.Toggle(rectValue,SPValue.boolValue);

		EditorGUI.EndProperty();
	}


}
