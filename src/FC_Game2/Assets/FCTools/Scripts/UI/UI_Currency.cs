using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FCTools
{
	public class UI_Currency : MonoBehaviour
	{
		private const float TIME_MAX_TO_UPDATE = 1.5f;
		#region inspector
		public Currencies.Name CurrencyName => m_currencyName;
		[SerializeField] private Currencies.Name m_currencyName;
		[SerializeField] private TextMeshProUGUI m_currencyTMP;
		#endregion

		#region properties
		private int m_lastValue;
		private int m_currenValue;
		private int m_newValue;
		private float m_animDuration;
		private float m_timer;
		private bool m_isAnimate;
		#endregion

		public void Setup()
		{
			m_currencyTMP.text = "0";
			m_isAnimate = false;
		}

		void Update()
		{
			if (m_isAnimate)
			{
				m_timer += Time.deltaTime;
				if (m_timer < m_animDuration)
				{
					m_currenValue = (int)Mathf.Lerp(m_lastValue, m_newValue, m_timer / m_animDuration);
				}
				else
				{
					m_currenValue = m_lastValue = m_newValue;
					m_isAnimate = false;
				}
				m_currencyTMP.text = m_currenValue.ToUINumber();
			}
		}

		public void SetValue(int a_value)
		{
			m_newValue = m_currenValue = m_lastValue = a_value;
			m_currencyTMP.text = m_newValue.ToUINumber();
		}

		public void UpdateValue(int a_value)
		{
			int diff = Mathf.Abs(m_newValue - a_value);
			m_timer = 0;
			m_animDuration = Mathf.Clamp((float)diff / 100f, 0, TIME_MAX_TO_UPDATE);
			m_newValue = a_value;
			m_isAnimate = true;
		}
	}
}