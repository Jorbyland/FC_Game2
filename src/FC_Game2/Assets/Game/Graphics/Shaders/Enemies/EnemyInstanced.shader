Shader "Custom/EnemyInstanced"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
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
            #include "UnityCG.cginc" // ✅ ajoute les macros UNITY_INSTANCING_BUFFER

            StructuredBuffer<float3> enemies;
            UNITY_INSTANCING_BUFFER_START(Props)
            UNITY_INSTANCING_BUFFER_END(Props)

            fixed4 _Color;

            struct appdata
            {
                float3 vertex : POSITION;
                uint instanceID : SV_InstanceID;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                float3 pos = enemies[v.instanceID];
                o.pos = UnityObjectToClipPos(float4(v.vertex + pos, 1.0));
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return _Color;
            }
            ENDCG
        }
    }
}