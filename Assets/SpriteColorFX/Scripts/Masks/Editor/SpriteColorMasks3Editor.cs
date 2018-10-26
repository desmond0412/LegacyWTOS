///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Sprite Color FX.
// Copyright (c) Ibuprogames. All rights reserved.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;

using UnityEditor;
using UnityEngine;

namespace SpriteColorFX
{
  /// <summary>
  /// SpriteColorMasks3 editor.
  /// </summary>
  [CustomEditor(typeof(SpriteColorMasks3))]
  public sealed class SpriteColorMasks3Editor : SpriteColorBaseEditor
	{
		private SpriteColorMasks3 baseTarget;

    /// <summary>
    /// Set the default values.
    /// </summary>
    protected override void ResetDefaultValues()
    {
      if (baseTarget == null)
        baseTarget = this.target as SpriteColorMasks3;

      baseTarget.strength = 1.0f;
      baseTarget.pixelOp = SpriteColorHelper.PixelOp.Multiply;

      baseTarget.strengthMaskRed = 1.0f;
      baseTarget.colorMaskRed = Color.white;
      baseTarget.textureMaskRedUVParams = new Vector4(1.0f, 1.0f, 0.0f, 0.0f);
      baseTarget.textureMaskRedUVAngle = 0.0f;

      baseTarget.strengthMaskGreen = 1.0f;
      baseTarget.colorMaskGreen = Color.white;
      baseTarget.textureMaskGreenUVParams = new Vector4(1.0f, 1.0f, 0.0f, 0.0f);
      baseTarget.textureMaskGreenUVAngle = 0.0f;

      baseTarget.strengthMaskBlue = 1.0f;
      baseTarget.colorMaskBlue = Color.white;
      baseTarget.textureMaskBlueUVParams = new Vector4(1.0f, 1.0f, 0.0f, 0.0f);
      baseTarget.textureMaskBlueUVAngle = 0.0f;

      base.ResetDefaultValues();
    }

