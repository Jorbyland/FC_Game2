Shader "Game/HeightCutoffToon"
{
    Properties
    {
        _MainTex ("Albedo", 2D) = "white" {}
        _ColorTint("Tint", Color) = (1,1,1,1)

        _NormalNoise("Normal Noise (RGB)", 2D) = "white" {}
        _NormalStrength("Normal Strength", Range(0,1)) = 0.08

        _ProcLightDir("Proc Light Direction", Vector) = (0.3,0.8,-0.6,0)
        _ProcLightColor("Proc Light Color", Color) = (1,0.95,0.9,1)
        _ProcLightIntensity("Proc Light Intensity", Float) = 1.0
        _ProcAmbientColor("Proc Ambient Color", Color) = (0.25,0.30,0.35,1)
        _ProcAmbientIntensity("Proc Ambient Intensity", Float) = 0.35

        _ToonRamp("Toon Ramp (1D)", 2D) = "white" {}
        _UseToonRamp("Use Toon Ramp", Float) = 1
        _ToonSteps("Toon Steps (fallback)", Range(1,8)) = 4

        _RimPower("Rim Power", Float) = 3.0
        _RimIntensity("Rim Intensity", Float) = 0.12

        _CutoffHeight("Cutoff Height (world Y)", Float) = 0
        _CutoffFeather("Cutoff Feather", Float) = 0.15

        _DitherStrength("Dither Strength (0..1)", Range(0,1)) = 1.0

        _AlphaClipThreshold("Alpha Clip Threshold (fallback)", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="TransparentCutout" "Queue"="AlphaTest" }
        LOD 200

        Pass
        {
            Name "FORWARD"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "UnityCG.cginc"

            // Properties (accessors)
            sampler2D _MainTex;
            float4 _ColorTint;

            sampler2D _NormalNoise;
            float _NormalStrength;

            float4 _ProcLightDir;
            float4 _ProcLightColor;
            float _ProcLightIntensity;
            float4 _ProcAmbientColor;
            float _ProcAmbientIntensity;

            sampler2D _ToonRamp;
            float _UseToonRamp;
            float _ToonSteps;

            float _RimPower;
            float _RimIntensity;

            float _CutoffHeight;
            float _CutoffFeather;

            float _DitherStrength;
            float _AlphaClipThreshold;


            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv     : TEXCOORD0;
                float4 color  : COLOR; // optional
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float2 uv : TEXCOORD2;
                float4 screenPos : TEXCOORD3;
                float3 viewDir : TEXCOORD4;
            };

            v2f Vert(appdata v)
            {
                v2f o;
                float4 worldPos4 = mul(unity_ObjectToWorld, v.vertex);
                o.worldPos = worldPos4.xyz;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                // transform normal to world
                o.worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
                o.screenPos = o.pos;
                float3 camWorldPos = _WorldSpaceCameraPos;
                o.viewDir = normalize(camWorldPos - o.worldPos);
                return o;
            }

            // Bayer 4x4 matrix (values in range 0..1)
            static const float bayer[16] = {
                0.0/16.0,  8.0/16.0,  2.0/16.0, 10.0/16.0,
                12.0/16.0, 4.0/16.0, 14.0/16.0, 6.0/16.0,
                3.0/16.0, 11.0/16.0, 1.0/16.0, 9.0/16.0,
                15.0/16.0, 7.0/16.0, 13.0/16.0, 5.0/16.0
            };

            // sample palette helper not used here, leave for future

            // small noise normal (world-space perturbation)
            float3 SampleNormalNoise(float2 uv)
            {
                float3 n = tex2D(_NormalNoise, uv).rgb * 2.0 - 1.0; // -1..1
                return n;
            }

            // compute screen pixel coordinates from clip pos
            float2 ScreenPixelCoords(float4 clipPos)
            {
                // clipPos is SV_POSITION (already in clip space)
                // Convert to NDC then to pixel:
                float2 ndc = clipPos.xy / clipPos.w;
                float2 uv = ndc * 0.5 + 0.5;
                float2 pixels = uv * _ScreenParams.xy;
                return pixels;
            }

            float DitherThreshold(float4 screenPos)
            {
                float2 px = ScreenPixelCoords(screenPos);
                // floor to integer
                float fx = fmod(floor(px.x), 4.0);
                float fy = fmod(floor(px.y), 4.0);
                int ix = (int)fx + (int)fy * 4;
                // clamp
                ix = ix % 16;
                return bayer[ix];
            }

            // sample toon ramp: expects a 1D ramp along U axis
            float3 SampleToonRamp(float nDotL)
            {
                if (_UseToonRamp > 0.5)
                {
                    float u = saturate(nDotL);
                    float2 uv = float2(u, 0.5);
                    return tex2D(_ToonRamp, uv).rgb;
                }
                else
                {
                    // fallback quantized steps
                    float steps = max(1, _ToonSteps);
                    float q = floor(saturate(nDotL) * steps) / steps;
                    return float3(q,q,q);
                }
            }

            fixed4 Frag(v2f i) : SV_Target
            {
                // base color
                float4 albedo = tex2D(_MainTex, i.uv) * _ColorTint;

                // world normal with tiny noise perturbation
                float3 N = normalize(i.worldNormal);
                float3 noiseN = SampleNormalNoise(i.uv) * _NormalStrength;
                N = normalize(N + noiseN);

                // procedural light direction (world)
                float3 L = normalize(_ProcLightDir.xyz);
                float3 lightCol = _ProcLightColor.rgb;
                float mainIntensity = saturate(dot(N, L));

                // toon ramp or quantized lighting
                float3 toon = SampleToonRamp(mainIntensity);

                // main light color applied to toon (treat toon as intensity mask)
                float3 mainLight = toon * lightCol * _ProcLightIntensity;

                // ambient
                float3 ambient = _ProcAmbientColor.rgb * _ProcAmbientIntensity;

                // rim
                float rimFactor = pow(1.0 - saturate(dot(i.viewDir, N)), _RimPower) * _RimIntensity;
                float3 rim = rimFactor * lightCol;

                // compose lighting
                float3 finalLighting = mainLight + ambient + rim;

                // final color
                float3 color = albedo.rgb * finalLighting;

                // Cutoff based on height: produce alpha (1 visible, 0 hidden)
                float lower = _CutoffHeight - _CutoffFeather;
                float upper = _CutoffHeight + _CutoffFeather;
                // alpha = 1 when world y <= lower, 0 when above upper
                float alphaLinear = 1.0 - smoothstep(lower, upper, i.worldPos.y);

                // Dither
                float threshold = DitherThreshold(i.screenPos) * _DitherStrength;
                // combine with alphaLinear to compute final clip decision
                // if alphaLinear <= threshold => clip (i.e. hidden)
                // But to keep a baseline fallback, also include _AlphaClipThreshold
                float effectiveAlpha = alphaLinear;
                // do clip using Bayer pattern
                if (effectiveAlpha <= threshold && _DitherStrength > 0.001)
                {
                    clip(-1); // discard fragment
                }
                else
                {
                    // final alpha (keep opaque for lighting, but we respect cutoff visually)
                    // set alpha to 1 for visible, 0 for hidden (for some systems)
                    if (effectiveAlpha < 0.5 && _DitherStrength <= 0.001)
                    {
                        // fallback discrete clip
                        clip(effectiveAlpha - _AlphaClipThreshold);
                    }
                }

                // output
                return float4(color, 1.0);
            }

            ENDHLSL
        } // Pass
    } // SubShader

    FallBack "Diffuse"
}