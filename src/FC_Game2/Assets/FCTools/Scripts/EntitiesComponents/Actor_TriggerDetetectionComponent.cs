using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace FCTools.EntitiesComponents
{
    [RequireComponent(typeof(Collider))]
    public class Actor_TriggerDetetectionComponent : MonoBehaviour
    {
        public delegate void OnTriggerDelegate(Entity a_entity);

        public OnTriggerDelegate onTriggerEnterDelegate;
        public OnTriggerDelegate onTriggerExitDelegate;

        public List<GameObject> ObjectsInRange => m_objectInRange;
        private List<GameObject> m_objectInRange = new List<GameObject>();

        public bool HaveObjectInRange { get { return m_objectInRange.Count > 0; } }

        public void Setup()
        {

        }
        public void Init()
        {
            m_objectInRange = new List<GameObject>();
        }

        void OnTriggerEnter(Collider a_other)
        {
            if (!m_objectInRange.Contains(a_other.gameObject))
            {
                m_objectInRange.Add(a_other.gameObject);
                Entity entity = a_other.gameObject.GetComponent<Entity>();
                if (entity != null)
                {
                    onTriggerEnterDelegate?.Invoke(entity);
                }
            }
        }

        void OnTriggerExit(Collider a_other)
        {
            if (m_objectInRange.Contains(a_other.gameObject))
            {
                m_objectInRange.Remove(a_other.gameObject);

                Entity entity = a_other.gameObject.GetComponent<Entity>();
                if (entity != null)
                {
                    onTriggerExitDelegate?.Invoke(entity);
                }
            }
        }
    }
}
