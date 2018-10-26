using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BuildScript
{
    static readonly string[] Scenes = FindEnabledEditorScenes();

	static readonly string AppName = PlayerSettings.productName;
    /// <summary>
    /// hardcoded, used in jenkins
    /// </summary>
    static string JENKINS_TARGET_DIR = "D:/ARTONCODE/_BUILD/WTOS";

    static void PerformDefaultWindowsBuild() {
        string appName = AppName;
		GenericBuild(Scenes, JENKINS_TARGET_DIR, appName, BuildTarget.StandaloneWindows, BuildOptions.None);
    }

    [MenuItem("Build/Custom Development Build")]
    static void PerformDevelopmentBuild()
    {
        var path = EditorUtility.SaveFolderPanel("Choose Location of Built Game: ", "", "");
        if (path.Length != 0) {
            string appName = AppName;
            try {
                int currentBuildNumber = GetBuildNumber();
                string versionedAppName = appName + " (b" + currentBuildNumber + ")";
                GenericBuild(Scenes, path, versionedAppName, EditorUserBuildSettings.activeBuildTarget, BuildOptions.Development);
                SetBuildNumber (currentBuildNumber + 1);
                EditorUtility.DisplayDialog ("Build Success", "You can access the build from: " + path, "Ok");
            }
            catch (Exception e) {
                EditorUtility.DisplayDialog ("Build Fail", e.ToString (), "Ok");
            }
        }
    }

    [MenuItem("Build/Custom Build %&b")]
    static void PerformBuild() {
		var path = EditorUtility.SaveFolderPanel("Choose Location of Built Game: ", "", "");
        if (path.Length != 0) {
            string appName = AppName;
			try {
				int currentBuildNumber = GetBuildNumber();
				string versionedAppName = appName + " (b" + currentBuildNumber + ")";
				GenericBuild(Scenes, path, versionedAppName, EditorUserBuildSettings.activeBuildTarget, BuildOptions.None);
				SetBuildNumber (currentBuildNumber + 1);
				EditorUtility.DisplayDialog ("Build Success", "You can access the build from: " + path, "Ok");
			}
			catch (Exception e) {
				EditorUtility.DisplayDialog ("Build Fail", e.ToString (), "Ok");
			}
        }
    }



    private static string[] FindEnabledEditorScenes() {
        return (from scene in EditorBuildSettings.scenes where scene.enabled select scene.path).ToArray();
    }

    static void GenericBuild(string[] scenes, string targetDir, string appName, BuildTarget buildTarget, BuildOptions buildOptions) {
        string res = BuildPipeline.BuildPlayer(scenes, targetDir+"/"+appName+".exe", buildTarget, buildOptions);
        if (res.Length > 0) {
            throw new Exception("BuildPlayer failure: " + res);
        }
        else {
			// Copy WWise soundbanks
			string wwiseProjFile = Path.Combine(Application.dataPath, WwiseSetupWizard.Settings.WwiseProjectPath).Replace('/', '\\');
			string wwiseProjectFolder = wwiseProjFile.Remove(wwiseProjFile.LastIndexOf(Path.DirectorySeparatorChar));
			string wwisePlatformString = UnityToWwisePlatformString(EditorUserBuildSettings.activeBuildTarget.ToString());
			string sourceSoundBankFolder = Path.Combine(wwiseProjectFolder, AkBasePathGetter.GetPlatformBasePath());
			string destinationSoundBankFolder = Path.Combine(targetDir + "/" + appName + "_Data/StreamingAssets",
				Path.Combine(WwiseSetupWizard.Settings.SoundbankPath, wwisePlatformString)
			);

			if (!AkUtilities.DirectoryCopy(sourceSoundBankFolder, destinationSoundBankFolder, true)) {
				Debug.LogError("WwiseUnity: The soundbank folder for the " + wwisePlatformString + " platform doesn't exist. Make sure it was generated in your Wwise project");
				throw new Exception("BuildPlayer failure: " + res);
			}
			Directory.GetFiles(targetDir + "/" + appName + "_Data/StreamingAssets", "*.meta", SearchOption.AllDirectories).ForEach(file => File.Delete(file));
        }
    }

	private static int GetBuildNumber()    {
		string buildNumberPath = Application.dataPath + "\\..\\buildNumber";
		int buildNumber = 1;

		if (File.Exists (buildNumberPath)) {
			StreamReader sr = File.OpenText (buildNumberPath);
			string s = sr.ReadLine ();
			sr.Close ();

			if (int.TryParse (s, out buildNumber)) {
				return buildNumber;
			}
			else {
				return 1;
			}
		}

		return buildNumber;
	}

	private static void SetBuildNumber (int buildNumber) {
		string buildNumberPath = Application.dataPath + "\\..\\buildNumber";

		if (!File.Exists (buildNumberPath)) {
			FileStream fs = File.Open(buildNumberPath, FileMode.OpenOrCreate);
			fs.Close();
		}

		StreamWriter sw = new StreamWriter(buildNumberPath, false);
		sw.WriteLine(buildNumber);
		sw.Close ();
	}

	private static string UnityToWwisePlatformString(string unityPlatormString) {
		if (unityPlatormString == BuildTarget.StandaloneWindows.ToString() ||
			unityPlatormString == BuildTarget.StandaloneWindows64.ToString()) {
			return "Windows";
		}
		else if (unityPlatormString == BuildTarget.StandaloneOSXIntel.ToString() ||
			unityPlatormString == BuildTarget.StandaloneOSXIntel64.ToString() ||
			unityPlatormString == BuildTarget.StandaloneOSXUniversal.ToString()) {
			return "Mac";
		}
		#if UNITY_5
		else if (unityPlatormString == BuildTarget.iOS.ToString()) {
		#else
		else if(unityPlatormString == BuildTarget.iPhone.ToString()) {
		#endif
			return "iOS";
		}
		else if (unityPlatormString == BuildTarget.XBOX360.ToString()) {
			return "Xbox360";
		}

		//Android, PS3 and Wii have the correct strings in Wwise v2013.2.7 and Unity version 4.3.4
		return unityPlatormString;
	}

	private static string getPlatFormExtension() {
		string unityPlatormString = EditorUserBuildSettings.activeBuildTarget.ToString();

		if (unityPlatormString == BuildTarget.StandaloneWindows.ToString() ||
			unityPlatormString == BuildTarget.StandaloneWindows64.ToString()) {
			return "exe";
		}
		else if (unityPlatormString == BuildTarget.StandaloneOSXIntel.ToString() ||
			unityPlatormString == BuildTarget.StandaloneOSXIntel64.ToString() ||
			unityPlatormString == BuildTarget.StandaloneOSXUniversal.ToString()) {
			return "app";
		}
		#if UNITY_5
		else if (unityPlatormString == BuildTarget.iOS.ToString()) {
		#else
		else if(unityPlatormString == BuildTarget.iPhone.ToString()) {
		#endif
			return "ipa";
		}
		else if (unityPlatormString == BuildTarget.XBOX360.ToString()) {
			return "XEX";
		}
		else if (unityPlatormString == BuildTarget.Android.ToString()) {
			return "apk";
		}
		else if (unityPlatormString == BuildTarget.PS3.ToString()) {
			return "self";
		}

		return "";
	}
}