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
  /// SpriteColorBlend editor.
  /// </summary>
  [CustomEditor(typeof(SpriteColorBlend))]
  public sealed class SpriteColorBlendEditor : SpriteColorBaseEditor
	{
    private SpriteColorBlend baseTarget;

    /// <summary>
    /// Set the default values.
    /// </summary>
    protected override void ResetDefaultValues()
    {
      if (baseTarget == null)
        baseTarget = this.target as SpriteColorBlend;

      baseTarget.strength = 1.0f;

      baseTarget.SetPixelOp(SpriteColorHelper.PixelOp.Solid);

      base.ResetDefaultValues();
    }

    /// <summary>
    /// Inspector.
    /// </summary>
    protected override void Inspector()
    {
			if (baseTarget == null)
        baseTarget = base.target as SpriteColorBlend;

			EditorGUIUtility.fieldWidth = 40.0f;
			
      baseTarget.strength = (float)SpriteColorFXEditorHelper.IntSliderWithReset(@"Strength", SpriteColorFXEditorHelper.TooltipStrength, Mathf.RoundToInt(baseTarget.strength * 100.0f), 0, 100, 100) * 0.01f;

      SpriteColorHelper.PixelOp newPixelOp = (SpriteColorHelper.PixelOp)EditorGUILayout.EnumPopup(new GUIContent(@"Blend mode", @"Blend modes"), baseTarget.pixelOp);
      if (newPixelOp != baseTarget.pixelOp)
        baseTarget.SetPixelOp(newPixelOp);
		}
	}
}
