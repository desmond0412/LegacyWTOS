using UnityEngine;
using UnityEditor;
using System.Collections;

class AssetPreProcessor : AssetPostprocessor {

	void OnPreprocessModel(){
		//REMOVE IMPORT MATERIAL WHILE IMPORTING FBX
		ModelImporter importer = (ModelImporter)assetImporter;
		importer.importMaterials = false;
		importer.animationCompression = ModelImporterAnimationCompression.Off;
	}

}