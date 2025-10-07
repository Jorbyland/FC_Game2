using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools.EntitiesComponents
{
    public class Player_ExperienceComponent : MonoBehaviour
    {
        public delegate void LevelAndXpChangedEvent(float a_experience, float a_maxExperience, int a_level);
        public delegate void LevelChangedEvent(float a_experience, int a_level, float remainingXP);

        #region Properties
        private float m_experience;
        public float Experience => m_experience;

        private int m_level = 1;
        public int Level => m_level;

        private float m_maxExperience;
        private float m_xpMultiplicator;

        public LevelAndXpChangedEvent onExperienceChanged;
        public LevelChangedEvent onLevelUp;
        #endregion

        public void Setup()
        {
            m_xpMultiplicator = 1;
        }

        public void Init(float a_currentXP, float a_maxXP, int a_level)
        {
            m_experience = a_currentXP;
            m_maxExperience = a_maxXP;
            m_level = a_level;
        }

        public void AddMultiplicator(float a_value)
        {
            m_xpMultiplicator += a_value;
        }

        public void AddXp(float a_experienceToAdd)
        {
            m_experience += (a_experienceToAdd * m_xpMultiplicator);
            bool levelUp = m_experience >= m_maxExperience;
            float remainingXp = 0;
            onExperienceChanged?.Invoke(m_experience, m_maxExperience, m_level);
            if (levelUp)
            {
                remainingXp = m_experience - m_maxExperience;
                m_experience = m_maxExperience;
                InternalLevelUp(remainingXp);
            }
        }

        protected virtual void InternalLevelUp(float a_remainingXP)
        {
            m_level++;
            onLevelUp?.Invoke(m_experience, m_level, a_remainingXP);
        }

        protected virtual void OnDestroy()
        {
            onExperienceChanged = null;
            onLevelUp = null;
        }
    }
}
