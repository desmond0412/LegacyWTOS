// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:1058,x:32750,y:32729,varname:node_1058,prsc:2|diff-8460-OUT,spec-6440-OUT,gloss-8639-OUT,emission-5039-OUT,transm-6353-OUT,lwrap-9864-OUT,alpha-8269-A,clip-8269-A,voffset-358-OUT;n:type:ShaderForge.SFN_Tex2d,id:8269,x:32213,y:32597,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:_Diffuse,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Vector1,id:6440,x:32498,y:32760,varname:node_6440,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:8639,x:32498,y:32811,varname:node_8639,prsc:2,v1:0;n:type:ShaderForge.SFN_Tex2d,id:363,x:31875,y:32817,ptovrint:False,ptlb:Emissive,ptin:_Emissive,varname:_Emissive,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:6056,x:31875,y:33011,ptovrint:False,ptlb:Emissive Color,ptin:_EmissiveColor,varname:_EmissiveColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:4346,x:31875,y:33191,ptovrint:False,ptlb:Emissive value,ptin:_Emissivevalue,varname:_Emissivevalue,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:5039,x:32213,y:32894,varname:node_5039,prsc:2|A-363-RGB,B-6056-RGB,C-4346-OUT;n:type:ShaderForge.SFN_Vector3,id:6353,x:32214,y:33043,varname:node_6353,prsc:2,v1:0.28,v2:0.31,v3:0.15;n:type:ShaderForge.SFN_Vector3,id:9864,x:32214,y:33144,varname:node_9864,prsc:2,v1:0.32,v2:0.32,v3:0.29;n:type:ShaderForge.SFN_NormalVector,id:1018,x:31630,y:33449,prsc:2,pt:False;n:type:ShaderForge.SFN_Normalize,id:7465,x:32226,y:33386,varname:node_7465,prsc:2|IN-9570-OUT;n:type:ShaderForge.SFN_Add,id:9570,x:31918,y:33387,varname:node_9570,prsc:2|A-9941-XYZ,B-1018-OUT;n:type:ShaderForge.SFN_Vector4Property,id:9941,x:31630,y:33299,ptovrint:False,ptlb:Wind Direction,ptin:_WindDirection,varname:_WindDirection,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1,v2:0.5,v3:0.5,v4:0;n:type:ShaderForge.SFN_VertexColor,id:4545,x:31630,y:33636,varname:node_4545,prsc:2;n:type:ShaderForge.SFN_Pi,id:183,x:31663,y:33764,varname:node_183,prsc:2;n:type:ShaderForge.SFN_Time,id:7769,x:31630,y:33919,varname:node_7769,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:8700,x:31630,y:34062,ptovrint:False,ptlb:Wind Power,ptin:_WindPower,varname:_WindPower,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:2068,x:31910,y:33726,varname:node_2068,prsc:2|A-4545-B,B-183-OUT;n:type:ShaderForge.SFN_Multiply,id:7396,x:31910,y:33961,varname:node_7396,prsc:2|A-7769-T,B-8700-OUT;n:type:ShaderForge.SFN_Add,id:6491,x:32167,y:33907,varname:node_6491,prsc:2|A-2068-OUT,B-7396-OUT,C-2177-OUT;n:type:ShaderForge.SFN_Sin,id:1013,x:32324,y:33907,varname:node_1013,prsc:2|IN-6491-OUT;n:type:ShaderForge.SFN_Multiply,id:358,x:32553,y:33626,varname:node_358,prsc:2|A-7465-OUT,B-4545-R,C-1013-OUT,D-1992-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1992,x:32337,y:34071,ptovrint:False,ptlb:Amplitude,ptin:_Amplitude,varname:_Amplitude,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_TexCoord,id:6126,x:31395,y:34175,varname:node_6126,prsc:2,uv:0;n:type:ShaderForge.SFN_ComponentMask,id:3771,x:31630,y:34175,varname:node_3771,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-6126-UVOUT;n:type:ShaderForge.SFN_ValueProperty,id:7656,x:31630,y:34366,ptovrint:False,ptlb:Delay Value,ptin:_DelayValue,varname:_DelayValue,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:2177,x:31910,y:34205,varname:node_2177,prsc:2|A-3771-OUT,B-7656-OUT;n:type:ShaderForge.SFN_Divide,id:8460,x:32498,y:32595,varname:node_8460,prsc:2|A-8269-RGB,B-9770-OUT;n:type:ShaderForge.SFN_ValueProperty,id:9770,x:32213,y:32792,ptovrint:False,ptlb:Level Color,ptin:_LevelColor,varname:_LevelColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;proporder:8269-363-6056-4346-9941-8700-1992-7656-9770;pass:END;sub:END;*/

