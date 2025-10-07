using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace FCTools.LoadingScreen
{
	public class LoadingScreen : MonoBehaviour
	{
		#region inspector
		[SerializeField] private MMF_Player m_fadeInFeedback;
		[SerializeField] private MMF_Player m_fadeOutFeedback;
		#endregion

		#region properties
		private Action m_onFadeInComplete;
		private Action m_onFadeOutComplete;
		#endregion


		public void Display(Action a_onFadeInComplete, Action a_onFadeOutComplete)
		{
			m_onFadeInComplete = a_onFadeInComplete;
			m_onFadeOutComplete = a_onFadeOutComplete;
			m_fadeInFeedback.PlayFeedbacks();
		}

		public void OnFadeInCompleted()
		{
			m_onFadeInComplete?.Invoke();
			m_fadeOutFeedback.PlayFeedbacks();
		}

		public void OnFadeOutCompleted()
		{
			m_onFadeOutComplete?.Invoke();
		}
	}
}