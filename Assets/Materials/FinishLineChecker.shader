Shader "Custom/FinishLineChecker"
{
    Properties
    {
        _WhiteColor ("White", Color) = (1, 1, 1, 1)
        _BlackColor ("Black", Color) = (0, 0, 0, 1)
        _Columns ("Columns", Float) = 8
        _Rows ("Rows", Float) = 2
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
            "Queue" = "Geometry"
        }

        Pass
        {
            Name "Checker"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
                half4 _WhiteColor;
                half4 _BlackColor;
                float _Columns;
                float _Rows;
            CBUFFER_END

            Varyings Vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = input.uv;
                return output;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                float2 square = floor(input.uv * float2(_Columns, _Rows));
                float checker = fmod(square.x + square.y, 2.0);
                return lerp(_WhiteColor, _BlackColor, checker);
            }
            ENDHLSL
        }
    }
}
