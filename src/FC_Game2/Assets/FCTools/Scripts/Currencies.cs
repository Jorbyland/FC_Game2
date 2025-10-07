using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools
{
	public class Currencies : MonoBehaviour
	{
		public enum Name { Gem, Gold }
		[System.Serializable]
		public class Currency
		{
			public Name Name => m_name;
			[SerializeField] private Name m_name;
			public Sprite Visual => m_visual;
			[SerializeField] private Sprite m_visual;
		}

		[System.Serializable]
		public class CurrencyData
		{
			public Dictionary<Name, int> currencies = new Dictionary<Name, int>();

			public CurrencyData()
			{

			}
			public CurrencyData(Currency[] a_currencies)
			{
				currencies = new Dictionary<Name, int>();
				for (int i = 0; i < a_currencies.Length; i++)
				{
					currencies.Add(a_currencies[i].Name, 0);
				}
			}

			public int GetCurrencyAmount(Name a_name)
			{
				return currencies[a_name];
			}
		}
	}
}