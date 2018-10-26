using UnityEngine;
using UnityEditor;
using System.Collections;

public class LevControllerSOCreator {

	[MenuItem("Assets/Create/LevController SO")]
	public static void createSO ()
	{
		AssetCreator.CreateObject<LevControllerSO>("LevControllerSetting");
	}

}
