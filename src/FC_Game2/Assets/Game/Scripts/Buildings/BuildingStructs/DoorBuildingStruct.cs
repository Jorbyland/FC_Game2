using UnityEngine;

namespace Game
{
    public class DoorBuildingStruct : BuildingStruct
    {
        #region inspector
        [SerializeField] protected MeshRenderer m_doorframeMeshRenderer;
        public MeshRenderer DoorMeshRenrerer => m_doorMeshRenderer;
        [SerializeField] protected MeshRenderer m_doorMeshRenderer;
        [SerializeField] protected Door m_door;

        #endregion

        public override void SetCutOffHeight(float a_value = 999)
        {
            base.SetCutOffHeight(a_value);
            a_value = a_value < 999 && Mathf.Abs(a_value - transform.position.y) < 1 ? 999 : a_value;
            m_doorframeMeshRenderer.material.SetFloat("_CutoffHeight", a_value);
            m_doorMeshRenderer.material.SetFloat("_CutoffHeight", a_value);
        }

        public override void SetVisibility(bool a_hide)
        {
            base.SetVisibility(a_hide);
            m_doorframeMeshRenderer.material.SetFloat("_Visibility", a_hide ? TRANSPARENT_VALUE : VISIBLE_VALUE);
            m_doorMeshRenderer.material.SetFloat("_Visibility", a_hide ? TRANSPARENT_VALUE : VISIBLE_VALUE);
        }
    }
}
