using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using SmartLocalization;

public class DialogCreator {

	[MenuItem("Assets/Create/Dialog")]
	public static void createDialog ()
	{
		AssetCreator.CreateObject<DialogSO>("Dialog");
	}

	[MenuItem("Assets/Generate/Dialog From Smart Localization")]
	public static void generatedialog()
	{
		string basePath = "Assets/ScriptableObject";

		DialogSO lastDialogSO = null;
		string lastCutSceneFolderID = "";
		int lastLineIdx = 0 ;


		if(LanguageManager.Instance == null ) 
		{
			Debug.LogError("Need to run in PLAY MODE"); return;
		}

		foreach (string item in LanguageManager.Instance.GetAllKeys()) {

			string[] splittedPath = item.Split('.');
			if(splittedPath[0] != "Dialog") continue;

			if(splittedPath.Length != 7)
			{
				string assetPath = basePath + "/Others";
				CreateFolder(assetPath);
				DialogSO asset = AssetCreator.CreateObject<DialogSO>(item,assetPath);
				asset.objectID = item;
			}
			else
			{ 
				string dialogFolder		= splittedPath[0];
				string areaFolder 		= splittedPath[1];
				string puzzleFolder 	= splittedPath[2];
				string cutsceneFolder	= splittedPath[3];
				string dialogLine		= splittedPath[4];
				string actor			= splittedPath[5];

				int currLineIdx = int.Parse(dialogLine);


				string assetPath = basePath + "/" + dialogFolder + "/" + areaFolder + "/" + puzzleFolder + "/" + cutsceneFolder;
				string filePath = assetPath + "/" + item + ".asset";
				//				Dialog.A01.P02.CS01.01.Lev.01

				CreateFolder(assetPath);
				DialogSO asset = null;

				if(!File.Exists(filePath)) 
				{
					asset = AssetCreator.CreateObject<DialogSO>(item,assetPath);
					asset.objectID = item;
					asset.alignment = DialogAlignmentType.TopCenter;
					asset.SetActor(actor);
				}
				else
				{
					asset = AssetDatabase.LoadAssetAtPath<DialogSO>(filePath);
				}



				if(cutsceneFolder.Equals(lastCutSceneFolderID)) //if in the same cutscene folder
				{
					if(currLineIdx > lastLineIdx)
					{
						lastDialogSO.nextDialog = asset;	
						lastLineIdx = currLineIdx;
					}	
				}
				else //reset 
				{
					lastLineIdx = 0;
				}

				lastCutSceneFolderID = cutsceneFolder;
				lastDialogSO = asset;
				EditorUtility.SetDirty(asset);
			}


		}

		Debug.Log("GENERATE SUCCESSFUL");
	}

	private static void CreateFolder(string path)
	{
		
		Debug.Log(Directory.Exists(path));
		if(!Directory.Exists(path))
		{
			System.IO.Directory.CreateDirectory(path);
		}
	}



//	private List<string> GetScenes(){
//
//
//		List<string> levelContainer = new List<string>();
//		foreach (string path in scenesPath) {
//			string realPath = path.Replace("\\","/"); 
//			string sceneFullName = realPath.Substring(realPath.LastIndexOf("/")+1); // remove assets/level/, only get [level].unity
//			string sceneName = sceneFullName.Substring(0,sceneFullName.Length - 6); // remove .unity, only get [level]
//			levelContainer.Add(sceneName);
//		}
//
//		return levelContainer;
//	}
}
