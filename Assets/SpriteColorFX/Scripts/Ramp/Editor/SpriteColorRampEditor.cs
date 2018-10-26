///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Sprite Color FX.
// Copyright (c) Ibuprogames. All rights reserved.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEditor;

namespace SpriteColorFX
{
  /// <summary>
  /// SpriteColorRamp editor.
  /// </summary>
  [CustomEditor(typeof(SpriteColorRamp))]
  public sealed class SpriteColorRampEditor : SpriteColorBaseEditor
  {
    private SpriteColorRamp baseTarget;

    /// <summary>
    /// Set the default values.
    /// </summary>
    protected override void ResetDefaultValues()
    {
      if (baseTarget == null)
        baseTarget = this.target as SpriteColorRamp;

      baseTarget.strength = 1.0f;

      baseTarget.gammaCorrect = 1.2f;

      baseTarget.uvScroll = 0.0f;

      baseTarget.invertLum = false;

      baseTarget.luminanceRangeMin = 0.0f;

      baseTarget.luminanceRangeMax = 1.0f;

      base.ResetDefaultValues();
    }

    /// <summary>
    /// Inspector.
    /// </summary>
    protected override void Inspector()
    {
      if (baseTarget == null)
        baseTarget = this.target as SpriteColorRamp;

      EditorGUIUtility.fieldWidth = 40.0f;

      baseTarget.palette = (SpriteColorRampPalettes)EditorGUILayout.EnumPopup(@"Palette", baseTarget.palette);

      baseTarget.strength = SpriteColorFXEditorHelper.IntSliderWithReset(@"Strength", SpriteColorFXEditorHelper.TooltipStrength, Mathf.RoundToInt(baseTarget.strength * 100.0f), 0, 100, 100) * 0.01f;

      baseTarget.gammaCorrect = SpriteColorFXEditorHelper.SliderWithReset(@"Gamma", SpriteColorFXEditorHelper.TooltipGamma, baseTarget.gammaCorrect, 0.5f, 3.0f, 1.2f);

      baseTarget.uvScroll = SpriteColorFXEditorHelper.SliderWithReset(@"UV Scroll", SpriteColorFXEditorHelper.TooltipUVScroll, baseTarget.uvScroll, 0.0f, 1.0f, 0.0f);

      EditorGUIUtility.fieldWidth = 60.0f;

      SpriteColorFXEditorHelper.MinMaxSliderWithReset(@"Luminance range", SpriteColorFXEditorHelper.TooltipLuminanceRange, ref baseTarget.luminanceRangeMin, ref baseTarget.luminanceRangeMax, 0.0f, 1.0f, 0.0f, 1.0f);

      baseTarget.invertLum = SpriteColorFXEditorHelper.ToogleWithReset(@"Invert luminance", SpriteColorFXEditorHelper.TooltipInvertLuminance, baseTarget.invertLum, false);
    }
  }
}