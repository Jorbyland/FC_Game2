using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools
{
    public class ReturnToPoolOnDisable : MonoBehaviour
    {
        private System.Action<GameObject> m_onDisable;
        public void Init(System.Action<GameObject> a_onDisable)
        {
            m_onDisable = a_onDisable;
        }

        private void OnDisable()
        {
            m_onDisable(gameObject);
        }
    }
}
