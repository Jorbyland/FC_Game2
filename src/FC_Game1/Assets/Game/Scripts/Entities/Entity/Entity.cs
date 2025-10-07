using UnityEngine;

namespace Game
{
    public class Entity : MonoBehaviour, IGameComponent
    {
        #region inspector
        public Entity_HitboxComponent HitboxComponent => m_hitboxComponent;
        [SerializeField] private Entity_HitboxComponent m_hitboxComponent;
        #endregion

        #region properties

        #endregion
        public virtual void Setup(GameContextScriptable context)
        {
            m_hitboxComponent.Setup(this);
        }

        public virtual void Init(GameState state)
        {
            m_hitboxComponent.Init();
        }
    }
}
