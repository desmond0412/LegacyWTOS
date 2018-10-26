// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:6874,x:35776,y:32663,varname:node_6874,prsc:2|emission-4465-OUT,transm-5665-OUT,lwrap-5665-OUT,custl-6103-OUT,clip-8123-OUT,voffset-4408-OUT;n:type:ShaderForge.SFN_Tex2d,id:34,x:33083,y:32802,ptovrint:False,ptlb:Emission,ptin:_Emission,varname:node_34,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:6980,x:32560,y:33673,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:node_6980,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Panner,id:4304,x:32091,y:32399,varname:node_4304,prsc:2,spu:0.15,spv:0|UVIN-3233-OUT;n:type:ShaderForge.SFN_ComponentMask,id:8242,x:32286,y:32399,varname:node_8242,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-4304-UVOUT;n:type:ShaderForge.SFN_Frac,id:8489,x:32480,y:32390,varname:node_8489,prsc:2|IN-8242-OUT;n:type:ShaderForge.SFN_Subtract,id:8786,x:32667,y:32390,varname:node_8786,prsc:2|A-8489-OUT,B-7466-OUT;n:type:ShaderForge.SFN_Vector1,id:7466,x:32451,y:32565,varname:node_7466,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Multiply,id:4698,x:33047,y:32390,varname:node_4698,prsc:2|A-493-OUT,B-387-OUT;n:type:ShaderForge.SFN_Abs,id:493,x:32856,y:32390,varname:node_493,prsc:2|IN-8786-OUT;n:type:ShaderForge.SFN_Vector1,id:387,x:32856,y:32522,varname:node_387,prsc:2,v1:2;n:type:ShaderForge.SFN_Power,id:5665,x:33332,y:32577,varname:node_5665,prsc:2|VAL-4698-OUT,EXP-9091-OUT;n:type:ShaderForge.SFN_ValueProperty,id:9091,x:33080,y:32596,ptovrint:False,ptlb:Bulge Shape,ptin:_BulgeShape,varname:node_9091,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_Color,id:6319,x:32879,y:32712,ptovrint:False,ptlb:Glow Color,ptin:_GlowColor,varname:node_6319,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.2,c3:0.1,c4:1;n:type:ShaderForge.SFN_Multiply,id:8806,x:33668,y:32764,varname:node_8806,prsc:2|A-5665-OUT,B-6319-RGB,C-34-RGB,D-9581-OUT;n:type:ShaderForge.SFN_NormalVector,id:4362,x:35412,y:33162,prsc:2,pt:False;n:type:ShaderForge.SFN_Multiply,id:4408,x:35594,y:33082,varname:node_4408,prsc:2|A-4362-OUT,B-9259-OUT;n:type:ShaderForge.SFN_TexCoord,id:1612,x:31363,y:32415,varname:node_1612,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:9581,x:33297,y:32962,varname:node_9581,prsc:2|A-5548-OUT,B-9251-OUT;n:type:ShaderForge.SFN_Vector1,id:9251,x:33083,y:33006,varname:node_9251,prsc:2,v1:5;n:type:ShaderForge.SFN_Rotator,id:9354,x:31805,y:32417,varname:node_9354,prsc:2|UVIN-1612-UVOUT,PIV-8200-OUT,ANG-9922-OUT;n:type:ShaderForge.SFN_Slider,id:5548,x:32748,y:32959,ptovrint:False,ptlb:Glow Intensity,ptin:_GlowIntensity,varname:node_5548,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1.5;n:type:ShaderForge.SFN_Vector2,id:8200,x:31505,y:32506,varname:node_8200,prsc:2,v1:0.5,v2:0.5;n:type:ShaderForge.SFN_SwitchProperty,id:5616,x:31655,y:32948,ptovrint:False,ptlb:Rotate Direction,ptin:_RotateDirection,cmnt:Rotate Direction,varname:node_5616,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-3799-OUT,B-4639-OUT;n:type:ShaderForge.SFN_Multiply,id:1687,x:31908,y:32735,varname:node_1687,prsc:2|A-1767-OUT,B-5616-OUT;n:type:ShaderForge.SFN_Slider,id:1767,x:31554,y:32778,ptovrint:False,ptlb:Direction Angle,ptin:_DirectionAngle,varname:node_1767,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:80,max:360;n:type:ShaderForge.SFN_RemapRange,id:9922,x:32088,y:32735,varname:node_9922,prsc:2,frmn:0,frmx:1,tomn:0,tomx:6.28|IN-1687-OUT;n:type:ShaderForge.SFN_Vector1,id:3799,x:31422,y:32881,cmnt:Clockwise,varname:node_3799,prsc:2,v1:-1;n:type:ShaderForge.SFN_Vector1,id:4639,x:31422,y:32982,cmnt:Counter CLockwise,varname:node_4639,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:1088,x:31838,y:32166,cmnt:Speed,varname:node_1088,prsc:2|A-5916-OUT,B-2509-OUT;n:type:ShaderForge.SFN_Time,id:9211,x:31493,y:32225,varname:node_9211,prsc:2;n:type:ShaderForge.SFN_Add,id:3233,x:32024,y:32211,varname:node_3233,prsc:2|A-1088-OUT,B-9354-UVOUT;n:type:ShaderForge.SFN_Slider,id:5916,x:31415,y:32120,ptovrint:False,ptlb:Glow Speed,ptin:_GlowSpeed,varname:node_5916,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:2;n:type:ShaderForge.SFN_LightVector,id:5885,x:32126,y:33424,varname:node_5885,prsc:2;n:type:ShaderForge.SFN_LightColor,id:7434,x:33519,y:33178,varname:node_7434,prsc:2;n:type:ShaderForge.SFN_LightAttenuation,id:6041,x:33507,y:33385,varname:node_6041,prsc:2;n:type:ShaderForge.SFN_AmbientLight,id:9746,x:32857,y:33467,varname:node_9746,prsc:2;n:type:ShaderForge.SFN_Dot,id:4628,x:32656,y:33333,varname:node_4628,prsc:2,dt:0|A-7428-OUT,B-5885-OUT;n:type:ShaderForge.SFN_Multiply,id:2299,x:32997,y:33284,varname:node_2299,prsc:2|A-4628-OUT,B-6980-RGB;n:type:ShaderForge.SFN_Add,id:5953,x:33353,y:33288,varname:node_5953,prsc:2|A-2299-OUT,B-3057-OUT;n:type:ShaderForge.SFN_Multiply,id:3057,x:33107,y:33550,varname:node_3057,prsc:2|A-9746-RGB,B-6980-RGB;n:type:ShaderForge.SFN_Multiply,id:8902,x:33914,y:33671,varname:node_8902,prsc:2|A-6980-RGB,B-807-OUT;n:type:ShaderForge.SFN_Slider,id:807,x:33507,y:33610,ptovrint:False,ptlb:Unlit Value,ptin:_UnlitValue,varname:node_807,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5,max:1;n:type:ShaderForge.SFN_Subtract,id:7799,x:33914,y:33518,varname:node_7799,prsc:2|A-7529-OUT,B-807-OUT;n:type:ShaderForge.SFN_Vector1,id:7529,x:33664,y:33518,varname:node_7529,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:6235,x:33774,y:33248,varname:node_6235,prsc:2|A-7434-RGB,B-6041-OUT,C-5953-OUT;n:type:ShaderForge.SFN_NormalVector,id:7428,x:32126,y:33259,prsc:2,pt:False;n:type:ShaderForge.SFN_Multiply,id:5287,x:34000,y:33248,varname:node_5287,prsc:2|A-6235-OUT,B-7799-OUT;n:type:ShaderForge.SFN_Add,id:6103,x:34298,y:33246,varname:node_6103,prsc:2|A-5287-OUT,B-8902-OUT;n:type:ShaderForge.SFN_Divide,id:2509,x:31680,y:32276,varname:node_2509,prsc:2|A-9211-T,B-5189-OUT;n:type:ShaderForge.SFN_Vector1,id:5189,x:31505,y:32353,varname:node_5189,prsc:2,v1:5;n:type:ShaderForge.SFN_Tex2d,id:6136,x:33172,y:33873,ptovrint:False,ptlb:Dissolve,ptin:_Dissolve,varname:node_6136,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:3920,x:33488,y:33889,varname:node_3920,prsc:2|A-6136-R,B-1000-OUT;n:type:ShaderForge.SFN_Slider,id:1000,x:33021,y:34182,ptovrint:False,ptlb:Dissolve Intensity,ptin:_DissolveIntensity,varname:node_1000,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Multiply,id:8598,x:33762,y:33922,varname:node_8598,prsc:2|A-3920-OUT,B-2250-OUT;n:type:ShaderForge.SFN_Vector1,id:2250,x:33488,y:34011,varname:node_2250,prsc:2,v1:5;n:type:ShaderForge.SFN_Add,id:9455,x:34016,y:33970,varname:node_9455,prsc:2|A-8598-OUT,B-1000-OUT;n:type:ShaderForge.SFN_Power,id:946,x:34272,y:33967,varname:node_946,prsc:2|VAL-9455-OUT,EXP-9440-OUT;n:type:ShaderForge.SFN_Vector1,id:9440,x:34016,y:34120,varname:node_9440,prsc:2,v1:100;n:type:ShaderForge.SFN_ConstantClamp,id:336,x:34548,y:33984,varname:node_336,prsc:2,min:0,max:1|IN-946-OUT;n:type:ShaderForge.SFN_If,id:7136,x:34680,y:33645,varname:node_7136,prsc:2|A-5573-OUT,B-946-OUT,GT-6977-OUT,EQ-9150-OUT,LT-9150-OUT;n:type:ShaderForge.SFN_Slider,id:527,x:34288,y:33464,ptovrint:False,ptlb:Edge Width,ptin:_EdgeWidth,varname:node_527,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Vector1,id:6977,x:34376,y:33682,varname:node_6977,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector1,id:9150,x:34376,y:33737,varname:node_9150,prsc:2,v1:0;n:type:ShaderForge.SFN_Add,id:4465,x:34093,y:32770,varname:node_4465,prsc:2|A-8806-OUT,B-2127-OUT;n:type:ShaderForge.SFN_Multiply,id:650,x:35034,y:33645,varname:node_650,prsc:2|A-7136-OUT,B-4680-RGB;n:type:ShaderForge.SFN_Color,id:4680,x:34835,y:33724,ptovrint:False,ptlb:Edge Color,ptin:_EdgeColor,varname:node_4680,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5735294,c2:0.8588235,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:5573,x:34669,y:33463,varname:node_5573,prsc:2|A-527-OUT,B-4437-OUT;n:type:ShaderForge.SFN_Vector1,id:4437,x:34426,y:33549,varname:node_4437,prsc:2,v1:10;n:type:ShaderForge.SFN_Set,id:9859,x:35256,y:33645,varname:EdgeNode,prsc:2|IN-650-OUT;n:type:ShaderForge.SFN_Get,id:2127,x:33864,y:32826,varname:node_2127,prsc:2|IN-9859-OUT;n:type:ShaderForge.SFN_Set,id:4590,x:34782,y:33984,varname:OpacityNode,prsc:2|IN-336-OUT;n:type:ShaderForge.SFN_Get,id:8123,x:35573,y:32942,varname:node_8123,prsc:2|IN-4590-OUT;n:type:ShaderForge.SFN_Vector1,id:9259,x:35412,y:33103,varname:node_9259,prsc:2,v1:0;n:type:ShaderForge.SFN_OneMinus,id:9167,x:33412,y:34205,varname:node_9167,prsc:2|IN-1000-OUT;proporder:6980-807-34-6319-5548-5916-9091-5616-1767-6136-1000-527-4680;pass:END;sub:END;*/

