using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools.Tutorial
{
	public class Tutorial_Controller : MonoBehaviour
	{
		#region inspector

		#endregion

		#region properties
		private Tutorial m_tutorial;
		#endregion

		public void Setup(Tutorial a_tutorial)
		{
			m_tutorial = a_tutorial;
		}
		public void Init()
		{
			m_tutorial.DataComponent.onStepCompleted += OnStepCompleted;
		}

		public void DoOnDisable()
		{
			m_tutorial.DataComponent.onStepCompleted -= OnStepCompleted;
		}

		private void OnStepCompleted(int a_lastCompletedStep)
		{
			int newStep = a_lastCompletedStep + 1;
			m_tutorial.ExecutorComponent.ExecuteNextStep(a_lastCompletedStep);
		}
	}
}