using System.Collections.Generic;
using FCTools;
using UnityEngine;

namespace Game
{
    public class VFXManager : MonoBehaviour
    {
        [System.Serializable]
        public class VFX
        {
            public string id;
            public int poolSize = 100;
            public int maxPoolSize = int.MaxValue;
            public ParticleSystem particleSystem;
        }

        private static VFXManager s_instance;
        public static VFXManager Instance => s_instance;
        #region properties
        [SerializeField] private VFX[] m_vfxs;
        private Dictionary<string, ObjectPooler> m_vfxPools;

        [SerializeField] private BulletTrail m_bulletTrailPrefab;
        private ObjectPooler m_bulletTrailPool;
        #endregion

        private void Awake()
        {
            if (s_instance != null && s_instance != this)
            {
                Destroy(gameObject);
                return;
            }

            s_instance = this;
        }

        void Start()
        {
            m_vfxPools = new Dictionary<string, ObjectPooler>();
            for (int i = 0; i < m_vfxs.Length; i++)
            {
                m_vfxPools.Add(m_vfxs[i].id, new ObjectPooler(m_vfxs[i].particleSystem.gameObject, m_vfxs[i].poolSize, transform, m_vfxs[i].maxPoolSize));
            }
            m_bulletTrailPool = new ObjectPooler(m_bulletTrailPrefab.gameObject, 10, transform, 20);
        }

        public void Instantiate(string a_id, Vector3 a_postion)
        {
            Instantiate(a_id, a_postion, Quaternion.identity);
        }
        public void Instantiate(string a_id, Vector3 a_postion, Quaternion a_rotation)
        {
            m_vfxPools.TryGetValue(a_id, out ObjectPooler pool);
            if (pool != null)
            {
                GameObject go = pool.GetItem();
                go.transform.position = a_postion;
                go.transform.rotation = a_rotation;
                go.SetActive(true);
                go.GetComponent<ParticleSystem>().Play();
            }
        }

        public void InstantiateBulletTrail(Vector3 a_startPosition, Vector3 a_targetPosition)
        {
            GameObject go = m_bulletTrailPool.GetItem();
            go.GetComponent<BulletTrail>().Init(a_startPosition, a_targetPosition);
        }

    }
}
