using UnityEngine;

namespace Game
{
    public class BuildingStruct : MonoBehaviour
    {
        public const float VISIBLE_VALUE = 1;
        public const float TRANSPARENT_VALUE = 1.53f;
        #region inspector
        [SerializeField] protected MeshRenderer m_structMeshRenderer;
        public MeshRenderer StructMeshRenderer => m_structMeshRenderer;
        #endregion
        #region properties

        #endregion

        public virtual void SetCutOffHeight(float a_value = 999)
        {
            m_structMeshRenderer.material.SetFloat("_CutoffHeight", a_value);
        }
        public virtual void SetVisibility(bool a_hide)
        {
            m_structMeshRenderer.material.SetFloat("_Visibility", a_hide ? TRANSPARENT_VALUE : VISIBLE_VALUE);
        }
    }
}
