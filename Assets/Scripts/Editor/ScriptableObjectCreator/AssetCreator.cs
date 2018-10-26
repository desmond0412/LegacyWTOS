using UnityEngine;
using UnityEditor;
using System.Collections;

public class AssetCreator {

	/// <summary>
	/// Creates the object.
	/// </summary>
	/// <param name="itemName">Item name.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static void CreateObject<T> (string itemName)where T: ScriptableObject
	{
//		T asset = ScriptableObject.CreateInstance<T> ();
//		string path = AssetDatabase.GetAssetPath (Selection.activeObject) + "/"+ itemName+".asset";
//		AssetDatabase.CreateAsset (asset, path);
//		AssetDatabase.SaveAssets ();
		CreateObject<T>(itemName,AssetDatabase.GetAssetPath (Selection.activeObject));
	}

	public static T CreateObject<T> (string itemName,string itemPath)where T: ScriptableObject
	{
		T asset = ScriptableObject.CreateInstance<T> ();
		string path = itemPath + "/"+ itemName+".asset";
		AssetDatabase.CreateAsset (asset, path);
		AssetDatabase.SaveAssets ();

		return asset;
	}

}
