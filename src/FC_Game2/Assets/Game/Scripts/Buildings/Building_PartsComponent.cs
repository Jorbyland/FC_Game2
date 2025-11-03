using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Building_PartsComponent : MonoBehaviour
    {
        [System.Serializable]
        public class FloorRef
        {
            public GameObject[] OutdoorWallsOutsideVisual;
            public GameObject[] OutdoorWallsInsideVisual;
            public Transform IndoorWallsVisualContent;
            public Transform OtherVisualContent;
            public Transform FloorsContent;
        }
        #region inspector
        [Header("Requiered References")]
        [SerializeField] private BuildingFloor[] m_floors;
        [Header("Auto assign References")]
        [SerializeField] private BuildingStruct[] m_buildingStructs;
        #endregion
        #region properties
        private Building m_building;
        #endregion

        public void Setup(Building a_building)
        {
            m_building = a_building;

            for (int i = 0; i < m_floors.Length; i++)
            {
                m_floors[i].Setup(m_building.TriggerManagerComponent, i);
            }
        }
        public void Init()
        {

        }
        public void UpdateVisibility(bool a_hide)
        {
            for (int i = 0; i < m_buildingStructs.Length; i++)
            {
                m_buildingStructs[i].SetVisibility(a_hide);
            }
        }

        public void UpdateInsideVisibility(bool a_visible)
        {
            if (a_visible)
            {
                for (int i = 0; i < m_floors.Length; i++)
                {
                    m_floors[i].InsideContent.SetActive(false);
                    m_floors[i].OtherContent.SetActive(false);
                }
                for (int i = 0; i < m_buildingStructs.Length; i++)
                {
                    m_buildingStructs[i].SetCutOffHeight();
                }
            }
            else
            {
                for (int i = 0; i < m_floors.Length; i++)
                {
                    m_floors[i].InsideContent.SetActive(true);
                    m_floors[i].OtherContent.SetActive(true);
                }
            }
        }

        public void UpdateInsideVisibilityByFloorIndex(int a_visibleFloorIndex)
        {
            for (int i = 0; i < m_floors.Length; i++)
            {
                m_floors[i].InsideContent.SetActive(i == a_visibleFloorIndex);
            }
        }

        public void UpdateFloorVisibility(int a_floorIndex)
        {
            float cutOffHeight = m_floors[a_floorIndex].GetFloorCutoffHeight();
            for (int i = 0; i < m_buildingStructs.Length; i++)
            {
                m_buildingStructs[i].SetCutOffHeight(cutOffHeight);
            }
        }

        #region Editor
        public void SetFloor(BuildingFloor a_buildingFloor)
        {
            if (m_floors == null)
            {
                m_floors = new BuildingFloor[1];
                m_floors[0] = a_buildingFloor;
            }
            else
            {
                List<BuildingFloor> floors = m_floors.ToList();
                floors.Add(a_buildingFloor);
                m_floors = floors.ToArray();
            }
        }
        public void GetAllParts()
        {
            m_buildingStructs = FindAllBuildingStructs();
        }

        private BuildingStruct[] FindAllBuildingStructs()
        {
            List<BuildingStruct> result = new List<BuildingStruct>();
            for (int i = 0; i < m_floors.Length; i++)
            {
                result.AddRange(m_floors[i].FloorContent.GetComponentsInChildren<BuildingStruct>());
                result.AddRange(m_floors[i].OutsideContent.GetComponentsInChildren<BuildingStruct>());
                result.AddRange(m_floors[i].InsideContent.GetComponentsInChildren<BuildingStruct>());
                result.AddRange(m_floors[i].OtherContent.GetComponentsInChildren<BuildingStruct>());
            }
            return result.ToArray();
        }
        #endregion
    }
}
