Shader "Custom/VerticalCut_WithCap"
{
    Properties
    {
        _BaseMap ("Base Map", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _CutoffHeight ("Cutoff Height (world Y)", Float) = 0.0
        _SmoothRange ("Smooth Range (m)", Float) = 0.0

        // éclairage simple (contrôlé depuis l'inspector ou par script)
        _LightDir ("Light Direction", Vector) = (0.3, 0.7, 0.2, 0)
        _LightColor ("Light Color", Color) = (1,1,1,1)
        _Ambient ("Ambient Intensity", Range(0,1)) = 0.25
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalRenderPipeline" "RenderType"="Opaque" "Queue"="Geometry" }

        // ----------------------------------------------------------------
        // PASS 1 - OUTSIDE FACES (Cull Back) : lit simple, dessous du cutoff
        // ----------------------------------------------------------------
        Pass
        {
            Name "OUTSIDE_LIT"
            Tags { "LightMode"="UniversalForward" }
            Cull Back
            ZWrite On
            ZTest LEqual
            Blend Off

            HLSLPROGRAM
            #pragma vertex vert_out
            #pragma fragment frag_out
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 worldPos    : TEXCOORD0;
                float3 normalWS    : TEXCOORD1;
                float2 uv          : TEXCOORD2;
            };

            TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);
            float4 _BaseColor;
            float _CutoffHeight;
            float _SmoothRange;
            float4 _LightDir;
            float4 _LightColor;
            float _Ambient;

            Varyings vert_out(Attributes IN)
            {
                Varyings OUT;
                float3 wp = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.worldPos = wp;
                OUT.positionHCS = TransformWorldToHClip(wp);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag_out(Varyings IN) : SV_Target
            {
                // mask : 1 = visible (sous la coupe), 0 = invisible (au dessus)
                float mask = 1.0 - smoothstep(_CutoffHeight - _SmoothRange, _CutoffHeight + _SmoothRange, IN.worldPos.y);
                if (mask <= 0.001) discard;

                // albedo
                float3 albedo = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv).rgb * _BaseColor.rgb;

                // lighting simple Lambert
                float3 N = normalize(IN.normalWS);
                float3 L = normalize(_LightDir.xyz);
                float NdotL = saturate(dot(N, L));
                float3 lit = albedo * (_Ambient + NdotL * _LightColor.rgb);

                // applique mask (pour bord doux garder depth write plein)
                lit *= mask;

                return half4(lit, 1.0);
            }
            ENDHLSL
        }

        // ----------------------------------------------------------------
        // PASS 2 - INSIDE BLACK (Cull Front) : backfaces noires unlit, dessous du cutoff
        // ----------------------------------------------------------------
        Pass
        {
            Name "INSIDE_BLACK"
            Tags { "LightMode"="SRPDefaultUnlit" }
            Cull Front
            ZWrite On
            ZTest LEqual
            Blend Off

            HLSLPROGRAM
            #pragma vertex vert_in
            #pragma fragment frag_in
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct AttrI { float4 positionOS : POSITION; float3 normalOS : NORMAL; };
            struct VarI  { float4 positionHCS : SV_POSITION; float3 worldPos : TEXCOORD0; float3 normalWS : TEXCOORD1; };

            float _CutoffHeight;
            float _SmoothRange;

            VarI vert_in(AttrI IN)
            {
                VarI OUT;
                float3 wp = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.worldPos = wp;
                OUT.positionHCS = TransformWorldToHClip(wp);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                return OUT;
            }

            half4 frag_in(VarI IN) : SV_Target
            {
                // même masque que la pass extérieure : on ne veut dessiner que ce qui est sous la coupe
                float mask = 1.0 - smoothstep(_CutoffHeight - _SmoothRange, _CutoffHeight + _SmoothRange, IN.worldPos.y);
                if (mask <= 0.001) discard;

                // On affiche toutes les backfaces visibles (Cull Front -> on dessine les faces qui normalement seraient internes)
                // Noir unlit, on ignore la normale et l'éclairage
                return half4(0.0, 0.0, 0.0, 1.0);
            }
            ENDHLSL
        }
    }

    FallBack Off
}