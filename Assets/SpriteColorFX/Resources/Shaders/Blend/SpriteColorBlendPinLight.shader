// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Sprite Color FX.
// Copyright (c) Ibuprogames. All rights reserved.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// http://unity3d.com/support/documentation/Components/SL-Shader.html
Shader "Sprites/Sprite Color FX/Sprite Color Blend PinLight"
{
  // http://unity3d.com/support/documentation/Components/SL-Properties.html
  Properties
  {
    [PerRendererData]
	_MainTex("Base (RGB)", 2D) = "white" {}

    _Color("Tint", Color) = (1, 1, 1, 1)

	[MaterialToggle]
	PixelSnap("Pixel snap", Float) = 0
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

	// GrabPass (http://docs.unity3d.com/es/current/Manual/SL-GrabPass.html)
	GrabPass
	{
	}

    CGPROGRAM
    #include "../SpriteColorFXCG.cginc"

    #pragma surface surf Lambert alpha vertex:vert addshadow
    #pragma fragmentoption ARB_precision_hint_fastest
    #pragma multi_compile DUMMY PIXELSNAP_ON
    #pragma target 3.0

    sampler2D _MainTex;
    sampler2D _GrabTexture;
    fixed4 _Color;

    float _Strength;

    struct Input
    {
      float2 uv_MainTex;
      fixed4 color;
      float4 proj : TEXCOORD0;
    };

    void vert(inout appdata_full v, out Input o)
    {
#if defined(PIXELSNAP_ON) && !defined(SHADER_API_FLASH)
      v.vertex = UnityPixelSnap (v.vertex);
#endif
        
      UNITY_INITIALIZE_OUTPUT(Input, o);
      o.color = _Color * v.color;

      float4 oPos = UnityObjectToClipPos(v.vertex);
#if UNITY_UV_STARTS_AT_TOP
      float scale = -1.0;
#else
      float scale = 1.0;
#endif
      o.proj.xy = (float2(oPos.x, oPos.y * scale) + oPos.w) * 0.5;
      o.proj.zw = oPos.zw;
    }

    void surf(Input IN, inout SurfaceOutput o)
    {
      float4 pixel = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
      
	  float3 grabColor = tex2D(_GrabTexture, UNITY_PROJ_COORD(IN.proj).xy).rgb;

	  o.Albedo = lerp(pixel.rgb, PinLight(pixel.rgb, grabColor), _Strength) * pixel.a;
      o.Alpha = pixel.a;
    }
    ENDCG
  }

  Fallback "Sprites/Default"
}