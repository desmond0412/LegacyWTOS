///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Sprite Color FX.
// Copyright (c) Ibuprogames. All rights reserved.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// http://unity3d.com/support/documentation/Components/SL-Shader.html
Shader "Sprites/Sprite Color FX/Sprite Color Shift Radial Bump Lit"
{
  // http://unity3d.com/support/documentation/Components/SL-Properties.html
  Properties
  {
    [PerRendererData]
	_MainTex("Base (RGB)", 2D) = "white" {}

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

    // Define this add noise effect.
    #define USE_NOISE

    float _Strength = 0.0;
    float _NoiseAmount = 0.0;
    float _NoiseSpeed = 0.0;

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
      float shift = _Strength;
#ifdef USE_NOISE
      shift += _NoiseAmount * Rand1(_SinTime.w * _NoiseSpeed);
#endif
      shift *= distance(IN.uv_MainTex, 0.5);

      float2 colorVec = normalize(IN.uv_MainTex - 0.5);

      float2 uvR = float2(IN.uv_MainTex.x - (colorVec.x * shift), IN.uv_MainTex.y - (colorVec.y * shift));
      float2 uvG = float2(IN.uv_MainTex.x, IN.uv_MainTex.y);
      float2 uvB = float2(IN.uv_MainTex.x + (colorVec.x * shift), IN.uv_MainTex.y + (colorVec.y * shift));

      float2 red = tex2D(_MainTex, uvR).ra * IN.color.r;
      float2 green = tex2D(_MainTex, uvG).ga * IN.color.g;
      float2 blue = tex2D(_MainTex, uvB).ba * IN.color.b;
 
      float4 final = float4(red.x * red.y, green.x * green.y, blue.x * blue.y, (red.y + green.y + blue.y) * 0.333);

	  o.Albedo = final.rgb * final.a;
      o.Gloss = final.a;
      o.Alpha = final.a * _Color.a;
      o.Specular = _Shininess;
	  o.Normal = UnpackNormal(tex2D(_NormalTex, IN.uv_MainTex));
    }
    ENDCG
  }

  Fallback "Sprites/Default"
}