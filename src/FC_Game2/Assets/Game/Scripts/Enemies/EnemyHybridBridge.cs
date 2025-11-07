using UnityEngine;

namespace Game
{
    public class EnemyHybridBridge : MonoBehaviour
    {
        public EnemyManagerGrid manager;
        public int id;

        void Update()
        {
            // Exemple : mort ou autre
            // if (health <= 0) manager.ReturnEnemyToGPU(id, gameObject);
        }

        public void Kill()
        {
            manager.DestroyEnemy(id, gameObject);
        }
    }
}