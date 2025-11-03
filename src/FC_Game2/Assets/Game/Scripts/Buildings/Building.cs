using UnityEngine;

namespace Game
{
    public class Building : MonoBehaviour
    {
        #region inspector
        private Building_TriggerManagerComponent m_triggerManagerComponent;
        public Building_TriggerManagerComponent TriggerManagerComponent => m_triggerManagerComponent;
        private Building_PartsComponent m_buildingPartsComponent;
        public Building_PartsComponent BuildingPartsComponent => m_buildingPartsComponent;
        private Building_Controller m_buildingController;
        public Building_Controller Building_Controller => m_buildingController;
        #endregion
        #region properties
        #endregion

        private void Start()
        {
            m_triggerManagerComponent = GetComponent<Building_TriggerManagerComponent>();
            m_buildingPartsComponent = GetComponent<Building_PartsComponent>();
            m_buildingController = GetComponent<Building_Controller>();

            m_triggerManagerComponent.Setup(this);
            m_buildingPartsComponent.Setup(this);
            m_buildingController.Setup(this);

            m_triggerManagerComponent.Init();
            m_buildingPartsComponent.Init();
            m_buildingController.Init();
        }

        public void Hide(bool a_hide)
        {
            m_buildingPartsComponent.UpdateVisibility(a_hide);
        }
        public void ShowInterior(bool active)
        {
            m_buildingPartsComponent.UpdateInsideVisibility(!active);
        }

        public void UpdateVisibleFloor(int a_floorIndex)
        {
            m_buildingPartsComponent.UpdateFloorVisibility(a_floorIndex);
            m_buildingPartsComponent.UpdateInsideVisibilityByFloorIndex(a_floorIndex);
        }

    }
}