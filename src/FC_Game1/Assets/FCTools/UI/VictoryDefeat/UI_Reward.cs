using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FCTools
{
	public class UI_Reward : MonoBehaviour
	{
		#region inspector
		[SerializeField] private TextMeshProUGUI m_rewardAmount;
		[SerializeField] private Image m_rewardIcon;
		#endregion

		#region properties

		#endregion

		public void Setup()
		{

		}
		public void Init(int a_rewardAmount, Sprite a_rewardIcon)
		{
			gameObject.SetActive(a_rewardAmount > 0);
			m_rewardIcon.sprite = a_rewardIcon;
			m_rewardAmount.text = a_rewardAmount.ToUINumber();
		}

	}
}