using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools.EntitiesComponents
{
    public abstract class Entity : MonoBehaviour
    {
        #region inspector
        public Entity_HitboxComponent HitboxComponent => m_hitboxComponent;
        [SerializeField] private Entity_HitboxComponent m_hitboxComponent;
        #endregion

        #region properties

        #endregion

        public virtual void Setup()
        {
            m_hitboxComponent.Setup(this);
        }
    }
}
