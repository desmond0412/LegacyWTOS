// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:6874,x:36099,y:32668,varname:node_6874,prsc:2|custl-2204-OUT,clip-336-OUT;n:type:ShaderForge.SFN_Tex2d,id:6980,x:32952,y:33192,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:node_6980,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_LightVector,id:5885,x:32599,y:32966,varname:node_5885,prsc:2;n:type:ShaderForge.SFN_LightColor,id:7434,x:33992,y:32720,varname:node_7434,prsc:2;n:type:ShaderForge.SFN_LightAttenuation,id:6041,x:33980,y:32927,varname:node_6041,prsc:2;n:type:ShaderForge.SFN_AmbientLight,id:9746,x:33330,y:33009,varname:node_9746,prsc:2;n:type:ShaderForge.SFN_Dot,id:4628,x:33129,y:32875,varname:node_4628,prsc:2,dt:0|A-7428-OUT,B-5885-OUT;n:type:ShaderForge.SFN_Multiply,id:2299,x:33470,y:32826,varname:node_2299,prsc:2|A-4628-OUT,B-6980-RGB;n:type:ShaderForge.SFN_Add,id:5953,x:33826,y:32830,varname:node_5953,prsc:2|A-2299-OUT,B-3057-OUT;n:type:ShaderForge.SFN_Multiply,id:3057,x:33580,y:33092,varname:node_3057,prsc:2|A-9746-RGB,B-6980-RGB;n:type:ShaderForge.SFN_Multiply,id:8902,x:34387,y:33213,varname:node_8902,prsc:2|A-6980-RGB,B-807-OUT;n:type:ShaderForge.SFN_Slider,id:807,x:33980,y:33152,ptovrint:False,ptlb:Unlit Value,ptin:_UnlitValue,varname:node_807,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5,max:1;n:type:ShaderForge.SFN_Subtract,id:7799,x:34387,y:33060,varname:node_7799,prsc:2|A-7529-OUT,B-807-OUT;n:type:ShaderForge.SFN_Vector1,id:7529,x:34137,y:33060,varname:node_7529,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:6235,x:34247,y:32790,varname:node_6235,prsc:2|A-7434-RGB,B-6041-OUT,C-5953-OUT;n:type:ShaderForge.SFN_NormalVector,id:7428,x:32599,y:32801,prsc:2,pt:False;n:type:ShaderForge.SFN_Multiply,id:5287,x:34473,y:32790,varname:node_5287,prsc:2|A-6235-OUT,B-7799-OUT;n:type:ShaderForge.SFN_Add,id:6103,x:34771,y:32788,varname:node_6103,prsc:2|A-5287-OUT,B-8902-OUT;n:type:ShaderForge.SFN_Tex2d,id:6136,x:34817,y:33083,ptovrint:False,ptlb:Dissolve Map,ptin:_DissolveMap,varname:node_6136,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:3920,x:35133,y:33099,varname:node_3920,prsc:2|A-6136-R,B-1000-OUT;n:type:ShaderForge.SFN_Slider,id:1000,x:34666,y:33392,ptovrint:False,ptlb:Dissolve Intensity,ptin:_DissolveIntensity,varname:node_1000,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Multiply,id:8598,x:35407,y:33132,varname:node_8598,prsc:2|A-3920-OUT,B-2250-OUT;n:type:ShaderForge.SFN_Vector1,id:2250,x:35133,y:33221,varname:node_2250,prsc:2,v1:5;n:type:ShaderForge.SFN_Add,id:9455,x:35661,y:33180,varname:node_9455,prsc:2|A-8598-OUT,B-1000-OUT;n:type:ShaderForge.SFN_Power,id:946,x:35917,y:33177,varname:node_946,prsc:2|VAL-9455-OUT,EXP-9440-OUT;n:type:ShaderForge.SFN_Vector1,id:9440,x:35661,y:33330,varname:node_9440,prsc:2,v1:100;n:type:ShaderForge.SFN_ConstantClamp,id:336,x:35816,y:33003,varname:node_336,prsc:2,min:0,max:1|IN-946-OUT;n:type:ShaderForge.SFN_OneMinus,id:9167,x:35057,y:33415,varname:node_9167,prsc:2|IN-1000-OUT;n:type:ShaderForge.SFN_Color,id:1682,x:34844,y:32430,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_1682,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:2204,x:35371,y:32564,varname:node_2204,prsc:2|A-3204-OUT,B-6103-OUT;n:type:ShaderForge.SFN_Slider,id:4501,x:34706,y:32636,ptovrint:False,ptlb:Color Strength,ptin:_ColorStrength,varname:node_4501,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Multiply,id:3204,x:35076,y:32440,varname:node_3204,prsc:2|A-1682-RGB,B-4501-OUT;proporder:807-6980-1682-4501-6136-1000;pass:END;sub:END;*/

Shader "Custom/DissolveBasicShader" {
    Properties {
        _UnlitValue ("Unlit Value", Range(0, 1)) = 0.5
        _Diffuse ("Diffuse", 2D) = "white" {}
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        _ColorStrength ("Color Strength", Range(0, 1)) = 0
        _DissolveMap ("Dissolve Map", 2D) = "white" {}
        _DissolveIntensity ("Dissolve Intensity", Range(0, 1)) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float _UnlitValue;
            uniform sampler2D _DissolveMap; uniform float4 _DissolveMap_ST;
            uniform float _DissolveIntensity;
            uniform float4 _Color;
            uniform float _ColorStrength;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float4 _DissolveMap_var = tex2D(_DissolveMap,TRANSFORM_TEX(i.uv0, _DissolveMap));
                clip(clamp(pow((((_DissolveMap_var.r*_DissolveIntensity)*5.0)+_DissolveIntensity),100.0),0,1) - 0.5);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float3 finalColor = ((_Color.rgb*_ColorStrength)*(((_LightColor0.rgb*attenuation*((dot(i.normalDir,lightDirection)*_Diffuse_var.rgb)+(UNITY_LIGHTMODEL_AMBIENT.rgb*_Diffuse_var.rgb)))*(1.0-_UnlitValue))+(_Diffuse_var.rgb*_UnlitValue)));
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
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float _UnlitValue;
            uniform sampler2D _DissolveMap; uniform float4 _DissolveMap_ST;
            uniform float _DissolveIntensity;
            uniform float4 _Color;
            uniform float _ColorStrength;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float4 _DissolveMap_var = tex2D(_DissolveMap,TRANSFORM_TEX(i.uv0, _DissolveMap));
                clip(clamp(pow((((_DissolveMap_var.r*_DissolveIntensity)*5.0)+_DissolveIntensity),100.0),0,1) - 0.5);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float3 finalColor = ((_Color.rgb*_ColorStrength)*(((_LightColor0.rgb*attenuation*((dot(i.normalDir,lightDirection)*_Diffuse_var.rgb)+(UNITY_LIGHTMODEL_AMBIENT.rgb*_Diffuse_var.rgb)))*(1.0-_UnlitValue))+(_Diffuse_var.rgb*_UnlitValue)));
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
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _DissolveMap; uniform float4 _DissolveMap_ST;
            uniform float _DissolveIntensity;
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
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 _DissolveMap_var = tex2D(_DissolveMap,TRANSFORM_TEX(i.uv0, _DissolveMap));
                clip(clamp(pow((((_DissolveMap_var.r*_DissolveIntensity)*5.0)+_DissolveIntensity),100.0),0,1) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
