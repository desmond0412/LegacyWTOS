///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Sprite Color FX.
// Copyright (c) Ibuprogames. All rights reserved.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEditor;

namespace SpriteColorFX
{
  /// <summary>
  /// SpriteColorShiftLinear editor.
  /// </summary>
  [CustomEditor(typeof(SpriteColorShiftLinear))]
  public sealed class SpriteColorShiftLinearEditor : SpriteColorBaseEditor
  {
    private SpriteColorShiftLinear baseTarget;

    /// <summary>
    /// Set the default values.
    /// </summary>
    protected override void ResetDefaultValues()
    {
      if (baseTarget == null)
        baseTarget = this.target as SpriteColorShiftLinear;

      baseTarget.redShift = Vector2.zero;

      baseTarget.greenShift = Vector2.zero;

      baseTarget.blueShift = Vector2.zero;

      baseTarget.noiseAmount = 0.0f;

      baseTarget.noiseSpeed = 1.0f;

      base.ResetDefaultValues();
    }

    /// <summary>
    /// Inspector.
    /// </summary>
    protected override void Inspector()
    {
      if (baseTarget == null)
        baseTarget = this.target as SpriteColorShiftLinear;

      EditorGUIUtility.fieldWidth = 40.0f;

      baseTarget.redShift = SpriteColorFXEditorHelper.Vector2WithReset(@"Red offset", SpriteColorFXEditorHelper.TooltipRedOffset, baseTarget.redShift, Vector2.zero);

      baseTarget.greenShift = SpriteColorFXEditorHelper.Vector2WithReset(@"Green offset", SpriteColorFXEditorHelper.TooltipGreenOffset, baseTarget.greenShift, Vector2.zero);

      baseTarget.blueShift = SpriteColorFXEditorHelper.Vector2WithReset(@"Blue offset", SpriteColorFXEditorHelper.TooltipBlueOffset, baseTarget.blueShift, Vector2.zero);

      baseTarget.noiseAmount = SpriteColorFXEditorHelper.SliderWithReset(@"Noise amount", SpriteColorFXEditorHelper.TooltipNoiseAmount, baseTarget.noiseAmount * 100.0f, 0.0f, 100.0f, 0.0f) * 0.01f;

      baseTarget.noiseSpeed = SpriteColorFXEditorHelper.SliderWithReset(@"Noise speed", SpriteColorFXEditorHelper.TooltipNoiseSpeed, baseTarget.noiseSpeed * 100.0f, 0.0f, 100.0f, 0.0f) * 0.01f;
    }
  }
}