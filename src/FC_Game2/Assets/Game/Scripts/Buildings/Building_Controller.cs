using UnityEngine;

namespace Game
{
    public class Building_Controller : MonoBehaviour
    {
        #region properties
        private Building m_building;
        #endregion

        public void Setup(Building a_building)
        {
            m_building = a_building;
        }
        public void Init()
        {

        }
    }
}
