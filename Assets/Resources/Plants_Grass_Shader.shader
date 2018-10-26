// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:False,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:1058,x:33774,y:32800,varname:node_1058,prsc:2|spec-6440-OUT,gloss-8639-OUT,emission-5039-OUT,transm-6353-OUT,lwrap-9864-OUT,custl-9665-OUT,clip-264-OUT,voffset-358-OUT;n:type:ShaderForge.SFN_Tex2d,id:8269,x:32065,y:32504,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:node_8269,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Vector1,id:6440,x:33217,y:32624,varname:node_6440,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:8639,x:33217,y:32722,varname:node_8639,prsc:2,v1:0;n:type:ShaderForge.SFN_Tex2d,id:363,x:31852,y:32748,ptovrint:False,ptlb:Emissive,ptin:_Emissive,varname:node_363,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:6056,x:31852,y:32949,ptovrint:False,ptlb:Emissive Color,ptin:_EmissiveColor,varname:node_6056,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:4346,x:31875,y:33191,ptovrint:False,ptlb:Emissive value,ptin:_Emissivevalue,varname:node_4346,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:5039,x:32213,y:32894,varname:node_5039,prsc:2|A-363-RGB,B-6056-RGB,C-4346-OUT;n:type:ShaderForge.SFN_Vector3,id:6353,x:33217,y:32941,varname:node_6353,prsc:2,v1:0.28,v2:0.31,v3:0.15;n:type:ShaderForge.SFN_Vector3,id:9864,x:33217,y:33037,varname:node_9864,prsc:2,v1:0.32,v2:0.32,v3:0.29;n:type:ShaderForge.SFN_NormalVector,id:1018,x:31630,y:33449,prsc:2,pt:False;n:type:ShaderForge.SFN_Normalize,id:7465,x:32226,y:33386,varname:node_7465,prsc:2|IN-9570-OUT;n:type:ShaderForge.SFN_Add,id:9570,x:31918,y:33387,varname:node_9570,prsc:2|A-9941-XYZ,B-1018-OUT;n:type:ShaderForge.SFN_Vector4Property,id:9941,x:31630,y:33299,ptovrint:False,ptlb:Wind Direction,ptin:_WindDirection,varname:node_9941,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1,v2:0.5,v3:0.5,v4:0;n:type:ShaderForge.SFN_VertexColor,id:4545,x:31630,y:33636,varname:node_4545,prsc:2;n:type:ShaderForge.SFN_Pi,id:183,x:31663,y:33764,varname:node_183,prsc:2;n:type:ShaderForge.SFN_Time,id:7769,x:31630,y:33919,varname:node_7769,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:8700,x:31630,y:34062,ptovrint:False,ptlb:Wind Power,ptin:_WindPower,varname:node_8700,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:2068,x:31910,y:33726,varname:node_2068,prsc:2|A-4545-B,B-183-OUT;n:type:ShaderForge.SFN_Multiply,id:7396,x:31910,y:33961,varname:node_7396,prsc:2|A-7769-T,B-8700-OUT;n:type:ShaderForge.SFN_Add,id:6491,x:32167,y:33907,varname:node_6491,prsc:2|A-2068-OUT,B-7396-OUT,C-2177-OUT;n:type:ShaderForge.SFN_Sin,id:1013,x:32324,y:33907,varname:node_1013,prsc:2|IN-6491-OUT;n:type:ShaderForge.SFN_Multiply,id:358,x:32553,y:33626,varname:node_358,prsc:2|A-7465-OUT,B-4545-R,C-1013-OUT,D-1992-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1992,x:32324,y:34086,ptovrint:False,ptlb:Amplitude,ptin:_Amplitude,varname:node_1992,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_TexCoord,id:6126,x:31395,y:34175,varname:node_6126,prsc:2,uv:0;n:type:ShaderForge.SFN_ComponentMask,id:3771,x:31630,y:34175,varname:node_3771,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-6126-UVOUT;n:type:ShaderForge.SFN_ValueProperty,id:7656,x:31630,y:34366,ptovrint:False,ptlb:Delay Value,ptin:_DelayValue,varname:node_7656,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:2177,x:31910,y:34205,varname:node_2177,prsc:2|A-3771-OUT,B-7656-OUT;n:type:ShaderForge.SFN_Tex2d,id:3332,x:32405,y:33133,ptovrint:False,ptlb:Alpha Map,ptin:_AlphaMap,varname:node_3332,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Vector1,id:8285,x:32405,y:33319,varname:node_8285,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:264,x:32643,y:33204,varname:node_264,prsc:2|A-3332-RGB,B-8285-OUT;n:type:ShaderForge.SFN_LightVector,id:672,x:32077,y:32312,varname:node_672,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:1344,x:32077,y:32127,prsc:2,pt:False;n:type:ShaderForge.SFN_Dot,id:4944,x:32355,y:32188,varname:node_4944,prsc:2,dt:0|A-1344-OUT,B-672-OUT;n:type:ShaderForge.SFN_Multiply,id:6814,x:32619,y:32258,varname:node_6814,prsc:2|A-4944-OUT,B-8269-RGB;n:type:ShaderForge.SFN_AmbientLight,id:3233,x:32455,y:32384,varname:node_3233,prsc:2;n:type:ShaderForge.SFN_LightAttenuation,id:7170,x:32830,y:32007,varname:node_7170,prsc:2;n:type:ShaderForge.SFN_Multiply,id:1019,x:33094,y:32159,varname:node_1019,prsc:2|A-7170-OUT,B-4197-RGB,C-4391-OUT;n:type:ShaderForge.SFN_Multiply,id:7608,x:32830,y:32433,varname:node_7608,prsc:2|A-3233-RGB,B-8269-RGB;n:type:ShaderForge.SFN_Add,id:4391,x:32830,y:32271,varname:node_4391,prsc:2|A-6814-OUT,B-7608-OUT;n:type:ShaderForge.SFN_LightColor,id:4197,x:32830,y:32142,varname:node_4197,prsc:2;n:type:ShaderForge.SFN_Multiply,id:904,x:33325,y:32237,varname:node_904,prsc:2|A-1019-OUT,B-4834-OUT;n:type:ShaderForge.SFN_Add,id:7616,x:33376,y:32463,varname:node_7616,prsc:2|A-904-OUT,B-1342-OUT;n:type:ShaderForge.SFN_Slider,id:2099,x:32416,y:32681,ptovrint:False,ptlb:unlit Value,ptin:_unlitValue,varname:node_2099,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5,max:1;n:type:ShaderForge.SFN_Vector1,id:7433,x:32573,y:32761,varname:node_7433,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:1342,x:32830,y:32567,varname:node_1342,prsc:2|A-8269-RGB,B-2099-OUT;n:type:ShaderForge.SFN_Subtract,id:4834,x:32830,y:32703,varname:node_4834,prsc:2|A-7433-OUT,B-2099-OUT;n:type:ShaderForge.SFN_Color,id:1420,x:33491,y:32078,ptovrint:False,ptlb:Color Add,ptin:_ColorAdd,varname:node_1420,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:9665,x:33730,y:32170,varname:node_9665,prsc:2|A-1420-RGB,B-7616-OUT;proporder:8269-1420-3332-363-6056-4346-9941-8700-1992-7656-2099;pass:END;sub:END;*/

