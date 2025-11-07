// Shader "Custom/EnemyInstancedFromBuffer"
// {
//     Properties
//     {
//         _Color ("Color", Color) = (1,1,1,1)
//         _MainTex ("Main Texture", 2D) = "white" {}
//     }
//     SubShader
//     {
//         Tags { "RenderType"="Opaque" }
//         Pass
//         {
//             CGPROGRAM
//             #pragma vertex vert
//             #pragma fragment frag
//             #pragma multi_compile_instancing
//             #include "UnityCG.cginc"

//             struct EnemyData
//             {
//                 float3 position;
//                 float3 velocity;
//                 float health;
//                 float padding;
//                 int active;
//                 int _pad1;
//                 int _pad2;
//                 int _pad3;
//             };
//             StructuredBuffer<EnemyData> enemies;

//             sampler2D _MainTex;
//             float4 _Color;

//             struct appdata {
//                 float3 vertex : POSITION;
//                 float2 uv : TEXCOORD0;
//                 uint instanceID : SV_InstanceID;
//             };

//             struct v2f {
//                 float4 pos : SV_POSITION;
//                 float2 uv  : TEXCOORD0;
//                 nointerpolation uint iid : TEXCOORD1;
//             };

//             v2f vert(appdata v)
//             {
//                 v2f o;
//                 // read position for this instance
//                 uint id = v.instanceID;
//                 float3 pos = enemies[id].position;

//                 float3 worldPos = v.vertex + pos;
//                 o.pos = UnityObjectToClipPos(float4(worldPos, 1.0));
//                 o.uv = v.uv;
//                 o.iid = id;
//                 return o;
//             }

//             fixed4 frag(v2f i) : SV_Target
//             {
//                 uint id = i.iid;
//                 // if inactive, discard
//                 if (enemies[id].active == 0)
//                     discard;

//                 fixed4 texColor = tex2D(_MainTex, i.uv);
//                 return texColor * _Color;
//             }
//             ENDCG
//         }
//     }
//     FallBack "Diffuse"
// }
// Shader "Custom/EnemyInstancedFromBuffer_Emission"
// {
//     Properties
//     {
//         _MainTex ("Main Texture", 2D) = "white" {}
//         _EmissionIntensity ("Emission Intensity", Range(0, 10)) = 1.0
//     }

//     SubShader
//     {
//         Tags { "RenderType"="Opaque" "Queue"="Geometry" }
//         LOD 100

//         Pass
//         {
//             Name "FORWARD"
//             Tags { "LightMode"="UniversalForward" }

//             ZWrite On
//             Cull Back
//             Blend One Zero // <- pas de transparence

//             HLSLPROGRAM
//             #pragma vertex vert
//             #pragma fragment frag
//             #pragma multi_compile_instancing
//             #include "UnityCG.cginc"

//             struct EnemyData
//             {
//                 float3 position;
//                 float3 velocity;
//                 float health;
//                 float padding;
//                 int active;
//                 int _pad1;
//                 int _pad2;
//                 int _pad3;
//             };
//             StructuredBuffer<EnemyData> enemies;

//             sampler2D _MainTex;
//             float _EmissionIntensity;

//             struct appdata
//             {
//                 float3 vertex : POSITION;
//                 float2 uv : TEXCOORD0;
//                 uint instanceID : SV_InstanceID;
//             };

//             struct v2f
//             {
//                 float4 pos : SV_POSITION;
//                 float2 uv : TEXCOORD0;
//                 nointerpolation uint iid : TEXCOORD1;
//             };

//             v2f vert(appdata v)
//             {
//                 v2f o;
//                 uint id = v.instanceID;
//                 float3 pos = enemies[id].position;

//                 float3 worldPos = v.vertex + pos;
//                 o.pos = UnityObjectToClipPos(float4(worldPos, 1.0));
//                 o.uv = v.uv;
//                 o.iid = id;
//                 return o;
//             }

//             fixed4 frag(v2f i) : SV_Target
//             {
//                 uint id = i.iid;
//                 if (enemies[id].active == 0)
//                     discard;

//                 fixed4 texColor = tex2D(_MainTex, i.uv);
//                 texColor.rgb *= _EmissionIntensity;

//                 // Forcer alpha = 1 pour éviter toute transparence
//                 texColor.a = 1;

//                 return texColor;
//             }
//             ENDHLSL
//         }
//     }

//     FallBack "Unlit/Texture"
// }
Shader "Custom/EnemyInstancedFromBuffer_Emission_Billboard"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _EmissionIntensity ("Emission Intensity", Range(0, 10)) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 100

        Pass
        {
            Name "FORWARD"
            Tags { "LightMode"="UniversalForward" }

            ZWrite On
            Cull Off
            Blend One Zero

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"

            struct EnemyData
            {
                float3 position;
                float3 velocity;
                float health;
                float padding;
                int active;
                int _pad1;
                int _pad2;
                int _pad3;
            };
            StructuredBuffer<EnemyData> enemies;

            sampler2D _MainTex;
            float _EmissionIntensity;
            float3 _PlayerPosition; // <-- on passera cette valeur depuis C#

            struct appdata
            {
                float3 vertex : POSITION;
                float2 uv : TEXCOORD0;
                uint instanceID : SV_InstanceID;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                nointerpolation uint iid : TEXCOORD1;
            };

            v2f vert(appdata v)
            {

                // v2f o;
                // uint id = v.instanceID;
                // float3 pos = enemies[id].position;

                // // vecteur direction XZ vers le player
                // float3 dir = _PlayerPosition - pos;
                // dir.y = 0;

                // // fallback si le player est exactement dessus
                // float len = length(dir);
                // float angleY = 0;
                // if (len > 0.0001)
                // {
                //     // atan2(X,Z) pour obtenir un angle robuste selon tous les quadrants
                //     angleY = atan2(dir.x, dir.z);
                // }

                // // construire matrice de rotation Y manuellement
                // float cosA = cos(angleY);
                // float sinA = sin(angleY);

                // float3 right = -float3(cosA, 0, -sinA);
                // float3 forward = float3(sinA, 0, cosA);
                // float3 up = float3(0,1,0);

                // float3x3 rot = float3x3(right, up, forward);

                // float3 worldPos = mul(rot, v.vertex) + pos;

                // o.pos = UnityObjectToClipPos(float4(worldPos, 1.0));
                // o.uv = v.uv;
                // o.iid = id;
                // return o;
                
                v2f o;
                uint id = v.instanceID;
                float3 pos = enemies[id].position;
            
                // --- Billboard face au player ---
                float3 toPlayer = normalize(_PlayerPosition - pos);
                float3 up = float3(0, 1, 0);
            
                // Correction : inverser l'ordre du cross pour un repère droit
                float3 right = normalize(cross(toPlayer, up));
                float3 forward = normalize(cross(up, right));
            
                // matrice de rotation billboard
                float3x3 rot = float3x3(right, up, forward);
            
                // rotation des sommets
                float3 worldPos = mul(rot, v.vertex) + pos;
                
                o.pos = UnityObjectToClipPos(float4(worldPos, 1.0));
                o.uv = v.uv;
                o.iid = id;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                uint id = i.iid;
                if (enemies[id].active == 0)
                    discard;

                fixed4 texColor = tex2D(_MainTex, i.uv);
                texColor.rgb *= _EmissionIntensity;
                texColor.a = 1;
                return texColor;
            }
            ENDHLSL
        }
    }

    FallBack "Unlit/Texture"
}