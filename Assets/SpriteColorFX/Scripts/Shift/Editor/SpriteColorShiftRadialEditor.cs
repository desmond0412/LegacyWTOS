///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Sprite Color FX.
// Copyright (c) Ibuprogames. All rights reserved.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEditor;

namespace SpriteColorFX
{
  /// <summary>
  /// SpriteColorShiftSphere editor.
  /// </summary>
  [CustomEditor(typeof(SpriteColorShiftRadial))]
  public sealed class SpriteColorShiftRadialEditor : SpriteColorBaseEditor
  {
    private SpriteColorShiftRadial baseTarget;

    /// <summary>
    /// Set the default values.
    /// </summary>
    protected override void ResetDefaultValues()
    {
      if (baseTarget == null)
        baseTarget = this.target as SpriteColorShiftRadial;

      baseTarget.strength = 0.0f;
      
      baseTarget.noiseAmount = 0.0f;

      base.ResetDefaultValues();
    }

    /// <summary>
    /// Inspector.
    /// </summary>
    protected override void Inspector()
    {
      if (baseTarget == null)
        baseTarget = this.target as SpriteColorShiftRadial;

      EditorGUIUtility.fieldWidth = 40.0f;

      baseTarget.strength = SpriteColorFXEditorHelper.IntSliderWithReset(@"Strength", SpriteColorFXEditorHelper.TooltipStrength, Mathf.RoundToInt(baseTarget.strength * 100.0f), 0, 100, 0) * 0.01f;

      baseTarget.noiseAmount = SpriteColorFXEditorHelper.SliderWithReset(@"Noise amount", SpriteColorFXEditorHelper.TooltipNoiseAmount, baseTarget.noiseAmount * 100.0f, 0.0f, 100.0f, 0.0f) * 0.01f;

      baseTarget.noiseSpeed = SpriteColorFXEditorHelper.SliderWithReset(@"Noise speed", SpriteColorFXEditorHelper.TooltipNoiseSpeed, baseTarget.noiseSpeed * 100.0f, 0.0f, 100.0f, 0.0f) * 0.01f;
    }
  }
}