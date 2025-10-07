using System.Collections;
using System.Collections.Generic;
using RivalVillages;
using UnityEngine;

namespace FCTools
{
	public class PlayerProfile_CurrencyComponent : MonoBehaviour
	{
		#region inspector
		public Currencies.Currency[] Currencies => m_currencies;
		[SerializeField] Currencies.Currency[] m_currencies;
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
	}
}