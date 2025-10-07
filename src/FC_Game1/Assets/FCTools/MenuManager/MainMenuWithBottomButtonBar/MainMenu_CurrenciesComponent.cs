using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools.Menus
{
	public class MainMenu_CurrenciesComponent : MonoBehaviour
	{
		#region inspector

		[SerializeField] private UI_Currency[] m_currencies;
		#endregion

		#region properties
		private MainMenu m_mainMenu;

		private Dictionary<Currencies.Name, List<UI_Currency>> m_currenciesDict = new Dictionary<Currencies.Name, List<UI_Currency>>();
		#endregion

		public void Setup(MainMenu a_mainMenu)
		{
			m_mainMenu = a_mainMenu;

			m_currenciesDict = new Dictionary<Currencies.Name, List<UI_Currency>>();
			for (int i = 0; i < m_currencies.Length; i++)
			{
				if (m_currenciesDict.ContainsKey(m_currencies[i].CurrencyName))
				{
					m_currenciesDict[m_currencies[i].CurrencyName].Add(m_currencies[i]);
				}
				else
				{
					m_currenciesDict.Add(m_currencies[i].CurrencyName, new List<UI_Currency>() { m_currencies[i] });
				}
			}
		}
		public void Init()
		{
			foreach (var item in m_currenciesDict)
			{
				for (int i = 0; i < item.Value.Count; i++)
				{
					item.Value[i].Setup();
				}
			}
		}

		public void SetCurrency(Currencies.Name a_name, int a_value)
		{
			if (m_currenciesDict.TryGetValue(a_name, out List<UI_Currency> uI_Currency))
			{
				for (int i = 0; i < uI_Currency.Count; i++)
				{
					uI_Currency[i].SetValue(a_value);
				}
			}
		}
		public void UpdateCurrency(Currencies.Name a_name, int a_value)
		{
			if (m_currenciesDict.TryGetValue(a_name, out List<UI_Currency> uI_Currency))
			{
				for (int i = 0; i < uI_Currency.Count; i++)
				{
					uI_Currency[i].UpdateValue(a_value);
				}
			}
		}
	}
}