Shader "Custom/Plants_Grass_Shader" {
    Properties {
        _Diffuse ("Diffuse", 2D) = "white" {}
        _ColorAdd ("Color Add", Color) = (1,1,1,1)
        _AlphaMap ("Alpha Map", 2D) = "white" {}
        _Emissive ("Emissive", 2D) = "white" {}
        _EmissiveColor ("Emissive Color", Color) = (1,1,1,1)
        _Emissivevalue ("Emissive value", Float ) = 0
        _WindDirection ("Wind Direction", Vector) = (1,0.5,0.5,0)
        _WindPower ("Wind Power", Float ) = 0
        _Amplitude ("Amplitude", Float ) = 0
        _DelayValue ("Delay Value", Float ) = 0
        _unlitValue ("unlit Value", Range(0, 1)) = 0.5
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
            Blend SrcAlpha OneMinusSrcAlpha
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
            uniform float4 _TimeEditor;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform sampler2D _Emissive; uniform float4 _Emissive_ST;
            uniform float4 _EmissiveColor;
            uniform float _Emissivevalue;
            uniform float4 _WindDirection;
            uniform float _WindPower;
            uniform float _Amplitude;
            uniform float _DelayValue;
            uniform sampler2D _AlphaMap; uniform float4 _AlphaMap_ST;
            uniform float _unlitValue;
            uniform float4 _ColorAdd;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 vertexColor : COLOR;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_7769 = _Time + _TimeEditor;
                v.vertex.xyz += (normalize((_WindDirection.rgb+v.normal))*o.vertexColor.r*sin(((o.vertexColor.b*3.141592654)+(node_7769.g*_WindPower)+(o.uv0.r*_DelayValue)))*_Amplitude);
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
                float4 _AlphaMap_var = tex2D(_AlphaMap,TRANSFORM_TEX(i.uv0, _AlphaMap));
                clip((_AlphaMap_var.rgb*1.0) - 0.5);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
////// Emissive:
                float4 _Emissive_var = tex2D(_Emissive,TRANSFORM_TEX(i.uv0, _Emissive));
                float3 emissive = (_Emissive_var.rgb*_EmissiveColor.rgb*_Emissivevalue);
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float3 finalColor = emissive + (_ColorAdd.rgb*(((attenuation*_LightColor0.rgb*((dot(i.normalDir,lightDirection)*_Diffuse_var.rgb)+(UNITY_LIGHTMODEL_AMBIENT.rgb*_Diffuse_var.rgb)))*(1.0-_unlitValue))+(_Diffuse_var.rgb*_unlitValue)));
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
            uniform float4 _TimeEditor;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform sampler2D _Emissive; uniform float4 _Emissive_ST;
            uniform float4 _EmissiveColor;
            uniform float _Emissivevalue;
            uniform float4 _WindDirection;
            uniform float _WindPower;
            uniform float _Amplitude;
            uniform float _DelayValue;
            uniform sampler2D _AlphaMap; uniform float4 _AlphaMap_ST;
            uniform float _unlitValue;
            uniform float4 _ColorAdd;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 vertexColor : COLOR;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_7769 = _Time + _TimeEditor;
                v.vertex.xyz += (normalize((_WindDirection.rgb+v.normal))*o.vertexColor.r*sin(((o.vertexColor.b*3.141592654)+(node_7769.g*_WindPower)+(o.uv0.r*_DelayValue)))*_Amplitude);
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
                float4 _AlphaMap_var = tex2D(_AlphaMap,TRANSFORM_TEX(i.uv0, _AlphaMap));
                clip((_AlphaMap_var.rgb*1.0) - 0.5);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float3 finalColor = (_ColorAdd.rgb*(((attenuation*_LightColor0.rgb*((dot(i.normalDir,lightDirection)*_Diffuse_var.rgb)+(UNITY_LIGHTMODEL_AMBIENT.rgb*_Diffuse_var.rgb)))*(1.0-_unlitValue))+(_Diffuse_var.rgb*_unlitValue)));
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
            uniform float4 _TimeEditor;
            uniform float4 _WindDirection;
            uniform float _WindPower;
            uniform float _Amplitude;
            uniform float _DelayValue;
            uniform sampler2D _AlphaMap; uniform float4 _AlphaMap_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_7769 = _Time + _TimeEditor;
                v.vertex.xyz += (normalize((_WindDirection.rgb+v.normal))*o.vertexColor.r*sin(((o.vertexColor.b*3.141592654)+(node_7769.g*_WindPower)+(o.uv0.r*_DelayValue)))*_Amplitude);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float4 _AlphaMap_var = tex2D(_AlphaMap,TRANSFORM_TEX(i.uv0, _AlphaMap));
                clip((_AlphaMap_var.rgb*1.0) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
