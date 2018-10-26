///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Sprite Color FX.
// Copyright (c) Ibuprogames. All rights reserved.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEditor;

namespace SpriteColorFX
{
  /// <summary>
  /// SpriteColorEditor base.
  /// </summary>
  [CustomEditor(typeof(SpriteColorBase))]
  public abstract class SpriteColorBaseEditor : Editor
  {
    /// <summary>
    /// Help text.
    /// </summary>
    public string Help { get; set; }

    /// <summary>
    /// Warnings.
    /// </summary>
    public string Warnings { get; set; }

    /// <summary>
    /// Errors.
    /// </summary>
    public string Errors { get; set; }

    private SpriteColorBase baseTarget;

    /// <summary>
    /// OnInspectorGUI.
    /// </summary>
    public override void OnInspectorGUI()
    {
      if (baseTarget == null)
        baseTarget = this.target as SpriteColorBase;

      EditorGUIUtility.LookLikeControls();

      EditorGUI.indentLevel = 0;

      EditorGUIUtility.labelWidth = 125.0f;

      EditorGUILayout.BeginVertical();
      {
        EditorGUILayout.Separator();

        /////////////////////////////////////////////////
        // Lit.
        /////////////////////////////////////////////////

        baseTarget.LightMode = (LightMode)EditorGUILayout.EnumPopup(@"Lighting mode", baseTarget.LightMode);
        if (baseTarget.LightMode == LightMode.BumpLit)
        {
          EditorGUI.indentLevel++;

          baseTarget.shininess = SpriteColorFXEditorHelper.SliderWithReset(@"Shininess", @"", baseTarget.shininess, 0.03f, 1.0f, 0.078125f);

          baseTarget.specularColor = EditorGUILayout.ColorField(@"Specular color", baseTarget.specularColor);

          baseTarget.normalTex = EditorGUILayout.ObjectField(@"Bump texture", baseTarget.normalTex, typeof(Texture), false) as Texture;

          EditorGUI.indentLevel--;
        }

        /////////////////////////////////////////////////
        // Common.
        /////////////////////////////////////////////////

        Inspector();

        EditorGUILayout.Separator();

        /////////////////////////////////////////////////
        // Misc.
        /////////////////////////////////////////////////

        EditorGUILayout.BeginHorizontal();
        {
          if (GUILayout.Button(new GUIContent("[web]", "Open website"), GUI.skin.label) == true)
            Application.OpenURL(SpriteColorFXEditorHelper.DocumentationURL);

          GUILayout.FlexibleSpace();

          if (GUILayout.Button("Reset ALL") == true)
            ResetDefaultValues();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();

        if (string.IsNullOrEmpty(Warnings) == false)
        {
          EditorGUILayout.HelpBox(Warnings, MessageType.Warning);

          EditorGUILayout.Separator();
        }

        if (string.IsNullOrEmpty(Errors) == false)
        {
          EditorGUILayout.HelpBox(Errors, MessageType.Error);

          EditorGUILayout.Separator();
        }

        if (string.IsNullOrEmpty(Help) == false)
          EditorGUILayout.HelpBox(Help, MessageType.Info);
      }
      EditorGUILayout.EndVertical();

      Warnings = Errors = string.Empty;

      if (GUI.changed == true)
        EditorUtility.SetDirty(target);

      EditorGUIUtility.LookLikeControls();

      EditorGUI.indentLevel = 0;

      EditorGUIUtility.labelWidth = 125.0f;
    }

    /// <summary>
    /// Set the default values.
    /// </summary>
    protected virtual void ResetDefaultValues()
    {
      baseTarget.shininess = 0.078125f;

      baseTarget.specularColor = Color.gray;
    }

    /// <summary>
    /// Inspector.
    /// </summary>
    protected virtual void Inspector()
    {
      DrawDefaultInspector();
    }
  }
}