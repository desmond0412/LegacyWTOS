///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Sprite Color FX.
// Copyright (c) Ibuprogames. All rights reserved.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;

using UnityEngine;

namespace SpriteColorFX
{
  /// <summary>
  /// Color ramp preset.
  /// </summary>
  [Serializable]
  public sealed class SpriteColorRampPreset : ScriptableObject
  {
    [Serializable]
    public class ColorPreset
    {
      [SerializeField]
      public string name = "Unnamed";
      
      [SerializeField]
      public Color[] colors = new Color[5];
    }

    [SerializeField]
    public int version = 0;

    [SerializeField]
    public bool interpolateColors = true;

    [SerializeField]
    public bool sortColorsByLuminance = false;

    [SerializeField]
    public List<ColorPreset> presets = new List<ColorPreset>();
  }
}