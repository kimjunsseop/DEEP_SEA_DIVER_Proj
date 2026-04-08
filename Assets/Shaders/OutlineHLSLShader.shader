Shader "Custom/URP/OutlineHLSLShader"
{
    Properties
    {
        [MainTexture] _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,0,0,1)
        _OutlineWidth ("Outline Width (pixels)", Float) = 2
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

        Pass
        {
            // 2D Renderer의 라이트 연산 루프에 포함되기 위한 태그
            Tags { "LightMode"="Universal2D" }

            ZWrite Off
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR; // Sprite Renderer의 Color와 Light 데이터가 담김
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR; // 보간된 라이트 컬러
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_TexelSize;
            float4 _OutlineColor;
            float _OutlineWidth;

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                // 중요: 2D Renderer는 라이팅 계산 결과를 정점 컬러(v.color)에 구워서 전달합니다.
                o.color = v.color; 
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                // 1. 메인 텍스처 샘플링
                float4 mainTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                
                // 2. 아웃라인 마스크 계산
                float2 offset = _MainTex_TexelSize.xy * _OutlineWidth;
                float a1 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv + float2(offset.x, 0)).a;
                float a2 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv + float2(-offset.x, 0)).a;
                float a3 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv + float2(0, offset.y)).a;
                float a4 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv + float2(0, -offset.y)).a;

                float outlineMask = saturate(max(max(a1, a2), max(a3, a4)) - mainTex.a);

                // 3. 최종 컬러 결정 (이미지 + 외곽선)
                // 조명을 곱하기 전의 순수 색상 상태입니다.
                float3 rawRGB = lerp(mainTex.rgb, _OutlineColor.rgb, outlineMask);
                float rawAlpha = max(mainTex.a, outlineMask);

                // 4. 라이팅 적용 (핵심)
                // i.color는 2D 라이트 시스템이 계산한 현재 픽셀의 밝기(Intensity)와 색상입니다.
                // 빛이 없는 곳에선 i.color가 (0,0,0)이 되어 아무것도 보이지 않게 됩니다.
                float4 finalCol;
                finalCol.rgb = rawRGB * i.color.rgb; 
                finalCol.a = rawAlpha * i.color.a;

                return finalCol;
            }
            ENDHLSL
        }
    }
}