#pragma target 3.5
void ProceduralLighting_float(
    float3 WorldNormal,
    float3 ViewDir,
    float3 BaseColor,
    float3 LightDirection,
    float3 LightColor,
    float  LightIntensity,
    float3 AmbientColor,
    float  AmbientIntensity,
    float  RimPower,
    float  RimIntensity,
    out float3 OutColor)
{
    float3 N = normalize(WorldNormal);
    float3 L = normalize(LightDirection);
    float3 V = normalize(ViewDir);

    float NdotL = saturate(dot(N, L));
    float3 mainLight = LightColor * LightIntensity * NdotL;
    float3 ambient = AmbientColor * AmbientIntensity;
    float rim = pow(1.0 - saturate(dot(V, N)), RimPower) * RimIntensity;
    float3 lighting = mainLight + ambient + rim;
    OutColor = BaseColor * lighting;
}