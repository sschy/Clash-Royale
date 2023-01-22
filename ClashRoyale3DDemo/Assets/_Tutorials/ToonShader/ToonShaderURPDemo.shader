﻿Shader "91make/ToonShaderURP"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Brightness("Brightness", Range(0,1)) = 0.3
        _Strength("Strength", Range(0,1)) = 0.5
        _Color("Color", COLOR) = (1,1,1,1)
        _Detail("Detail", Range(0,1)) = 0.3
    }
    SubShader
    {
        Tags 
        {
            "RenderType"="Opaque"
            "RenderPipeline"="UniversalPipeline" 
        }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                half3 worldNormal : TEXCOORD1;
            };

            sampler2D _MainTex;
                        
            CBUFFER_START(UnityPerMaterial)
            float4 _Color;
            float4 _MainTex_ST;
            float _Brightness;
            float _Strength;
            float _Detail;
            CBUFFER_END

            float Toon(float3 normal, float lightDir)
            {
                float f = max(0.0, dot(normalize(normal), normalize(lightDir)));
                return floor(f/_Detail);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.pos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = TransformObjectToHClip(v.normal);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // sample the texture
                half4 col = tex2D(_MainTex, i.uv);
                col *= Toon(i.worldNormal, _MainLightPosition.xyz)*_Strength*_Color+_Brightness;
                return col;
            }
            ENDHLSL
        }
    }
}
