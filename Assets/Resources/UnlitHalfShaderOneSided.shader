// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:9361,x:34458,y:32296,varname:node_9361,prsc:2|normal-7201-RGB,emission-1789-OUT,custl-2831-OUT,clip-3775-OUT;n:type:ShaderForge.SFN_LightVector,id:6869,x:31858,y:32654,varname:node_6869,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:9684,x:31858,y:32782,prsc:2,pt:False;n:type:ShaderForge.SFN_Dot,id:7782,x:32184,y:32739,varname:node_7782,prsc:2,dt:1|A-6869-OUT,B-9684-OUT;n:type:ShaderForge.SFN_Tex2d,id:851,x:31936,y:33014,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,cmnt:Texture,varname:node_851,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Multiply,id:5085,x:32887,y:32618,cmnt:Blinn Phong,varname:node_5085,prsc:2|A-8695-OUT,B-3434-RGB,C-7359-OUT;n:type:ShaderForge.SFN_Add,id:7359,x:32651,y:32775,varname:node_7359,prsc:2|A-6912-OUT,B-4120-OUT;n:type:ShaderForge.SFN_LightColor,id:3434,x:32651,y:32635,varname:node_3434,prsc:2;n:type:ShaderForge.SFN_AmbientLight,id:1467,x:32356,y:32945,varname:node_1467,prsc:2;n:type:ShaderForge.SFN_Multiply,id:6912,x:32411,y:32739,varname:node_6912,prsc:2|A-7782-OUT,B-851-RGB;n:type:ShaderForge.SFN_LightAttenuation,id:8695,x:32651,y:32498,varname:node_8695,prsc:2;n:type:ShaderForge.SFN_Multiply,id:6012,x:32651,y:33102,varname:node_6012,prsc:2|A-851-RGB,B-9477-OUT;n:type:ShaderForge.SFN_Add,id:7195,x:33365,y:32751,varname:node_7195,prsc:2|A-6493-OUT,B-6012-OUT;n:type:ShaderForge.SFN_Multiply,id:6493,x:33127,y:32631,varname:node_6493,prsc:2|A-5085-OUT,B-9616-OUT;n:type:ShaderForge.SFN_Subtract,id:9616,x:32651,y:33279,varname:node_9616,prsc:2|A-1359-OUT,B-9477-OUT;n:type:ShaderForge.SFN_Vector1,id:1359,x:32434,y:33358,varname:node_1359,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:4120,x:32651,y:32945,varname:node_4120,prsc:2|A-1467-RGB,B-851-RGB;n:type:ShaderForge.SFN_Slider,id:9477,x:32302,y:33258,ptovrint:False,ptlb:Unlit Value,ptin:_UnlitValue,varname:node_9477,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5,max:1;n:type:ShaderForge.SFN_Tex2d,id:7201,x:33408,y:32303,ptovrint:False,ptlb:Normal,ptin:_Normal,varname:node_7201,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Tex2d,id:5152,x:33137,y:32305,ptovrint:False,ptlb:Emissive Map,ptin:_EmissiveMap,varname:node_5152,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_ValueProperty,id:9423,x:33153,y:32515,ptovrint:False,ptlb:Emissive Lvl,ptin:_EmissiveLvl,varname:node_9423,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:1789,x:33408,y:32515,varname:node_1789,prsc:2|A-5152-RGB,B-9423-OUT;n:type:ShaderForge.SFN_Tex2d,id:1995,x:33365,y:32930,ptovrint:False,ptlb:Opacity Clip,ptin:_OpacityClip,varname:node_1995,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:3775,x:33567,y:33042,varname:node_3775,prsc:2|A-1995-A,B-9832-OUT;n:type:ShaderForge.SFN_Slider,id:9832,x:33208,y:33136,ptovrint:False,ptlb:Clip Lvl,ptin:_ClipLvl,varname:node_9832,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0.5,cur:1,max:1;n:type:ShaderForge.SFN_Slider,id:4288,x:33473,y:32157,ptovrint:False,ptlb:Color Value,ptin:_ColorValue,varname:node_4288,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Color,id:6446,x:33581,y:31992,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_6446,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Lerp,id:2831,x:34033,y:32119,varname:node_2831,prsc:2|A-7195-OUT,B-6446-RGB,T-4288-OUT;proporder:851-6446-4288-9477-7201-5152-9423-1995-9832;pass:END;sub:END;*/

