///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Sprite Color FX.
// Copyright (c) Ibuprogames. All rights reserved.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// http://unity3d.com/support/documentation/Components/SL-Shader.html
Shader "Sprites/Sprite Color FX/Sprite Color Dissolve Border Texture VividLight"
{
  // http://unity3d.com/support/documentation/Components/SL-Properties.html
  Properties
  {
    [PerRendererData]
	_MainTex("Base (RGB)", 2D) = "white" {}

    _DissolveTex("Dissolve (RGB)", 2D) = "white" {}

    _BorderTex("Border (RGB)", 2D) = "white" {}

	_DissolveAmount("Dissolve amount", Range(0.0, 1.0)) = 0.0

    _DissolveLineWitdh("Dissolve line size", Range(0.0, 0.2)) = 0.1
    
	_DissolveUVScale("Dissolve UV scale", Range(0.1, 5.0)) = 1.0

	_BorderUVScale("Border UV scale", Range(0.1, 5.0)) = 1.0

    _Color("Tint", Color) = (1, 1, 1, 1)

	[MaterialToggle]
	PixelSnap("Pixel snap", Float) = 0

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
    Lighting On
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

    sampler2D _DissolveTex;
	sampler2D _BorderTex;
    float _DissolveAmount;
    float _DissolveLineWitdh;
    float _DissolveUVScale;
    float _DissolveInverseOne;
    float _DissolveInverseTwo;
    float _BorderUVScale;

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
      
      fixed4 pixelBorder = tex2D(_BorderTex, IN.uv_MainTex * _BorderUVScale) * IN.color;
  
      float4 dissolve = _DissolveInverseOne - tex2D(_DissolveTex, IN.uv_MainTex * _DissolveUVScale) * _DissolveInverseTwo;
  
      int isClear = int(dissolve.r + _DissolveAmount);

      float3 border = lerp(0.0, pixelBorder.rgb > 0.0 ? VividLight(pixel.rgb, pixelBorder.rgb) : pixel.rgb, isClear);
  
	  o.Albedo = lerp(border, pixel.rgb, int(dissolve.r + _DissolveAmount - _DissolveLineWitdh)) * pixel.a;
      o.Gloss = lerp(0.0, 1.0, isClear) * pixel.a;
      o.Alpha = o.Gloss * _Color.a;
      o.Specular = _Shininess;
	  o.Normal = UnpackNormal(tex2D(_NormalTex, IN.uv_MainTex));
    }
    ENDCG
  }

  Fallback "Sprites/Default"
}