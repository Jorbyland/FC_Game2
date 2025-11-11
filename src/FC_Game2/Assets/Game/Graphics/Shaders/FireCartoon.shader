Shader "Custom/FireCartoon"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        [HDR]_FireColor1 ("Fire Color 1 (Inner)", Color) = (1, 0.3, 0, 1)
        [HDR]_FireColor2 ("Fire Color 2 (Middle)", Color) = (1, 0.6, 0.1, 1)
        [HDR]_FireColor3 ("Fire Color 3 (Outer)", Color) = (1, 0.9, 0.3, 1)
        _FireIntensity ("Fire Intensity", Range(0, 5)) = 2.0
        _DistortionStrength ("Distortion Strength", Range(0, 1)) = 0.3
        _Speed ("Animation Speed", Range(0, 5)) = 1.0
        _NoiseScale ("Noise Scale", Range(0.1, 5)) = 1.0
        _FresnelPower ("Fresnel Power", Range(0.1, 10)) = 2.0
        _FresnelIntensity ("Fresnel Intensity", Range(0, 2)) = 1.0
        _Alpha ("Alpha", Range(0, 1)) = 0.8
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            Name "FireCartoon"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
                float3 viewDirWS : TEXCOORD3;
                float fogCoord : TEXCOORD4;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_NoiseTex);
            SAMPLER(sampler_NoiseTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _FireColor1;
                float4 _FireColor2;
                float4 _FireColor3;
                float _FireIntensity;
                float _DistortionStrength;
                float _Speed;
                float _NoiseScale;
                float _FresnelPower;
                float _FresnelIntensity;
                float _Alpha;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS);
                
                output.positionCS = vertexInput.positionCS;
                output.positionWS = vertexInput.positionWS;
                output.normalWS = normalInput.normalWS;
                output.viewDirWS = GetWorldSpaceViewDir(vertexInput.positionWS);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                output.fogCoord = ComputeFogFactor(output.positionCS.z);
                
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float time = _Time.y * _Speed;
                
                // Calculer la direction vers le haut (pour l'effet de flamme)
                float3 up = float3(0, 1, 0);
                float verticalFactor = dot(input.normalWS, up);
                verticalFactor = saturate(verticalFactor * 0.5 + 0.5); // Normaliser entre 0 et 1
                
                // Échantillonner le noise pour la distortion
                float2 noiseUV1 = input.uv * _NoiseScale + float2(0, time * 0.5);
                float2 noiseUV2 = input.uv * _NoiseScale * 1.3 + float2(0, time * 0.7);
                
                float noise1 = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, noiseUV1).r;
                float noise2 = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, noiseUV2).r;
                float combinedNoise = (noise1 + noise2) * 0.5;
                
                // Appliquer la distortion
                float2 distortedUV = input.uv;
                distortedUV.x += (combinedNoise - 0.5) * _DistortionStrength * 0.1;
                distortedUV.y += (combinedNoise - 0.5) * _DistortionStrength * 0.2 + time * 0.1;
                
                // Échantillonner la texture de base avec distortion
                half4 baseColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, distortedUV);
                
                // Calculer le gradient vertical pour les couleurs de feu
                float fireGradient = verticalFactor;
                fireGradient += (combinedNoise - 0.5) * 0.3; // Ajouter de la variation
                fireGradient = saturate(fireGradient);
                
                // Interpoler entre les couleurs de feu (cartoon style avec transitions nettes)
                half4 fireColor;
                if (fireGradient < 0.33)
                {
                    // Zone extérieure (bleu/transparent)
                    fireColor = lerp(_FireColor3, _FireColor2, fireGradient * 3.0);
                }
                else if (fireGradient < 0.66)
                {
                    // Zone moyenne
                    fireColor = lerp(_FireColor2, _FireColor1, (fireGradient - 0.33) * 3.0);
                }
                else
                {
                    // Zone intérieure (blanc/jaune chaud)
                    fireColor = _FireColor1;
                }
                
                // Effet Fresnel pour les bords (style cartoon)
                float3 viewDir = normalize(input.viewDirWS);
                float fresnel = 1.0 - saturate(dot(viewDir, input.normalWS));
                fresnel = pow(fresnel, _FresnelPower);
                fresnel *= _FresnelIntensity;
                
                // Combiner les couleurs
                half4 finalColor = baseColor * fireColor * _FireIntensity;
                finalColor.rgb += fresnel * _FireColor1.rgb * 0.5; // Ajouter de la brillance sur les bords
                
                // Alpha basé sur le gradient et le noise
                float alpha = _Alpha;
                alpha *= fireGradient; // Plus opaque vers le bas
                alpha *= (1.0 - fresnel * 0.3); // Légèrement transparent sur les bords
                alpha *= baseColor.a;
                
                // Ajouter de la variation d'alpha avec le noise pour un effet plus organique
                alpha *= lerp(0.7, 1.0, combinedNoise);
                
                finalColor.a = saturate(alpha);
                
                // Appliquer le fog
                finalColor.rgb = MixFog(finalColor.rgb, input.fogCoord);
                
                return finalColor;
            }
            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}

