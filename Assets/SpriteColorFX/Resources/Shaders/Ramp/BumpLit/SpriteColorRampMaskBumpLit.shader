///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Sprite Color FX.
// Copyright (c) Ibuprogames. All rights reserved.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// http://unity3d.com/support/documentation/Components/SL-Shader.html
Shader "Sprites/Sprite Color FX/Sprite Color Ramp Mask Bump Lit"
{
  // http://unity3d.com/support/documentation/Components/SL-Properties.html
  Properties
  {
    [PerRendererData]
	_MainTex("Base (RGB)", 2D) = "white" {}

	_MaskTex("Mask (RGBA)", 2D) = "black" {}

    _Color("Tint", Color) = (1, 1, 1, 1)

	[MaterialToggle]
	PixelSnap("Pixel snap", Float) = 0

    // Color ramps 'Resources/Textures/SpriteColorRamps.png'.
    _RampsTex("Ramps (RGB)", 2D) = "" {}

	[PerRendererData]
    _NormalTex("Bump (RGB)", 2D) = "bump" {}

	_SpecColor("Specular Color", Color) = (0.5, 0.5, 0.5, 1)

	_Shininess("Shininess", Range(0.03, 1)) = 0.078125
  }

  // Techniques (http://unity3d.com/support/documentation/Components/SL-SubShader.html).
  SubShader
  {
    // Tags (http://docs.unity3d.com/Manual/SL-CullAndDepth.html).
    Tags
    { 
      "Queue" = "Transparent" 
      "IgnoreProjector" = "True" 
      "RenderType" = "Transparent" 
      "PreviewType" = "Plane"
      "CanUseSpriteAtlas" = "True"
    }

    Cull Off
    Lighting Off
    ZWrite Off
    Fog { Mode Off }
	
    CGPROGRAM
    #include "../../SpriteColorFXCG.cginc"

    #pragma surface surf BlinnPhong alpha vertex:vert addshadow
    #pragma fragmentoption ARB_precision_hint_fastest
    #pragma multi_compile DUMMY PIXELSNAP_ON
    #pragma target 3.0
     
    sampler2D _MainTex;
    sampler2D _NormalTex;
    fixed4 _Color;
	half _Shininess;

    float _Strength = 1.0f;
    float _RampRedIdx = 0.0f;
    float _RampGreenIdx = 0.0f;
    float _RampBlueIdx = 0.0f;
    float _GammaCorrect = 1.2f;
    float _UVScroll = 0.0f;
    float _InvertLum = 1.0f;
    float _LumRangeMin = 0.0f;
    float _LumRangeMax = 1.0f;
    
    sampler2D _RampsTex;
    sampler2D _MaskTex;

    struct Input
    {
      float2 uv_MainTex;
      fixed4 color;
    };
       
    void vert(inout appdata_full v, out Input o)
    {
#if defined(PIXELSNAP_ON) && !defined(SHADER_API_FLASH)
      v.vertex = UnityPixelSnap (v.vertex);
#endif
        
      UNITY_INITIALIZE_OUTPUT(Input, o);
      o.color = _Color * v.color;
    }
     
    void surf(Input IN, inout SurfaceOutput o)
    {
      float4 pixel = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
      
      // Masks (R = Ramp Red, G = Ramp Green, B = Ramp Blue, A = Intensity).
      float4 mask = tex2D(_MaskTex, IN.uv_MainTex);

      // Gamma correct.
      pixel = pow(pixel, _GammaCorrect);
      
      // Luminance.
      float lum = Luminance601(pixel.rgb) * _InvertLum;
      
      pixel = pow(pixel, 1.0f / _GammaCorrect);
      
      // Luminance range.
      lum = (1.0f / (_LumRangeMax - _LumRangeMin)) * (lum - _LumRangeMin);
      
      // Ramp.
      float3 rampRed = tex2D(_RampsTex, float2(lum + _UVScroll, _RampRedIdx)).rgb * mask.r;
      float3 rampGreen = tex2D(_RampsTex, float2(lum + _UVScroll, _RampGreenIdx)).rgb * mask.g;
      float3 rampBlue = tex2D(_RampsTex, float2(lum + _UVScroll, _RampBlueIdx)).rgb * mask.b;

      float3 final = lerp(pixel.rgb, rampRed + rampGreen + rampBlue, mask.a);

	  o.Albedo = lerp(pixel.rgb, final.rgb, _Strength) * pixel.a;
      o.Gloss = pixel.a;
      o.Alpha = pixel.a * _Color.a;
      o.Specular = _Shininess;
	  o.Normal = UnpackNormal(tex2D(_NormalTex, IN.uv_MainTex));
    }
    ENDCG
  }

  Fallback "Sprites/Default"
}
