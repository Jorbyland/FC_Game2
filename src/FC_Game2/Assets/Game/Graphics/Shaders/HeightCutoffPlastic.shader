Shader "Game/HeightCutoffPlastic"
{
    Properties
    {
        _BaseMap ("Base Map", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _Smoothness ("Smoothness", Range(0,1)) = 0.65
        _SpecularColor ("Specular Color", Color) = (1,1,1,1)
        _CutoffHeight ("Cutoff Height (World Y)", Float) = 0
        _CutoffFeather ("Cutoff Feather", Range(0.001, 1)) = 0.1
        _DitherStrength ("Dither Strength", Range(0,1)) = 1.0
        _Transparency ("Global Transparency", Range(0,1)) = 0.0
        _AmbientColor ("Ambient Color", Color) = (0.28,0.30,0.33,1)
    }

    SubShader
    {
        Tags { "RenderType"="TransparentCutout" "Queue"="AlphaTest" "RenderPipeline"="UniversalRenderPipeline" }
        LOD 300

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // Texture / samplers (classic declarations)
            sampler2D _BaseMap;
            float4 _BaseColor;
            float _Smoothness;
            float4 _SpecularColor;
            float _CutoffHeight;
            float _CutoffFeather;
            float _DitherStrength;
            float _Transparency;
            float4 _AmbientColor;
            float4 _LightColor0;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 positionWS  : TEXCOORD0;
                float3 normalWS    : TEXCOORD1;
                float2 uv          : TEXCOORD2;
                float4 screenPos   : TEXCOORD3;
            };

            Varyings vert(Attributes v)
            {
                Varyings o;
                float4 worldPos = mul(unity_ObjectToWorld, v.positionOS);
                o.positionWS = worldPos.xyz;
                o.positionHCS = UnityObjectToClipPos(v.positionOS);
                o.normalWS = UnityObjectToWorldNormal(v.normalOS);
                o.uv = v.uv;
                o.screenPos = o.positionHCS;
                return o;
            }

            static const float bayer4x4[16] = {
                0.0/16.0,  8.0/16.0,  2.0/16.0, 10.0/16.0,
                12.0/16.0, 4.0/16.0, 14.0/16.0, 6.0/16.0,
                3.0/16.0, 11.0/16.0, 1.0/16.0, 9.0/16.0,
                15.0/16.0, 7.0/16.0, 13.0/16.0, 5.0/16.0
            };

            float DitherPattern(float4 screenPos)
            {
                float2 ndc = (screenPos.xy / max(screenPos.w, 1e-6)) * 0.5 + 0.5;
                float2 pixelPos = ndc * _ScreenParams.xy;
                int xi = (int)fmod(floor(pixelPos.x), 4.0);
                int yi = (int)fmod(floor(pixelPos.y), 4.0);
                int idx = (xi + yi * 4) % 16;
                return bayer4x4[idx];
            }

            // Simple lighting based on Unity built-ins (_WorldSpaceLightPos0, _LightColor0)
            float3 SimpleLighting(float3 normalWS, float3 positionWS, float3 albedo)
            {
                float3 N = normalize(normalWS);
                float3 V = normalize(_WorldSpaceCameraPos - positionWS);

                // directional light: _WorldSpaceLightPos0 (legacy) — may be directional or positional
                float3 lightVec = _WorldSpaceLightPos0.xyz;
                // if directional, Unity gives direction as (x,y,z) pointing towards light; for legacy flip.
                float3 L = normalize(-lightVec);

                float NdotL = saturate(dot(N, L));

                // get unity primary light color
                float3 mainLightCol = _LightColor0.rgb;

                float3 diffuse = mainLightCol * NdotL;

                // Blinn-Phong specular approximation
                float3 H = normalize(L + V);
                float specPow = max(1.0, _Smoothness * 128.0);
                float spec = pow(saturate(dot(N, H)), specPow);
                float3 specular = _SpecularColor.rgb * spec;

                float3 ambient = _AmbientColor.rgb;

                return albedo * (ambient + diffuse) + specular;
            }

            half4 frag(Varyings i) : SV_Target
            {
                // Cutoff logic: visible below cutoff, invisible above
                float lower = _CutoffHeight - _CutoffFeather;
                float upper = _CutoffHeight + _CutoffFeather;
                float alphaLinear = 1.0 - smoothstep(lower, upper, i.positionWS.y);

                // Dither threshold near cutoff
                float threshold = DitherPattern(i.screenPos) * _DitherStrength;
                if (alphaLinear <= threshold) discard;

                // Sample base color (use tex2D directly)
                float4 baseCol = tex2D(_BaseMap, i.uv) * _BaseColor;

                // Simple lighting
                float3 lit = SimpleLighting(i.normalWS, i.positionWS, baseCol.rgb);

                // Apply global transparency as multiplicative factor to alphaLinear
                float finalAlpha = saturate((1.0 - _Transparency) * alphaLinear);

                // Slight gamma correction / softening for plastic feel
                lit = pow(max(lit, 0.0), 0.85);

                return half4(lit, finalAlpha);
            }
            ENDHLSL
        }
    }

    FallBack Off
}