Shader "" {
    Properties {
        _Diffuse ("Diffuse", 2D) = "white" {}
        _Emissive ("Emissive", 2D) = "white" {}
        _EmissiveColor ("Emissive Color", Color) = (1,1,1,1)
        _Emissivevalue ("Emissive value", Float ) = 0
        _WindDirection ("Wind Direction", Vector) = (1,0.5,0.5,0)
        _WindPower ("Wind Power", Float ) = 0
        _Amplitude ("Amplitude", Float ) = 0
        _DelayValue ("Delay Value", Float ) = 0
        _LevelColor ("Level Color", Float ) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform sampler2D _Emissive; uniform float4 _Emissive_ST;
            uniform float4 _EmissiveColor;
            uniform float _Emissivevalue;
            uniform float4 _WindDirection;
            uniform float _WindPower;
            uniform float _Amplitude;
            uniform float _DelayValue;
            uniform float _LevelColor;
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
                UNITY_FOG_COORDS(3)
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
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                clip(_Diffuse_var.a - 0.5);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = 0.0;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float node_6440 = 0.0;
                float3 specularColor = float3(node_6440,node_6440,node_6440);
                float3 directSpecular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = dot( normalDirection, lightDirection );
                float3 w = float3(0.32,0.32,0.29)*0.5; // Light wrapping
                float3 NdotLWrap = NdotL * ( 1.0 - w );
                float3 forwardLight = max(float3(0.0,0.0,0.0), NdotLWrap + w );
                float3 backLight = max(float3(0.0,0.0,0.0), -NdotLWrap + w ) * float3(0.28,0.31,0.15);
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = (forwardLight+backLight) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float3 diffuseColor = (_Diffuse_var.rgb/_LevelColor);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float4 _Emissive_var = tex2D(_Emissive,TRANSFORM_TEX(i.uv0, _Emissive));
                float3 emissive = (_Emissive_var.rgb*_EmissiveColor.rgb*_Emissivevalue);
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
                fixed4 finalRGBA = fixed4(finalColor,_Diffuse_var.a);
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
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform sampler2D _Emissive; uniform float4 _Emissive_ST;
            uniform float4 _EmissiveColor;
            uniform float _Emissivevalue;
            uniform float4 _WindDirection;
            uniform float _WindPower;
            uniform float _Amplitude;
            uniform float _DelayValue;
            uniform float _LevelColor;
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
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                clip(_Diffuse_var.a - 0.5);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = 0.0;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float node_6440 = 0.0;
                float3 specularColor = float3(node_6440,node_6440,node_6440);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = dot( normalDirection, lightDirection );
                float3 w = float3(0.32,0.32,0.29)*0.5; // Light wrapping
                float3 NdotLWrap = NdotL * ( 1.0 - w );
                float3 forwardLight = max(float3(0.0,0.0,0.0), NdotLWrap + w );
                float3 backLight = max(float3(0.0,0.0,0.0), -NdotLWrap + w ) * float3(0.28,0.31,0.15);
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = (forwardLight+backLight) * attenColor;
                float3 diffuseColor = (_Diffuse_var.rgb/_LevelColor);
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * _Diffuse_var.a,0);
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
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float4 _WindDirection;
            uniform float _WindPower;
            uniform float _Amplitude;
            uniform float _DelayValue;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_7769 = _Time + _TimeEditor;
                v.vertex.xyz += (normalize((_WindDirection.rgb+v.normal))*o.vertexColor.r*sin(((o.vertexColor.b*3.141592654)+(node_7769.g*_WindPower)+(o.uv0.r*_DelayValue)))*_Amplitude);
                o.pos = UnityObjectToClipPos(v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                clip(_Diffuse_var.a - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
