using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BuildingFloor : MonoBehaviour
    {
        public const float CUTOFF_SIZE = 0.8f;
        #region inspector
        [SerializeField] private GameObject m_floorContent;
        public GameObject FloorContent => m_floorContent;
        [SerializeField] private GameObject m_outsideContent;
        public GameObject OutsideContent => m_outsideContent;
        [SerializeField] private GameObject m_insideContent;
        public GameObject InsideContent => m_insideContent;
        [SerializeField] private GameObject m_otherContent;
        public GameObject OtherContent => m_otherContent;
        [SerializeField] private BuildingFloor_TriggerComponent m_triggerComponent;
        public BuildingFloor_TriggerComponent TriggerComponent => m_triggerComponent;

        public float positionY;

        [Header("Editor")]
        [SerializeField] private bool m_useManualCreation = false;
        [SerializeField] private List<MeshRenderer> m_renderersForVolume;
        #endregion

        public void Setup(Building_TriggerManagerComponent a_manager, int a_floorIndex)
        {
            m_triggerComponent.Setup(a_manager, a_floorIndex);
        }

        public float GetFloorCutoffHeight()
        {
            return positionY + CUTOFF_SIZE;
        }

        public void ActiveInsideElements(bool a_active)
        {
            m_insideContent.gameObject.SetActive(a_active);
        }

        #region Editor
        public void SetFloorContent(GameObject a_content)
        {
            m_floorContent = a_content;
        }
        public void SetOutsideContent(GameObject a_content)
        {
            m_outsideContent = a_content;
        }
        public void SetInsideContent(GameObject a_content)
        {
            m_insideContent = a_content;
        }
        public void SetOtherContent(GameObject a_content)
        {
            m_otherContent = a_content;
        }
        public void SetTriggerComponent(BuildingFloor_TriggerComponent a_triggerComponent)
        {
            m_triggerComponent = a_triggerComponent;
        }



        public void GenerateBoundingTrigger()
        {
            if (m_useManualCreation)
            {
                if (m_renderersForVolume == null || m_renderersForVolume.Count == 0)
                {
                    return;
                }
                GenerateBoundingBoxTriggerBySelection(m_renderersForVolume);
            }
            else
            {
                GenerateBoundingBoxTrigger(new List<MeshRenderer>(GetComponentsInChildren<MeshRenderer>()));
            }
        }

        public void ClearOldTriggers()
        {
            Transform root = FindTriggerRoot();

            if (root == null) return;

            for (int i = root.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(root.GetChild(i).gameObject);
            }
        }

        private void GenerateBoundingBoxTrigger(List<MeshRenderer> a_renderers)
        {
            ClearOldTriggers();

            Transform root = FindTriggerRoot();
            root = root != null ? root : transform;

            Bounds bounds = a_renderers[0].bounds;
            foreach (var r in a_renderers)
                bounds.Encapsulate(r.bounds);

            GameObject trigger = new GameObject("Trigger_Auto");
            trigger.transform.SetParent(root);
            trigger.transform.position = bounds.center;
            trigger.layer = LayerMask.NameToLayer("InsideTrigger");

            BoxCollider box = trigger.AddComponent<BoxCollider>();
            box.isTrigger = true;
            box.size = bounds.size;
        }
        private void GenerateBoundingBoxTriggerBySelection(List<MeshRenderer> a_renderers)
        {
            Transform root = FindTriggerRoot();
            root = root != null ? root : transform;

            Bounds bounds = a_renderers[0].bounds;
            foreach (var r in a_renderers)
                bounds.Encapsulate(r.bounds);

            GameObject trigger = new GameObject("Trigger_Auto");
            trigger.transform.SetParent(root);
            trigger.transform.position = bounds.center;
            trigger.layer = LayerMask.NameToLayer("InsideTrigger");

            BoxCollider box = trigger.AddComponent<BoxCollider>();
            box.isTrigger = true;
            box.size = bounds.size;
        }
        private Transform FindTriggerRoot()
        {
            return transform.Find("Triggers");
        }

        #endregion
    }
}