Shader "Custom/UnlitHalfShaderOneSided" {
    Properties {
        _Diffuse ("Diffuse", 2D) = "black" {}
        _Color ("Color", Color) = (0,0,0,1)
        _ColorValue ("Color Value", Range(0, 1)) = 0
        _UnlitValue ("Unlit Value", Range(0, 1)) = 0.5
        _Normal ("Normal", 2D) = "bump" {}
        _EmissiveMap ("Emissive Map", 2D) = "white" {}
        _EmissiveLvl ("Emissive Lvl", Float ) = 0
        _OpacityClip ("Opacity Clip", 2D) = "white" {}
        _ClipLvl ("Clip Lvl", Range(0.5, 1)) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float _UnlitValue;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform sampler2D _EmissiveMap; uniform float4 _EmissiveMap_ST;
            uniform float _EmissiveLvl;
            uniform sampler2D _OpacityClip; uniform float4 _OpacityClip_ST;
            uniform float _ClipLvl;
            uniform float _ColorValue;
            uniform float4 _Color;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 _Normal_var = UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(i.uv0, _Normal)));
                float3 normalLocal = _Normal_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float4 _OpacityClip_var = tex2D(_OpacityClip,TRANSFORM_TEX(i.uv0, _OpacityClip));
                clip((_OpacityClip_var.a*_ClipLvl) - 0.5);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
////// Emissive:
                float4 _EmissiveMap_var = tex2D(_EmissiveMap,TRANSFORM_TEX(i.uv0, _EmissiveMap));
                float3 emissive = (_EmissiveMap_var.rgb*_EmissiveLvl);
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse)); // Texture
                float3 finalColor = emissive + lerp((((attenuation*_LightColor0.rgb*((max(0,dot(lightDirection,i.normalDir))*_Diffuse_var.rgb)+(UNITY_LIGHTMODEL_AMBIENT.rgb*_Diffuse_var.rgb)))*(1.0-_UnlitValue))+(_Diffuse_var.rgb*_UnlitValue)),_Color.rgb,_ColorValue);
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float _UnlitValue;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform sampler2D _EmissiveMap; uniform float4 _EmissiveMap_ST;
            uniform float _EmissiveLvl;
            uniform sampler2D _OpacityClip; uniform float4 _OpacityClip_ST;
            uniform float _ClipLvl;
            uniform float _ColorValue;
            uniform float4 _Color;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 _Normal_var = UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(i.uv0, _Normal)));
                float3 normalLocal = _Normal_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float4 _OpacityClip_var = tex2D(_OpacityClip,TRANSFORM_TEX(i.uv0, _OpacityClip));
                clip((_OpacityClip_var.a*_ClipLvl) - 0.5);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse)); // Texture
                float3 finalColor = lerp((((attenuation*_LightColor0.rgb*((max(0,dot(lightDirection,i.normalDir))*_Diffuse_var.rgb)+(UNITY_LIGHTMODEL_AMBIENT.rgb*_Diffuse_var.rgb)))*(1.0-_UnlitValue))+(_Diffuse_var.rgb*_UnlitValue)),_Color.rgb,_ColorValue);
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma exclude_renderers d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _OpacityClip; uniform float4 _OpacityClip_ST;
            uniform float _ClipLvl;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 _OpacityClip_var = tex2D(_OpacityClip,TRANSFORM_TEX(i.uv0, _OpacityClip));
                clip((_OpacityClip_var.a*_ClipLvl) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
