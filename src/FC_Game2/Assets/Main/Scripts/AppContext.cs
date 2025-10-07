using FCTools;
using Game;
using UnityEngine;

namespace Main
{
    public class AppContext : MonoBehaviour
    {
        static public AppContext A;

        public GameController GameController => m_gameController;
        [SerializeField] private GameController m_gameController;
        public Player Player => m_player;
        [SerializeField] private Player m_player;

        private void Awake()
        {
            A = this;
        }
    }
}
