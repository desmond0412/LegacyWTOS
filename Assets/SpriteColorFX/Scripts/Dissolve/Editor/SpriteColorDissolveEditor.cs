///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Sprite Color FX.
// Copyright (c) Ibuprogames. All rights reserved.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEditor;

namespace SpriteColorFX
{
  /// <summary>
  /// SpriteColorDissolve editor.
  /// </summary>
  [CustomEditor(typeof(SpriteColorDissolve))]
  public sealed class SpriteColorDissolveEditor : SpriteColorBaseEditor
  {
    private SpriteColorDissolve baseTarget;

    /// <summary>
    /// Set the default values.
    /// </summary>
    protected override void ResetDefaultValues()
    {
      if (baseTarget == null)
        baseTarget = this.target as SpriteColorDissolve;

      baseTarget.dissolveAmount = 0.0f;
      
      baseTarget.dissolveBorderWitdh = 0.1f;
      
      baseTarget.dissolveBorderColor = Color.grey;
      
      baseTarget.dissolveUVScale = 1.0f;
      
      baseTarget.borderUVScale = 1.0f;

      base.ResetDefaultValues();
    }

    /// <summary>
    /// Inspector.
    /// </summary>
    protected override void Inspector()
    {
      if (baseTarget == null)
        baseTarget = this.target as SpriteColorDissolve;

      EditorGUIUtility.fieldWidth = 40.0f;

      baseTarget.dissolveAmount = SpriteColorFXEditorHelper.SliderWithReset(@"Amount", SpriteColorFXEditorHelper.TooltipNoiseAmount, baseTarget.dissolveAmount * 100.0f, 0.0f, 100.0f, 0.0f) * 0.01f;

      DissolveShaderType newShaderType = (DissolveShaderType)EditorGUILayout.EnumPopup(new GUIContent(@"Shader", @"Texture type"), baseTarget.shaderType);
      if (newShaderType != baseTarget.shaderType)
        baseTarget.SetShaderType(newShaderType);

      if (baseTarget.shaderType != DissolveShaderType.Normal)
      {
        SpriteColorHelper.PixelOp newPixelOp = (SpriteColorHelper.PixelOp)EditorGUILayout.EnumPopup(new GUIContent(@"Blend mode", @"Blend modes"), baseTarget.pixelOp);
        if (newPixelOp != baseTarget.pixelOp)
          baseTarget.SetPixelOp(newPixelOp);
      }

      DisolveTextureType newTextureType = (DisolveTextureType)EditorGUILayout.EnumPopup(@"Dissolve type", baseTarget.disolveTextureType);
      if (newTextureType != baseTarget.disolveTextureType)
        baseTarget.SetTextureType(newTextureType);

      if (baseTarget.disolveTextureType == DisolveTextureType.Custom)
        baseTarget.disolveTexture = EditorGUILayout.ObjectField(@"Dissolve texture", baseTarget.disolveTexture, typeof(Texture), false) as Texture;

      baseTarget.dissolveUVScale = SpriteColorFXEditorHelper.SliderWithReset(@"Dissolve UV scale", SpriteColorFXEditorHelper.TooltipNoiseAmount, baseTarget.dissolveUVScale, 0.1f, 5.0f, 1.0f);

      baseTarget.dissolveInverse = EditorGUILayout.Toggle(new GUIContent(@"Invert", SpriteColorFXEditorHelper.TooltipNoiseAmount), baseTarget.dissolveInverse);

      if (baseTarget.shaderType != DissolveShaderType.Normal)
        baseTarget.dissolveBorderWitdh = SpriteColorFXEditorHelper.SliderWithReset(@"Border witdh", SpriteColorFXEditorHelper.TooltipNoiseAmount, baseTarget.dissolveBorderWitdh * 500.0f, 0.0f, 100.0f, 50.0f) * 0.002f;

      if (baseTarget.shaderType == DissolveShaderType.BorderColor)
        baseTarget.dissolveBorderColor = SpriteColorFXEditorHelper.ColorWithReset(@"Border color", SpriteColorFXEditorHelper.TooltipNoiseAmount, baseTarget.dissolveBorderColor, Color.grey);
      else if (baseTarget.shaderType == DissolveShaderType.BorderTexture)
      {
        baseTarget.borderTexture = EditorGUILayout.ObjectField(@"Border texture", baseTarget.borderTexture, typeof(Texture), false) as Texture;
        baseTarget.borderUVScale = SpriteColorFXEditorHelper.SliderWithReset(@"Border UV scale", SpriteColorFXEditorHelper.TooltipNoiseAmount, baseTarget.borderUVScale, 0.1f, 5.0f, 1.0f);
      }
    }
  }
}