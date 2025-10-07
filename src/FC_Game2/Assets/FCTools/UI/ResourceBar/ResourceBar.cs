using System.Collections;
using System.Collections.Generic;
using TMPro;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace FCTools
{
	public class ResourceBar : MonoBehaviour
	{
		public delegate void OnResourceAmountChangeDelegate(int a_resourceAmount);
		#region inspector
		//[SerializeField] private int m_spriteIndex = 0;
		[SerializeField] private Slider m_slider;
		[SerializeField] private TextMeshProUGUI m_resourceAmount;
		[SerializeField] private MMF_Player m_reduceBarMMF;

		#endregion

		#region properties
		public OnResourceAmountChangeDelegate onResourceAmountChange;
		public int Value => m_value;
		private int m_value = 0;

		private float m_timer = 0;
		public bool IsActive => m_active;
		private bool m_active = false;

		private float m_baseTimeToGenerateOneResourceInSec;
		private float m_factor;
		#endregion

		public void Setup()
		{

		}
		public void Init(int a_startResourceAmount, float a_timeToGenerateOneResourceInSec, float a_factor = 1f)
		{
			m_baseTimeToGenerateOneResourceInSec = a_timeToGenerateOneResourceInSec;
			m_factor = a_factor;
			m_slider.minValue = 0;
			m_slider.maxValue = m_baseTimeToGenerateOneResourceInSec * m_factor;
			UpdateResourceAmount(a_startResourceAmount);
			m_timer = 0;
			m_active = true;
		}

		void Update()
		{
			if (!m_active) return;
			m_timer += Time.deltaTime;
			m_slider.value = m_timer;
			if (m_slider.value >= m_slider.maxValue)
			{
				m_timer = 0;
				m_value++;
				m_reduceBarMMF.PlayFeedbacks();
				UpdateResourceAmount(m_value);
				onResourceAmountChange?.Invoke(m_value);
			}
		}

		public void Active(bool a_active)
		{
			m_active = a_active;
		}


		public void SubstractValue(int a_value)
		{
			m_value -= a_value;
			m_value = Mathf.Clamp(m_value, 0, int.MaxValue);
			m_resourceAmount.text = $"{m_value}"; // <sprite={m_spriteIndex}>";
			onResourceAmountChange?.Invoke(m_value);
		}

		public void UpdateFactor(float a_value)
		{
			m_factor = a_value;
			m_slider.maxValue = m_baseTimeToGenerateOneResourceInSec * m_factor;
		}

		private void UpdateResourceAmount(int a_value)
		{
			m_value = a_value;
			m_resourceAmount.text = $"{m_value}"; // <sprite={m_spriteIndex}>";
		}
	}
}