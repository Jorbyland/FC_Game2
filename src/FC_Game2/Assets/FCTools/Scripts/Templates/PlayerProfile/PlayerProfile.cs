using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools
{
	public class PlayerProfile : MonoBehaviour
	{
		#region inspector
		public PlayerProfile_Controller Controller => m_controller;
		[SerializeField] private PlayerProfile_Controller m_controller;
		public PlayerProfile_DataComponent DataComponent => m_dataComponent;
		[SerializeField] private PlayerProfile_DataComponent m_dataComponent;
		public PlayerProfile_RewardComponent RewardComponent => m_rewardComponent;
		[SerializeField] private PlayerProfile_RewardComponent m_rewardComponent;
		public PlayerProfile_CurrencyComponent CurrencyComponent => m_currencyComponent;
		[SerializeField] private PlayerProfile_CurrencyComponent m_currencyComponent;
		#endregion

		#region properties

		#endregion

		public void Setup()
		{
			m_dataComponent.Setup(this);
			m_currencyComponent.Setup(this);
			m_rewardComponent.Setup(this);
			m_controller.Setup(this);
		}
		public void Init()
		{
			m_dataComponent.Init();
			m_currencyComponent.Init();
			m_rewardComponent.Init();
			m_controller.Init();
		}

		void OnDestroy()
		{
			m_controller.DoOnDestroy();
		}
	}
}