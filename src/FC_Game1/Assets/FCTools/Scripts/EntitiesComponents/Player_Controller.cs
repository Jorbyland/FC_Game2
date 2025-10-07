using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools.EntitiesComponents
{
    public class Player_Controller : MonoBehaviour
    {
        #region inspector

        #endregion

        #region properties
        private Player m_player;
        #endregion

        public void Setup(Player a_player)
        {
            m_player = a_player;
            m_player.TriggerDetection.onTriggerEnterDelegate += OnDetectEntity;
            m_player.TriggerDetection.onTriggerExitDelegate += OnLoseEntity;
        }
        public void Init()
        {

        }

        private void OnDestroy(){
            m_player.TriggerDetection.onTriggerEnterDelegate -= OnDetectEntity;
            m_player.TriggerDetection.onTriggerExitDelegate -= OnLoseEntity;
        }

        private void OnDetectEntity(Entity a_otherEntity){

        }
        private void OnLoseEntity(Entity a_otherEntity){

        }

    }
}
