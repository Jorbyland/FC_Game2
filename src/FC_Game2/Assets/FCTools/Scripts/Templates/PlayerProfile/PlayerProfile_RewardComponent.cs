using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools
{
	public class PlayerProfile_RewardComponent : MonoBehaviour
	{
		[System.Serializable]
		public class RewardData
		{
			public Dictionary<Currencies.Name, int> currencyReward = new Dictionary<Currencies.Name, int>();
		}
		#region inspector

		#endregion

		#region properties
		private PlayerProfile m_playerProfile;
		#endregion

		public void Setup(PlayerProfile a_playerProfile)
		{
			m_playerProfile = a_playerProfile;
		}
		public void Init()
		{

		}

		public void AddCurrencyReward(Currencies.Name a_name, int a_value)
		{
			m_playerProfile.DataComponent.AddCurrencyReward(a_name, a_value);
		}

		public void ClaimCurrencyReward()
		{
			Dictionary<Currencies.Name, int> currencyReward = m_playerProfile.DataComponent.CurrencyRewards.currencyReward;
			foreach (var item in currencyReward)
			{
				m_playerProfile.DataComponent.AddCurrency(item.Key, item.Value);
			}
			m_playerProfile.DataComponent.ClearCurrencyReward();
		}
	}
}