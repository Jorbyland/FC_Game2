using System.Collections;
using System.Collections.Generic;
using FCTools;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RivalVillages
{
	[RequireComponent(typeof(Slider))]
	public class SmoothSliderBar : MonoBehaviour
	{
		public delegate void OnReachMaxValueDelegate();
		#region inspector
		// [Range(0.01f, 1)]
		[SerializeField] private float m_smoothSpeed = 0.5f;

		[SerializeField] private bool m_useText = true;
		[SerializeField] private TextMeshProUGUI m_valueTMP;

		[SerializeField] private bool m_useFullAnim = true;
		[SerializeField] private MMF_Player m_fullAnim;
		#endregion

		#region properties
		public OnReachMaxValueDelegate onReachMaxValue;
		private Slider m_slider;
		private float m_currentValue;
		private float m_targetValue;
		private bool m_reachNextLevel = false;
		#endregion

		public void Setup()
		{
			m_slider = GetComponent<Slider>();
		}
		public void Init(float a_maxValue, float a_startValue)
		{
			m_reachNextLevel = false;
			m_slider.minValue = 0;
			m_slider.maxValue = a_maxValue;
			m_currentValue = 0;
			m_targetValue = a_startValue;
			if (m_useText)
			{
				UpdateText();
			}
			if (m_useFullAnim)
			{
				m_fullAnim.RestoreInitialValues();
				m_fullAnim.gameObject.SetActive(false);
				// if (m_fullAnim.IsPlaying)
				// {
				// 	try
				// 	{
				// 		m_fullAnim.StopFeedbacks();
				// 	}
				// 	catch
				// 	{
				// 		Debug.Log("<color=red>Feel bug error on StopFeedback</color>");
				// 	}
				// }
				// m_fullAnim.RestoreInitialValues();
			}
		}

		void Update()
		{
			if (m_slider.value != m_targetValue)
			{
				m_currentValue = Mathf.MoveTowards(m_currentValue, m_targetValue, m_smoothSpeed * Time.deltaTime);
				m_slider.value = m_currentValue;
				if (m_useText)
				{
					UpdateText();
				}
				if (m_slider.value == m_slider.maxValue && !m_reachNextLevel)
				{
					m_reachNextLevel = true;
					if (m_useFullAnim)
					{
						m_fullAnim.gameObject.SetActive(true);
						// m_fullAnim.RestoreInitialValues();
						// m_fullAnim.PlayFeedbacks();
					}
					onReachMaxValue.Invoke();
				}
			}
		}

		public void UpdateValue(float a_value)
		{
			m_targetValue = a_value;
		}

		private void UpdateText()
		{
			m_valueTMP.text = $"XP {(int)m_currentValue}/{(int)m_slider.maxValue}";
		}
	}
}