    /// <summary>
    /// Inspector.
    /// </summary>
    protected override void Inspector()
    {
			if (baseTarget == null)
				baseTarget = base.target as SpriteColorMasks3;

			EditorGUIUtility.fieldWidth = 40.0f;
			
      baseTarget.strength = (float)SpriteColorFXEditorHelper.IntSliderWithReset(@"Strength", SpriteColorFXEditorHelper.TooltipStrength, Mathf.RoundToInt(baseTarget.strength * 100.0f), 0, 100, 100) * 0.01f;

      SpriteColorHelper.PixelOp newPixelOp = (SpriteColorHelper.PixelOp)EditorGUILayout.EnumPopup(new GUIContent(@"Blend mode", @"Blend modes"), baseTarget.pixelOp);
      if (newPixelOp != baseTarget.pixelOp)
        baseTarget.SetPixelOp(newPixelOp);

      EditorGUILayout.LabelField(@"#1 mask (red)");
			{
				EditorGUI.indentLevel++;

				baseTarget.strengthMaskRed = (float)SpriteColorFXEditorHelper.IntSliderWithReset(@"Strength", SpriteColorFXEditorHelper.TooltipStrength, Mathf.RoundToInt(baseTarget.strengthMaskRed * 100f), 0, 100, 100) * 0.01f;
				  
        baseTarget.colorMaskRed = EditorGUILayout.ColorField(@"Color", baseTarget.colorMaskRed);
				  
        baseTarget.textureMaskRed = (EditorGUILayout.ObjectField(new GUIContent(@"Texture (RGB)", SpriteColorFXEditorHelper.TooltipTextureMask), baseTarget.textureMaskRed, typeof(Texture2D), false, GUILayout.Height(54.0f)) as Texture2D);
				if (baseTarget.textureMaskRed != null)
        {
          EditorGUILayout.LabelField(@"UV params");
            
          UVParamsInspectorGUI(ref baseTarget.textureMaskRedUVParams, ref baseTarget.textureMaskRedUVAngle);
        }

				EditorGUI.indentLevel--;
			}

      EditorGUILayout.LabelField(@"#2 mask (green)");
			{
				EditorGUI.indentLevel++;
				  
        baseTarget.strengthMaskGreen = (float)SpriteColorFXEditorHelper.IntSliderWithReset(@"Strength", SpriteColorFXEditorHelper.TooltipStrength, Mathf.RoundToInt(baseTarget.strengthMaskGreen * 100f), 0, 100, 100) * 0.01f;
				  
        baseTarget.colorMaskGreen = EditorGUILayout.ColorField(@"Color", baseTarget.colorMaskGreen);

        baseTarget.textureMaskGreen = (EditorGUILayout.ObjectField(new GUIContent(@"Texture (RGB)", SpriteColorFXEditorHelper.TooltipTextureMask), baseTarget.textureMaskGreen, typeof(Texture2D), false, GUILayout.Height(54.0f)) as Texture2D);
        if (baseTarget.textureMaskGreen != null)
        {
          EditorGUILayout.LabelField(@"UV params");

          UVParamsInspectorGUI(ref baseTarget.textureMaskGreenUVParams, ref baseTarget.textureMaskGreenUVAngle);
        }

				EditorGUI.indentLevel--;
			}

      EditorGUILayout.LabelField(@"#3 mask (blue)");
			{
				EditorGUI.indentLevel++;

				baseTarget.strengthMaskBlue = (float)SpriteColorFXEditorHelper.IntSliderWithReset(@"Strength", SpriteColorFXEditorHelper.TooltipStrength, Mathf.RoundToInt(baseTarget.strengthMaskBlue * 100f), 0, 100, 100) * 0.01f;
          
        baseTarget.colorMaskBlue = EditorGUILayout.ColorField(@"Color", baseTarget.colorMaskBlue);

        baseTarget.textureMaskBlue = (EditorGUILayout.ObjectField(new GUIContent(@"Texture (RGB)", SpriteColorFXEditorHelper.TooltipTextureMask), baseTarget.textureMaskBlue, typeof(Texture2D), false, GUILayout.Height(54.0f)) as Texture2D);
				if (baseTarget.textureMaskBlue != null)
        {
          EditorGUILayout.LabelField(@"UV params");
            
          UVParamsInspectorGUI(ref baseTarget.textureMaskBlueUVParams, ref baseTarget.textureMaskBlueUVAngle);
        }

				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Separator();

      baseTarget.textureMask = (EditorGUILayout.ObjectField(new GUIContent(@"Mask #1 (RGB)", SpriteColorFXEditorHelper.TooltipTextureMask), this.baseTarget.textureMask, typeof(Texture2D), false, GUILayout.Height(54.0f)) as Texture2D);
		}

		private void UVParamsInspectorGUI(ref Vector4 uvParams, ref float angle)
		{
      EditorGUI.indentLevel++;

      uvParams.x = SpriteColorFXEditorHelper.SliderWithReset(@"U coord scale", @"U texture coordinate scale", uvParams.x, -5f, 5f, 1f);
      uvParams.y = SpriteColorFXEditorHelper.SliderWithReset(@"V coord scale", @"V texture coordinate scale", uvParams.y, -5f, 5f, 1f);
      uvParams.z = SpriteColorFXEditorHelper.SliderWithReset(@"U coord vel", @"U texture coordinate velocity", uvParams.z, -2f, 2f, 0f);
      uvParams.w = SpriteColorFXEditorHelper.SliderWithReset(@"V coord vel", @"V texture coordinate velocity", uvParams.w, -2f, 2f, 0f);
      angle = SpriteColorFXEditorHelper.SliderWithReset(@"UV angle", @"UV rotation angle", angle, 0f, 360f, 0f);
			
      EditorGUI.indentLevel--;
		}
	}
}
