using UnityEngine;

namespace Game
{
    public abstract class BuildingStruct : MonoBehaviour
    {
        #region properties
        [SerializeField] protected GameObject m_exteriorVisual;
        public GameObject ExteriorVisual => m_exteriorVisual;
        #endregion
        #region properties
        [SerializeField] protected GameObject m_interiorVisual;
        public GameObject InteriorVisual => m_interiorVisual;
        #endregion

        public void Setup()
        {

        }
        public void Init()
        {

        }
    }
}
