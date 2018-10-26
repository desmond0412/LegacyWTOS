using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Linq;

[CustomPropertyDrawer(typeof(LocationModel))]
public class LocationModelPropertyDrawer : PropertyDrawer {

	bool once = true;
	List<string> sceneList;
	int selectedIdx = 0;

	private List<string> GetScenes(){

		string[] scenesPath = Directory.GetFiles ("Assets/scenes/levels", "*.unity", SearchOption.AllDirectories);

		List<string> levelContainer = new List<string>();
		foreach (string path in scenesPath) {
			string realPath = path.Replace("\\","/"); 
			string sceneFullName = realPath.Substring(realPath.LastIndexOf("/")+1); // remove assets/level/, only get [level].unity
			string sceneName = sceneFullName.Substring(0,sceneFullName.Length - 6); // remove .unity, only get [level]
			levelContainer.Add(sceneName);
		}

		return levelContainer;
	}

	private void initOnce()
	{
		if(once)
		{
			once = false;
			sceneList = GetScenes();
		}

	}

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
        if(property.hasMultipleDifferentValues) return;

		initOnce();
		EditorGUI.BeginProperty (position, label, property);
		position = EditorGUI.PrefixLabel (position, GUIUtility.GetControlID (FocusType.Passive), label);


		SerializedProperty SPSceneName = property.FindPropertyRelative("sceneName");
		SerializedProperty SPLocation = property.FindPropertyRelative("location");


		Rect sceneRect = new Rect(position.x,position.y,position.width/2-5,position.height);
		Rect locRect = new Rect(position.width/2+5 + position.x,position.y,position.width/2-5,position.height);


		//SCENE
		selectedIdx = sceneList.IndexOf(SPSceneName.stringValue);
		selectedIdx = Mathf.Clamp(selectedIdx,0,sceneList.Count);
		selectedIdx = EditorGUI.Popup(sceneRect,selectedIdx,sceneList.ToArray());
		SPSceneName.stringValue = sceneList[selectedIdx];
		//SCENE

		//Location
//		SPLocation = EditorGUI.EnumPopup(locRect
		SPLocation.enumValueIndex = EditorGUI.Popup(locRect,SPLocation.enumValueIndex,SPLocation.enumDisplayNames);

		
		//Location






		EditorGUI.EndProperty();
	}


}
