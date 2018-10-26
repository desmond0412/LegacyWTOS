// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True;n:type:ShaderForge.SFN_Final,id:4795,x:32724,y:32693,varname:node_4795,prsc:2|emission-5206-OUT,voffset-8582-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:32235,y:32601,ptovrint:False,ptlb:MainText,ptin:_MainText,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-2819-UVOUT;n:type:ShaderForge.SFN_Add,id:1426,x:32484,y:32676,varname:node_1426,prsc:2|A-6074-RGB,B-8609-RGB;n:type:ShaderForge.SFN_Tex2d,id:8609,x:32235,y:32809,ptovrint:False,ptlb:SecTexture,ptin:_SecTexture,varname:node_8609,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-5055-UVOUT;n:type:ShaderForge.SFN_Panner,id:2819,x:32043,y:32579,varname:node_2819,prsc:2,spu:0,spv:1|UVIN-6538-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:6538,x:31860,y:32579,varname:node_6538,prsc:2,uv:0;n:type:ShaderForge.SFN_TexCoord,id:5043,x:31847,y:32805,varname:node_5043,prsc:2,uv:0;n:type:ShaderForge.SFN_Panner,id:5055,x:32060,y:32805,varname:node_5055,prsc:2,spu:0,spv:0.5|UVIN-5043-UVOUT;n:type:ShaderForge.SFN_Noise,id:6465,x:32382,y:33012,varname:node_6465,prsc:2|XY-5043-UVOUT;n:type:ShaderForge.SFN_Slider,id:2111,x:32252,y:33196,ptovrint:False,ptlb:Displacement,ptin:_Displacement,varname:node_2111,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:0.1;n:type:ShaderForge.SFN_Multiply,id:8582,x:32564,y:33045,varname:node_8582,prsc:2|A-6465-OUT,B-2111-OUT;n:type:ShaderForge.SFN_Add,id:5206,x:32471,y:32514,varname:node_5206,prsc:2|A-6074-RGB,B-8609-RGB;proporder:6074-8609-2111;pass:END;sub:END;*/

Shader "Shader Forge/WaterFlow" {
    Properties {
        _MainText ("MainText", 2D) = "white" {}
        _SecTexture ("SecTexture", 2D) = "white" {}
        _Displacement ("Displacement", Range(0, 0.1)) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
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
            uniform float4 _TimeEditor;
            uniform sampler2D _MainText; uniform float4 _MainText_ST;
            uniform sampler2D _SecTexture; uniform float4 _SecTexture_ST;
            uniform float _Displacement;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                float2 node_6465_skew = o.uv0 + 0.2127+o.uv0.x*0.3713*o.uv0.y;
                float2 node_6465_rnd = 4.789*sin(489.123*(node_6465_skew));
                float node_6465 = frac(node_6465_rnd.x*node_6465_rnd.y*(1+node_6465_skew.x));
                float node_8582 = (node_6465*_Displacement);
                v.vertex.xyz += float3(node_8582,node_8582,node_8582);
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 node_4199 = _Time + _TimeEditor;
                float2 node_2819 = (i.uv0+node_4199.g*float2(0,1));
                float4 _MainText_var = tex2D(_MainText,TRANSFORM_TEX(node_2819, _MainText));
                float2 node_5055 = (i.uv0+node_4199.g*float2(0,0.5));
                float4 _SecTexture_var = tex2D(_SecTexture,TRANSFORM_TEX(node_5055, _SecTexture));
                float3 emissive = (_MainText_var.rgb+_SecTexture_var.rgb);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0,0,0,1));
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
