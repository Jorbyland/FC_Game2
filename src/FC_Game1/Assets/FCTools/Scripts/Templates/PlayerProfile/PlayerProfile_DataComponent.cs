using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools
{
	public class PlayerProfile_DataComponent : MonoBehaviour
	{
		public const string DATA_KEY_CURRENCIES_AMOUNT = "DATA_KEY_CURRENCIES_AMOUNT";
		public const int GEMS_DEFAULT_AMOUNT = 0;


		public const string DATA_KEY_CURRENCY_REWARDS = "DATA_KEY_CURRENCY_REWARDS";


		public delegate void OnIntegerValueChangeDelegate(Currencies.Name a_name, int a_value);
		#region inspector

		#endregion


		#region properties
		private PlayerProfile m_playerProfile;

		public OnIntegerValueChangeDelegate onCurrencyValueChange;


		public Currencies.CurrencyData CurrencyData => m_currencyData;
		private Currencies.CurrencyData m_currencyData;

		public PlayerProfile_RewardComponent.RewardData CurrencyRewards => m_currencyRewards;
		private PlayerProfile_RewardComponent.RewardData m_currencyRewards;

		#endregion

		public void Setup(PlayerProfile a_playerProfile)
		{
			m_playerProfile = a_playerProfile;
		}
		public void Init()
		{
			m_currencyData = LoadCurrencies();
			m_currencyRewards = LoadCurrencyRewards();
		}

		#region Rewards
		public void AddCurrencyReward(Currencies.Name a_name, int a_amount)
		{
			if (m_currencyRewards.currencyReward.ContainsKey(a_name))
			{
				m_currencyRewards.currencyReward[a_name] += a_amount;
			}
			else
			{
				m_currencyRewards.currencyReward.Add(a_name, a_amount);
			}
			SaveCurrencyRewards();
		}
		public void RemoveCurrencyReward(Currencies.Name a_name)
		{
			if (m_currencyRewards.currencyReward.ContainsKey(a_name))
			{
				m_currencyRewards.currencyReward.Remove(a_name);
			}
			SaveCurrencyRewards();
		}
		public void ClearCurrencyReward()
		{
			m_currencyRewards.currencyReward = new Dictionary<Currencies.Name, int>();
			SaveCurrencyRewards();
		}

		private PlayerProfile_RewardComponent.RewardData LoadCurrencyRewards()
		{
			PlayerProfile_RewardComponent.RewardData defaultVaule = new PlayerProfile_RewardComponent.RewardData();
			if (ES3.KeyExists(DATA_KEY_CURRENCY_REWARDS))
			{
				return ES3.Load(DATA_KEY_CURRENCY_REWARDS, defaultVaule);
			}
			return defaultVaule;
		}

		private void SaveCurrencyRewards()
		{
			ES3.Save(DATA_KEY_CURRENCY_REWARDS, m_currencyRewards);
		}
		#endregion

		#region Currencies
		public void SetCurrency(Currencies.Name a_name, int a_value)
		{
			m_currencyData.currencies[a_name] = a_value;
			SaveCurrencies();
			onCurrencyValueChange?.Invoke(a_name, m_currencyData.currencies[a_name]);
		}
		public void AddCurrency(Currencies.Name a_name, int a_value)
		{
			m_currencyData.currencies[a_name] += a_value;
			SaveCurrencies();
			onCurrencyValueChange?.Invoke(a_name, m_currencyData.currencies[a_name]);
		}
		public void SpendCurrency(Currencies.Name a_name, int a_value)
		{
			m_currencyData.currencies[a_name] -= a_value;
			m_currencyData.currencies[a_name] = Mathf.Clamp(m_currencyData.currencies[a_name], 0, int.MaxValue);
			onCurrencyValueChange?.Invoke(a_name, m_currencyData.currencies[a_name]);
			SaveCurrencies();
		}

		private Currencies.CurrencyData LoadCurrencies()
		{
			Currencies.CurrencyData defaultVaule = new Currencies.CurrencyData(m_playerProfile.CurrencyComponent.Currencies);
			if (ES3.KeyExists(DATA_KEY_CURRENCIES_AMOUNT))
			{
				return ES3.Load(DATA_KEY_CURRENCIES_AMOUNT, defaultVaule);
			}
			return defaultVaule;
		}

		private void SaveCurrencies()
		{
			ES3.Save(DATA_KEY_CURRENCIES_AMOUNT, m_currencyData);
		}
		#endregion
	}
}