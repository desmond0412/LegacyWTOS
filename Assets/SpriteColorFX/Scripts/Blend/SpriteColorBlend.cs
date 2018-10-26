///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Sprite Color FX.
// Copyright (c) Ibuprogames. All rights reserved.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;

using UnityEngine;

namespace SpriteColorFX
{
  /// <summary>
  /// Sprite Color Blend.
  /// </summary>
  [ExecuteInEditMode]
  [RequireComponent(typeof(SpriteRenderer))]
	[AddComponentMenu("Sprite Color FX/Color Blend")]
  public sealed class SpriteColorBlend : SpriteColorBase
	{
    /// <summary>
    /// Strength [0..1].
    /// </summary>
    public float strength = 1.0f;

    /// <summary>
    /// Use SetPixelOp().
    /// </summary>
    public SpriteColorHelper.PixelOp pixelOp = SpriteColorHelper.PixelOp.Solid;

    /// <summary>
    /// Shader path.
    /// </summary>
    protected override string ShaderPath
    {
      get
      {
        return string.Format("Shaders/Blend/{0}SpriteColorBlend{1}{2}",
          LightMode != SpriteColorFX.LightMode.UnLit ? LightMode.ToString() + "/" : string.Empty,
          pixelOp.ToString(),
          LightMode == SpriteColorFX.LightMode.UnLit ? string.Empty : LightMode.ToString());
      }
    }

    /// <summary>
    /// Set the pixel color operation.
    /// </summary>
    public void SetPixelOp(SpriteColorHelper.PixelOp pixelOp)
    {
      this.pixelOp = pixelOp;

      if (this.spriteRenderer != null)
        CreateMaterial();
    }

    protected override void UpdateShader()
    {
			spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderStrengthParam, strength);
		}
	}
}