Shader "Custom/LightEaterShader" {
    Properties {
        _Diffuse ("Diffuse", 2D) = "white" {}
        _UnlitValue ("Unlit Value", Range(0, 1)) = 0.5
        _Emission ("Emission", 2D) = "white" {}
        _GlowColor ("Glow Color", Color) = (1,0.2,0.1,1)
        _GlowIntensity ("Glow Intensity", Range(0, 1.5)) = 1
        _GlowSpeed ("Glow Speed", Range(0, 2)) = 0
        _BulgeShape ("Bulge Shape", Float ) = 2
        [MaterialToggle] _RotateDirection ("Rotate Direction", Float ) = -1
        _DirectionAngle ("Direction Angle", Range(0, 360)) = 80
        _Dissolve ("Dissolve", 2D) = "white" {}
        _DissolveIntensity ("Dissolve Intensity", Range(0, 1)) = 1
        _EdgeWidth ("Edge Width", Range(0, 1)) = 1
        _EdgeColor ("Edge Color", Color) = (0.5735294,0.8588235,1,1)
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
            uniform float4 _TimeEditor;
            uniform sampler2D _Emission; uniform float4 _Emission_ST;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float _BulgeShape;
            uniform float4 _GlowColor;
            uniform float _GlowIntensity;
            uniform fixed _RotateDirection;
            uniform float _DirectionAngle;
            uniform float _GlowSpeed;
            uniform float _UnlitValue;
            uniform sampler2D _Dissolve; uniform float4 _Dissolve_ST;
            uniform float _DissolveIntensity;
            uniform float _EdgeWidth;
            uniform float4 _EdgeColor;
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
                v.vertex.xyz += (v.normal*0.0);
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
                float4 _Dissolve_var = tex2D(_Dissolve,TRANSFORM_TEX(i.uv0, _Dissolve));
                float node_946 = pow((((_Dissolve_var.r*_DissolveIntensity)*5.0)+_DissolveIntensity),100.0);
                float OpacityNode = clamp(node_946,0,1);
                clip(OpacityNode - 0.5);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
////// Emissive:
                float4 node_5501 = _Time + _TimeEditor;
                float4 node_9211 = _Time + _TimeEditor;
                float node_9354_ang = ((_DirectionAngle*lerp( (-1.0), 1.0, _RotateDirection ))*6.28+0.0);
                float node_9354_spd = 1.0;
                float node_9354_cos = cos(node_9354_spd*node_9354_ang);
                float node_9354_sin = sin(node_9354_spd*node_9354_ang);
                float2 node_9354_piv = float2(0.5,0.5);
                float2 node_9354 = (mul(i.uv0-node_9354_piv,float2x2( node_9354_cos, -node_9354_sin, node_9354_sin, node_9354_cos))+node_9354_piv);
                float node_5665 = pow((abs((frac((((_GlowSpeed*(node_9211.g/5.0))+node_9354)+node_5501.g*float2(0.15,0)).r)-0.5))*2.0),_BulgeShape);
                float4 _Emission_var = tex2D(_Emission,TRANSFORM_TEX(i.uv0, _Emission));
                float node_7136_if_leA = step((_EdgeWidth*10.0),node_946);
                float node_7136_if_leB = step(node_946,(_EdgeWidth*10.0));
                float node_9150 = 0.0;
                float3 EdgeNode = (lerp((node_7136_if_leA*node_9150)+(node_7136_if_leB*1.0),node_9150,node_7136_if_leA*node_7136_if_leB)*_EdgeColor.rgb);
                float3 emissive = ((node_5665*_GlowColor.rgb*_Emission_var.rgb*(_GlowIntensity*5.0))+EdgeNode);
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float3 finalColor = emissive + (((_LightColor0.rgb*attenuation*((dot(i.normalDir,lightDirection)*_Diffuse_var.rgb)+(UNITY_LIGHTMODEL_AMBIENT.rgb*_Diffuse_var.rgb)))*(1.0-_UnlitValue))+(_Diffuse_var.rgb*_UnlitValue));
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
            uniform sampler2D _Emission; uniform float4 _Emission_ST;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float _BulgeShape;
            uniform float4 _GlowColor;
            uniform float _GlowIntensity;
            uniform fixed _RotateDirection;
            uniform float _DirectionAngle;
            uniform float _GlowSpeed;
            uniform float _UnlitValue;
            uniform sampler2D _Dissolve; uniform float4 _Dissolve_ST;
            uniform float _DissolveIntensity;
            uniform float _EdgeWidth;
            uniform float4 _EdgeColor;
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
                v.vertex.xyz += (v.normal*0.0);
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
                float4 _Dissolve_var = tex2D(_Dissolve,TRANSFORM_TEX(i.uv0, _Dissolve));
                float node_946 = pow((((_Dissolve_var.r*_DissolveIntensity)*5.0)+_DissolveIntensity),100.0);
                float OpacityNode = clamp(node_946,0,1);
                clip(OpacityNode - 0.5);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float3 finalColor = (((_LightColor0.rgb*attenuation*((dot(i.normalDir,lightDirection)*_Diffuse_var.rgb)+(UNITY_LIGHTMODEL_AMBIENT.rgb*_Diffuse_var.rgb)))*(1.0-_UnlitValue))+(_Diffuse_var.rgb*_UnlitValue));
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
            uniform sampler2D _Dissolve; uniform float4 _Dissolve_ST;
            uniform float _DissolveIntensity;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                v.vertex.xyz += (v.normal*0.0);
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
                float4 _Dissolve_var = tex2D(_Dissolve,TRANSFORM_TEX(i.uv0, _Dissolve));
                float node_946 = pow((((_Dissolve_var.r*_DissolveIntensity)*5.0)+_DissolveIntensity),100.0);
                float OpacityNode = clamp(node_946,0,1);
                clip(OpacityNode - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
