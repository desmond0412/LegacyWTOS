///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Sprite Color FX.
// Copyright (c) Ibuprogames. All rights reserved.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;

using UnityEngine;

namespace SpriteColorFX
{
  /// <summary>
  /// Color shift linear.
  /// </summary>
  [ExecuteInEditMode]
  [RequireComponent(typeof(SpriteRenderer))]
  [AddComponentMenu("Sprite Color FX/Color Shift Linear")]
  public sealed class SpriteColorShiftLinear : SpriteColorBase
  {
    /// <summary>
    /// Red shift channel.
    /// </summary>
    public Vector2 redShift = Vector2.zero;

    /// <summary>
    /// Green shift channel.
    /// </summary>
    public Vector2 greenShift = Vector2.zero;

    /// <summary>
    /// Blue shift channel.
    /// </summary>
    public Vector2 blueShift = Vector2.zero;

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
        return string.Format("Shaders/Shift/{0}SpriteColorShiftLinear{1}",
          LightMode != SpriteColorFX.LightMode.UnLit ? LightMode.ToString() + "/" : string.Empty,
          LightMode == SpriteColorFX.LightMode.UnLit ? string.Empty : LightMode.ToString());
      }
    }

    protected override void UpdateShader()
    {
      spriteRenderer.sharedMaterial.SetFloat("_RedShiftX", redShift.x);
      spriteRenderer.sharedMaterial.SetFloat("_RedShiftY", redShift.y);

      spriteRenderer.sharedMaterial.SetFloat("_GreenShiftX", greenShift.x);
      spriteRenderer.sharedMaterial.SetFloat("_GreenShiftY", greenShift.y);

      spriteRenderer.sharedMaterial.SetFloat("_BlueShiftX", blueShift.x);
      spriteRenderer.sharedMaterial.SetFloat("_BlueShiftY", blueShift.y);

      spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderNoiseAmountParam, noiseAmount * 0.1f);
      spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderNoiseSpeedParam, noiseSpeed * 0.005f);
    }
  }
}
