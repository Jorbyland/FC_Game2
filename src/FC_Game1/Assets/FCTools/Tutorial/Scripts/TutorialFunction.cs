using System.Collections;
using System.Collections.Generic;
using RumbleGuardian;
using UnityEngine;

namespace FCTools.Tutorial
{
	public abstract class TutorialFunction : MonoBehaviour
	{
		#region properties
		protected Tutorial tutorial;
		protected System.Action<int, bool> onCompleted;
		private int m_stepId;
		#endregion

		public virtual void Setup()
		{
			tutorial = Tutorial.T;
			enabled = false;
		}
		public virtual void Init(int a_stepId, System.Action<int, bool> a_onCompleted)
		{
			m_stepId = a_stepId;
			onCompleted = a_onCompleted;
			enabled = true;
		}
		protected void CompletStep(bool a_saveStep)
		{
			tutorial.UIComponent.ClearMessage();
			tutorial.UIComponent.ClearHand();
			enabled = false;
			onCompleted(m_stepId, a_saveStep);
		}
	}
}