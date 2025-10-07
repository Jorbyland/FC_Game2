using System;
using System.Collections;
using System.Collections.Generic;
using FCTools.Tween;
using TMPro;
using UnityEngine;

namespace RivalArcher
{
	public class UI_Counter : MonoBehaviour
	{
		#region inspector
		[SerializeField] private string m_format = "{0}";
		[SerializeField] private TextMeshProUGUI m_label;
		#endregion

		#region properties
		private int m_value;
		private int m_animatedValue;
		private float m_duration = 1f;
		#endregion

		public void SetDuration(float duration) => m_duration = duration;
		public int GetCount() => m_value;
		public void SetCountQuiet(int value)
		{
			m_value = value;
			m_animatedValue = value;

			UpdateLabel();
		}

		public void SetCount(int value)
		{
			m_value = value;

			TWAnimation.Value(this, m_animatedValue, m_value, v =>
			{
				m_animatedValue = Mathf.FloorToInt(v);
				UpdateLabel();
			}, m_duration);
		}

		private void UpdateLabel() => m_label.text = String.Format(m_format, m_animatedValue.ToString());
	}
}