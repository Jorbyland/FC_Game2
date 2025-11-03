using UnityEngine;

namespace Game
{
    public class Building_TriggerManagerComponent : MonoBehaviour
    {
        private Building m_building;
        private BuildingFloor_TriggerComponent[] m_floorTriggers;

        private bool m_isInside;
        private int m_currentFloor = -1;

        public bool IsInside => m_isInside;
        public int CurrentFloor => m_currentFloor;

        public void Setup(Building building)
        {
            m_building = building;
        }

        public void Init()
        {
            m_isInside = false;
            m_currentFloor = -1;
        }

        public void OnPlayerEnterFloor(int a_floorIndex)
        {
            if (!m_isInside)
            {
                m_isInside = true;
                OnEnterBuilding();
                OnChangeFloor(a_floorIndex);
            }

            if (m_currentFloor != a_floorIndex)
            {
                m_currentFloor = a_floorIndex;
                OnChangeFloor(a_floorIndex);
            }
        }

        public void OnPlayerExitFloor(int a_floorIndex)
        {
            // Si le joueur sort du bâtiment (plus aucun trigger actif)
            if (m_isInside && a_floorIndex == m_currentFloor)
            {
                m_isInside = false;
                m_currentFloor = -1;
                OnExitBuilding();
            }
        }

        private void OnEnterBuilding()
        {
            m_building.ShowInterior(true);
            CameraManager.Instance.SwitchToInterior();
            CameraManager.Instance.ObstacleDetector.SetCurrentBuilding(m_building);
        }

        private void OnExitBuilding()
        {
            m_building.ShowInterior(false);
            CameraManager.Instance.SwitchToExterior();
            CameraManager.Instance.ObstacleDetector.SetCurrentBuilding(null);
        }

        private void OnChangeFloor(int newFloorIndex)
        {
            m_building.UpdateVisibleFloor(newFloorIndex);
        }

        #region Editor
        [ContextMenu("Generate Floor Triggers")]
        public void GenerateFloorTriggers()
        {
            BuildingFloor[] floors = GetComponentsInChildren<BuildingFloor>();
            foreach (var floor in floors)
            {
                if (floor == null) continue;
                floor.GenerateBoundingTrigger();
            }
        }
        #endregion
    }
}