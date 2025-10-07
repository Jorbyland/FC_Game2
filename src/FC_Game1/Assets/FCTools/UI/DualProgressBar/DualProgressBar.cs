using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FCTools
{
	public class DualProgressBar : MonoBehaviour
	{
		#region inspector
		[SerializeField] private Slider m_leftSlider;
		[SerializeField] private Slider m_rightSlider;
		[SerializeField] private TextMeshProUGUI m_leftScoreTMP;
		[SerializeField] private TextMeshProUGUI m_leftPlayerNameTMP;
		[SerializeField] private TextMeshProUGUI m_rightScoreTMP;
		[SerializeField] private TextMeshProUGUI m_rightPlayerNameTMP;
		[SerializeField] private bool m_usePercent = true;
		#endregion

		#region properties
		public float LeftValue => m_leftValue;
		private float m_leftValue;
		public float RightValue => m_rightValue;
		private float m_rightValue;
		#endregion

		public void Setup()
		{

		}
		public void Init(string a_playerName, string a_opponentName)
		{
			UpdateProgression(0.5f, 0, 0);
			m_rightValue = 0.5f;
			m_leftValue = 0.5f;
			m_rightPlayerNameTMP.text = "";
			m_leftPlayerNameTMP.text = a_playerName;
			m_rightPlayerNameTMP.text = a_opponentName;
		}

		public void SetOpponentName(string a_opponentName)
		{
			m_rightPlayerNameTMP.text = a_opponentName;
		}

		public void UpdateScores(int a_leftScore, int a_rightScore)
		{
			UpdateProgression(CalculScoreRatio(a_leftScore, a_rightScore), a_leftScore, a_rightScore);
		}

		private void UpdateProgression(float a_leftValue, int a_leftScore, int a_rightScore)
		{
			a_leftValue = Mathf.Clamp01(a_leftValue);
			m_leftSlider.value = a_leftValue;
			m_rightSlider.value = 1f - a_leftValue;
			if (m_usePercent)
			{
				m_leftScoreTMP.text = $"{(m_leftSlider.value * 100f).ToUINumber()}%";
				m_rightScoreTMP.text = $"{(m_rightSlider.value * 100f).ToUINumber()}%";
			}
			else
			{
				m_leftScoreTMP.text = a_leftScore.ToUINumber();
				m_rightScoreTMP.text = a_rightScore.ToUINumber();
			}


			m_leftValue = a_leftValue;
			m_rightValue = 1 - a_leftValue;
		}

		private float CalculScoreRatio(int a_leftScore, int a_rightScore)
		{
			return (float)a_leftScore / (a_leftScore + a_rightScore);
		}
	}

}