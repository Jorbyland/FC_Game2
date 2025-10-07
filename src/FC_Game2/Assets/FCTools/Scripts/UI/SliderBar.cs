using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RivalVillages
{
	[RequireComponent(typeof(Slider))]
	public class SliderBar : MonoBehaviour
	{
		#region inspector

		#endregion

		#region properties
		private Slider m_slider;
		#endregion

		public void Setup()
		{
			m_slider = GetComponent<Slider>();
		}
		public void Init(int a_minvalue, int a_maxValue, int a_initialValue)
		{
			m_slider.minValue = a_minvalue;
			m_slider.maxValue = a_maxValue;
			m_slider.value = a_initialValue;
		}
		public void Init(float a_minvalue, float a_maxValue, float a_initialValue)
		{
			m_slider.minValue = a_minvalue;
			m_slider.maxValue = a_maxValue;
			m_slider.value = a_initialValue;
		}

		public void UpdateValue(int a_value)
		{
			m_slider.value = a_value;
		}
		public void UpdateValue(float a_value)
		{
			m_slider.value = a_value;
		}
	}
}