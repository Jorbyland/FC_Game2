using UnityEngine;

namespace Game
{
    public class WindowBuildingStruct : BuildingStruct
    {
        #region inspector
        [SerializeField] protected MeshRenderer m_windowframeMeshRenderer;
        #endregion

        public override void SetCutOffHeight(float a_value = 999)
        {
            base.SetCutOffHeight(a_value);
            a_value = a_value < 999 && Mathf.Abs(a_value - transform.position.y) < 1 ? 999 : a_value;
            m_windowframeMeshRenderer.material.SetFloat("_CutoffHeight", a_value);
        }

        public override void SetVisibility(bool a_hide)
        {
            base.SetVisibility(a_hide);
            m_windowframeMeshRenderer.material.SetFloat("_Visibility", a_hide ? TRANSPARENT_VALUE : VISIBLE_VALUE);
        }
    }
}
