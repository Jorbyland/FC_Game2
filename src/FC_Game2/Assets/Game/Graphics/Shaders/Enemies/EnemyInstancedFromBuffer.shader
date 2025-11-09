Shader "Custom/EnemyInstancedFromBuffer_Emission_Billboard"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _EmissionIntensity ("Emission Intensity", Range(0, 10)) = 1.0
        [Toggle] _UseBuffer ("Use Buffer (GPU Instanced)", Float) = 1.0
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
            #pragma multi_compile __ _USEBUFFER_ON
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
            float _UseBuffer;

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
                nointerpolation float useBuffer : TEXCOORD2;
            };

            v2f vert(appdata v)
            {
                v2f o;
                float3 pos;
                
                #ifdef _USEBUFFER_ON
                // Mode GPU instancié : utiliser le buffer
                uint id = v.instanceID;
                pos = enemies[id].position;
                o.iid = id;
                o.useBuffer = 1.0;
                #else
                // Mode CPU normal : utiliser la position du transform (seulement la position, pas la rotation)
                // On extrait seulement la position de la matrice de transformation
                float4x4 objectToWorld = unity_ObjectToWorld;
                pos = float3(objectToWorld._m03, objectToWorld._m13, objectToWorld._m23);
                o.iid = 0;
                o.useBuffer = 0.0;
                #endif
            
                // --- Billboard face au player ---
                float3 toPlayer = _PlayerPosition - pos;
                float distToPlayer = length(toPlayer);
                float3 dirToPlayer = (distToPlayer > 0.001) ? normalize(toPlayer) : float3(0, 0, 1);
                
                float3 up = float3(0, 1, 0);
            
                // Correction : inverser l'ordre du cross pour un repère droit
                float3 right = normalize(cross(dirToPlayer, up));
                // Si right est trop petit, utiliser une direction par défaut
                if (length(right) < 0.001)
                {
                    right = float3(1, 0, 0);
                }
                right = normalize(right);
                
                float3 forward = normalize(cross(up, right));
            
                // matrice de rotation billboard
                float3x3 rot = float3x3(right, up, forward);
            
                // rotation des sommets et transformation en espace monde
                float3 worldPos = mul(rot, v.vertex) + pos;
                
                // Transformation en espace clip
                o.pos = mul(UNITY_MATRIX_VP, float4(worldPos, 1.0));
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                #ifdef _USEBUFFER_ON
                // Mode GPU instancié : vérifier active
                uint id = i.iid;
                if (enemies[id].active == 0)
                    discard;
                #endif
                // Mode CPU : toujours afficher

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