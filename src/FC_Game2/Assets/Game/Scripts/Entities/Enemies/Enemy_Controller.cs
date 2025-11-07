using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    public class Enemy_Controller : MonoBehaviour
    {
        #region properties
        private Enemy m_enemy;
        #endregion

        public void Setup(Enemy a_enemy)
        {
            m_enemy = a_enemy;
        }
        public void Init()
        {
            m_enemy.HealthComponent.onDeath += OnEnemyDie;
        }

        void Onisable()
        {
            m_enemy.HealthComponent.onDeath -= OnEnemyDie;
        }

        private void OnEnemyDie(Enemy a_enemy)
        {
            a_enemy.Manager.DestroyEnemy(a_enemy.Id, a_enemy.gameObject);
        }
    }
}
