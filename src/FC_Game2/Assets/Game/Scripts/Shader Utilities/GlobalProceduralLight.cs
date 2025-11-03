using UnityEngine;

[ExecuteAlways]
public class GlobalProceduralLight : MonoBehaviour
{
    [SerializeField] private Vector3 lightDirection = new Vector3(0.3f, 0.8f, -0.6f);
    [SerializeField] private Color lightColor = Color.white;
    [SerializeField] private float lightIntensity = 1f;
    [SerializeField] private Color ambientColor = new Color(0.3f, 0.35f, 0.4f);
    [SerializeField] private float ambientIntensity = 0.5f;
    [SerializeField] private float rimPower = 3f;
    [SerializeField] private float rimIntensity = 0.6f;

    private static readonly int DirID = Shader.PropertyToID("_ProcLightDirection");
    private static readonly int ColID = Shader.PropertyToID("_ProcLightColor");
    private static readonly int IntID = Shader.PropertyToID("_ProcLightIntensity");
    private static readonly int AmbColID = Shader.PropertyToID("_ProcAmbientColor");
    private static readonly int AmbIntID = Shader.PropertyToID("_ProcAmbientIntensity");
    private static readonly int RimPowID = Shader.PropertyToID("_ProcRimPower");
    private static readonly int RimIntID = Shader.PropertyToID("_ProcRimIntensity");

    void Update()
    {
        Shader.SetGlobalVector(DirID, lightDirection.normalized);
        Shader.SetGlobalColor(ColID, lightColor);
        Shader.SetGlobalFloat(IntID, lightIntensity);
        Shader.SetGlobalColor(AmbColID, ambientColor);
        Shader.SetGlobalFloat(AmbIntID, ambientIntensity);
        Shader.SetGlobalFloat(RimPowID, rimPower);
        Shader.SetGlobalFloat(RimIntID, rimIntensity);
    }
}