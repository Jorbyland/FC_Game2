using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools
{
	public class FloatableParam
	{
		public float BaseValue => m_baseValue;
		private float m_baseValue = 0;
		public float AdditionnalValue => m_additionnalValue;
		public float m_additionnalValue = 0;
		public float Multiplicator => m_multiplicator;
		public float m_multiplicator = 1;
		public float Value => Compute();

		public FloatableParam(float a_baseValue, float a_additionnalValue = 0, float a_multiplicator = 1)
		{
			m_baseValue = a_baseValue;
			m_additionnalValue = a_additionnalValue;
			m_multiplicator = a_multiplicator;
		}

		public void AddMultiplicator(float a_value)
		{
			m_multiplicator += a_value;
		}
		public void AddAdditionnalValue(float a_value)
		{
			m_additionnalValue += a_value;
		}

		private float Compute()
		{
			return m_baseValue * m_multiplicator + m_additionnalValue;
		}
	}
}