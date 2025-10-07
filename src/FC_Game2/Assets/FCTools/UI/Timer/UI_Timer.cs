using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FCTools
{
	public class UI_Timer : MonoBehaviour
	{
		public delegate void OnTimerOutDelegate();
		#region inspector
		[SerializeField] private TextMeshProUGUI m_timerTMP;
		#endregion

		#region properties
		public bool IsActivate => m_isActivate;
		private bool m_isActivate;
		private float m_durationInSec;
		public OnTimerOutDelegate onTimerOut;
		#endregion

		public void Setup()
		{

		}
		public void Init(int a_durationInSec)
		{
			m_durationInSec = a_durationInSec;
			UpdateVisual();
			m_isActivate = true;
		}

		void Update()
		{
			if (!m_isActivate) return;

			UpdateVisual();
			if (m_durationInSec == 0)
			{
				m_isActivate = false;
				onTimerOut?.Invoke();
			}
		}

		public void DisactiveTimer()
		{
			m_isActivate = false;
		}
		public void ActiveTimer()
		{
			m_isActivate = true;
		}

		private void UpdateVisual()
		{
			m_durationInSec -= Time.deltaTime;
			m_durationInSec = Mathf.Clamp(m_durationInSec, 0, int.MaxValue);
			int minutes = Mathf.FloorToInt(m_durationInSec / 60f);
			int seconds = Mathf.FloorToInt(m_durationInSec) - (minutes * 60);
			m_timerTMP.text = $"{minutes.ToString("00")}:{seconds.ToString("00")}";
		}
	}

}