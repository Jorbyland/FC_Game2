using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools.EntitiesComponents
{
    public class Player : Actor
    {
        [System.Serializable]
        public class Settings
        {

        }
        
        #region inspector
        public Player_ExperienceComponent ExperienceComponent => m_experienceComponent;
        [SerializeField] private Player_ExperienceComponent m_experienceComponent;
        #endregion

        #region properties

        #endregion

        public override void Setup()
        {
            base.Setup();
            m_experienceComponent.Setup();
        }
        public void Init()
        {
            m_experienceComponent.Init(0,0,0);
            HealthComponent.Init(new FloatableParam(0));
        }
    }
}
