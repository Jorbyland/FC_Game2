using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public class BuildingFloor_TriggerComponent : MonoBehaviour
    {
        #region properties

        private Building_TriggerManagerComponent m_manager;
        private int m_floorIndex;
        private readonly HashSet<Collider> m_inside = new();
        private bool m_playerInside;
        #endregion

        public void Setup(Building_TriggerManagerComponent manager, int floorIndex)
        {
            m_manager = manager;
            m_floorIndex = floorIndex;

            // Attacher automatiquement le proxy sur tous les colliders enfants
            foreach (var col in GetComponentsInChildren<Collider>())
            {
                if (!col.isTrigger) continue;

                var proxy = col.gameObject.GetComponent<BuildingFloor_TriggerProxy>();
                if (proxy == null)
                    proxy = col.gameObject.AddComponent<BuildingFloor_TriggerProxy>();

                proxy.Setup(this);
            }
        }

        public void NotifyTriggerEnter(Collider other)
        {
            m_inside.Add(other);

            if (!m_playerInside)
            {
                m_playerInside = true;
                OnPlayerEnter();
            }
        }

        public void NotifyTriggerExit(Collider other)
        {
            if (!m_inside.Contains(other)) return;
            m_inside.Remove(other);

            if (m_inside.Count == 0 && m_playerInside)
            {
                m_playerInside = false;
                OnPlayerExit();
            }
        }

        private void OnPlayerEnter()
        {
            m_manager?.OnPlayerEnterFloor(m_floorIndex);
        }

        private void OnPlayerExit()
        {
            m_manager?.OnPlayerExitFloor(m_floorIndex);
        }
    }
}
