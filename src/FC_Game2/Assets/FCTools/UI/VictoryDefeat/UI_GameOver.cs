using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

namespace FCTools
{
	public class UI_GameOver : MonoBehaviour
	{
		[System.Serializable]
		public class Reward
		{
			public RewardType Type => m_type;
			[SerializeField] private RewardType m_type;
			public Sprite Icon => m_icon;
			[SerializeField] private Sprite m_icon;
		}
		public enum RewardType { gems, gold, xp, score }
		public delegate void OnClicContinueDelegate();
		#region inspector
		[SerializeField] private GameObject m_rewardPredab;
		[SerializeField] private Transform m_rewardFolder;
		[SerializeField] private Button m_continueBtn;
		[SerializeField] private MMF_Player m_anim;
		[SerializeField] private Reward[] m_avaiableRewards;
		#endregion

		#region properties
		public OnClicContinueDelegate onClicContinue;
		private List<UI_Reward> m_rewards = new List<UI_Reward>();
		private Dictionary<RewardType, Sprite> m_rewardsIcons = new Dictionary<RewardType, Sprite>();
		#endregion

		public void Setup()
		{
			m_continueBtn.onClick.AddListener(() =>
			{
				HapticManager.Play(HapticManager.Type.SELECTION);
				gameObject.SetActive(false);
				onClicContinue?.Invoke();
			});
		}
		public void Init()
		{
			m_rewards = new List<UI_Reward>();
			m_rewardsIcons = new Dictionary<RewardType, Sprite>();
			for (int i = 0; i < m_avaiableRewards.Length; i++)
			{
				m_rewardsIcons.Add(m_avaiableRewards[i].Type, m_avaiableRewards[i].Icon);
			}
			gameObject.SetActive(false);
		}

		public void Display(int a_gemsReward)
		{
			ClearRewards();
			GameObject instance = Instantiate(m_rewardPredab);
			instance.transform.SetParent(m_rewardFolder);
			UI_Reward uiReward = instance.GetComponent<UI_Reward>();
			uiReward.Setup();
			m_rewards.Add(uiReward);
			m_rewards[0].Init(a_gemsReward, m_rewardsIcons[RewardType.gems]);
			Display();
		}
		public void StopAnim()
		{
			if (m_anim.IsPlaying)
			{
				m_anim.ResetFeedbacks();
				m_anim.RestoreInitialValues();
				m_anim.Initialization();
			}
			gameObject.SetActive(false);
		}

		private void Display()
		{
			gameObject.SetActive(true);
			m_anim.PlayFeedbacks();
		}

		private void ClearRewards()
		{
			for (int i = 0; i < m_rewards.Count; i++)
			{
				Destroy(m_rewards[i].gameObject);
			}
			m_rewards = new List<UI_Reward>();
		}

	}
}