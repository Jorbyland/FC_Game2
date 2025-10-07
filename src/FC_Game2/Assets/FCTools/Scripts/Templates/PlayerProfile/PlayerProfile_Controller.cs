using System.Collections;
using System.Collections.Generic;
using RivalVillages;
using UnityEngine;

namespace FCTools
{
	public class PlayerProfile_Controller : MonoBehaviour
	{
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
			m_playerProfile.DataComponent.onCurrencyValueChange += OnCurrencyValueChange;
		}

		public void DoOnDestroy()
		{
			m_playerProfile.DataComponent.onCurrencyValueChange -= OnCurrencyValueChange;
		}

		private void OnCurrencyValueChange(Currencies.Name a_name, int a_value)
		{
			
		}
	}
}