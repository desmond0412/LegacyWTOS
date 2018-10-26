///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Sprite Color FX.
// Copyright (c) Ibuprogames. All rights reserved.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEditor;

namespace SpriteColorFX
{
  /// <summary>
  /// SpriteColorRampMask editor.
  /// </summary>
  [CustomEditor(typeof(SpriteColorRampMask))]
  public sealed class SpriteColorRampMaskEditor : SpriteColorBaseEditor
  {
    private SpriteColorRampMask baseTarget;

    /// <summary>
    /// Set the default values.
    /// </summary>
    protected override void ResetDefaultValues()
    {
      if (baseTarget == null)
        baseTarget = this.target as SpriteColorRampMask;

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
        baseTarget = this.target as SpriteColorRampMask;

      EditorGUIUtility.fieldWidth = 40.0f;

      baseTarget.palettes[0] = (SpriteColorRampPalettes)EditorGUILayout.EnumPopup(@"Palette 1 (Red)", baseTarget.palettes[0]);

      baseTarget.palettes[1] = (SpriteColorRampPalettes)EditorGUILayout.EnumPopup(@"Palette 2 (Green)", baseTarget.palettes[1]);

      baseTarget.palettes[2] = (SpriteColorRampPalettes)EditorGUILayout.EnumPopup(@"Palette 3 (Blue)", baseTarget.palettes[2]);

      baseTarget.strength = SpriteColorFXEditorHelper.IntSliderWithReset(@"Strength", SpriteColorFXEditorHelper.TooltipStrength, Mathf.RoundToInt(baseTarget.strength * 100.0f), 0, 100, 100) * 0.01f;

      baseTarget.gammaCorrect = SpriteColorFXEditorHelper.SliderWithReset(@"Gamma", SpriteColorFXEditorHelper.TooltipGamma, baseTarget.gammaCorrect, 0.5f, 3.0f, 1.2f);

      baseTarget.uvScroll = SpriteColorFXEditorHelper.SliderWithReset(@"UV Scroll", SpriteColorFXEditorHelper.TooltipUVScroll, baseTarget.uvScroll, 0.0f, 1.0f, 0.0f);

      EditorGUIUtility.fieldWidth = 60.0f;

      SpriteColorFXEditorHelper.MinMaxSliderWithReset(@"Luminance range", SpriteColorFXEditorHelper.TooltipLuminanceRange, ref baseTarget.luminanceRangeMin, ref baseTarget.luminanceRangeMax, 0.0f, 1.0f, 0.0f, 1.0f);

      baseTarget.invertLum = SpriteColorFXEditorHelper.ToogleWithReset(@"Invert luminance", SpriteColorFXEditorHelper.TooltipInvertLuminance, baseTarget.invertLum, false);

      baseTarget.textureMask = EditorGUILayout.ObjectField(new GUIContent(@"Mask (RGBA)", SpriteColorFXEditorHelper.TooltipTextureMask), baseTarget.textureMask, typeof(Texture2D), false) as Texture2D;
    }
  }
}