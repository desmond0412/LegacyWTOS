using UnityEngine;
using System.Collections;

public class StatusViewer : MonoBehaviour
{

    bool[] states;

    void OnEnable()
    {
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Save"))
            {
                GameDataManager.shared().Save();
            }
            if (GUILayout.Button("Reset Status"))
            {
                PuzzleStateManager.shared().Reset();
                GameDataManager.shared().Save();
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginVertical();
        GUILayout.Label(PuzzleStateManager.shared().ToString(), "box");
        GUILayout.EndVertical();
    }

}
