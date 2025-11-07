Shader "Custom/EnemyInstancedFromBuffer"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Main Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
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
            float4 _Color;

            struct appdata {
                float3 vertex : POSITION;
                float2 uv : TEXCOORD0;
                uint instanceID : SV_InstanceID;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
                nointerpolation uint iid : TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;
                // read position for this instance
                uint id = v.instanceID;
                float3 pos = enemies[id].position;

                float3 worldPos = v.vertex + pos;
                o.pos = UnityObjectToClipPos(float4(worldPos, 1.0));
                o.uv = v.uv;
                o.iid = id;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                uint id = i.iid;
                // if inactive, discard
                if (enemies[id].active == 0)
                    discard;

                fixed4 texColor = tex2D(_MainTex, i.uv);
                return texColor * _Color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}