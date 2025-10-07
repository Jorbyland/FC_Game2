using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools
{
    public class ObjectPooler
    {
        private int m_maxPoolSize;
        private Transform m_parentFolder;
        private GameObject m_prefab;
        private List<GameObject> m_pool;
        public List<GameObject> VisibleItems { get; private set; }
        private bool m_useReturnToPool;

        public ObjectPooler(GameObject a_prefab, int a_poolSize, Transform a_parentFolder, int a_maxPoolSize = int.MaxValue)
        {
            m_maxPoolSize = a_maxPoolSize;
            m_prefab = a_prefab;
            m_parentFolder = a_parentFolder;
            m_useReturnToPool = true;
            m_pool = new List<GameObject>();
            for (int i = 0; i < a_poolSize; i++)
            {
                CreateItem();
            }
            VisibleItems = new List<GameObject>();
        }
        public GameObject GetItem()
        {
            if (m_pool.Count == 0)
            {
                if (VisibleItems.Count < m_maxPoolSize)
                {
                    CreateItem();
                }
                else
                {
                    VisibleItems[0].gameObject.SetActive(false);
                }
            }
            GameObject instance = m_pool[0];
            m_pool.RemoveAt(0);
            VisibleItems.Add(instance);
            return instance;
        }

        public void DisableAllElements()
        {
            m_useReturnToPool = false;
            for (int i = 0; i < VisibleItems.Count; i++)
            {
                VisibleItems[i].SetActive(false);
                m_pool.Add(VisibleItems[i]);
            }
            VisibleItems = new List<GameObject>();
            m_useReturnToPool = true;
        }


        private void CreateItem()
        {
            GameObject instance = GameObject.Instantiate(m_prefab);
            instance.transform.SetParent(m_parentFolder, false);
            instance.gameObject.SetActive(false);
            instance.AddComponent<ReturnToPoolOnDisable>().Init(ReturnToPool);
            m_pool.Add(instance);
        }

        private void ReturnToPool(GameObject a_item)
        {
            if (m_useReturnToPool)
            {
                a_item.gameObject.SetActive(false);
                m_pool.Add(a_item);
                VisibleItems.Remove(a_item);
            }
        }


    }
}
