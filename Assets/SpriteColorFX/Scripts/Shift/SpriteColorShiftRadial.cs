///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Sprite Color FX.
// Copyright (c) Ibuprogames. All rights reserved.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;

using UnityEngine;

namespace SpriteColorFX
{
  /// <summary>
  /// Color shift radial.
  /// </summary>
  [ExecuteInEditMode]
  [RequireComponent(typeof(SpriteRenderer))]
  [AddComponentMenu("Sprite Color FX/Color Shift Radial")]
  public sealed class SpriteColorShiftRadial : SpriteColorBase
  {
    /// <summary>
    /// Effect strength [0..1].
    /// </summary>
    public float strength = 0.0f;

    /// <summary>
    /// Noise amount [0..1].
    /// </summary>
    public float noiseAmount = 0.0f;

    /// <summary>
    /// Noise speed [0..1].
    /// </summary>
    public float noiseSpeed = 0.0f;

    /// <summary>
    /// Shader path.
    /// </summary>
    protected override string ShaderPath
    {
      get
      {
        return string.Format("Shaders/Shift/{0}SpriteColorShiftRadial{1}",
          LightMode != SpriteColorFX.LightMode.UnLit ? LightMode.ToString() + "/" : string.Empty,
          LightMode == SpriteColorFX.LightMode.UnLit ? string.Empty : LightMode.ToString());
      }
    }

    protected override void UpdateShader()
    {
      spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderStrengthParam, strength * 0.1f);

      spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderNoiseAmountParam, noiseAmount * 0.1f);
      spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderNoiseSpeedParam, noiseSpeed * 0.005f);
    }
  }
}
