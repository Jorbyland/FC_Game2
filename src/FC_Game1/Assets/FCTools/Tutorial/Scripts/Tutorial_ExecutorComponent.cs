using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools.Tutorial
{
	public class Tutorial_ExecutorComponent : MonoBehaviour
	{
		#region properties
		private Tutorial m_tutorial;
		#endregion

		public void Setup(Tutorial a_tutorial)
		{
			m_tutorial = a_tutorial;
		}
		public void Init()
		{

		}

		public void ExecuteNextStep(int a_lastCompletedStep)
		{
			int newStep = a_lastCompletedStep + 1;
			if (newStep < m_tutorial.TutoSO.Steps.Length)
			{
				TutorialStepScriptable nextStep = m_tutorial.TutoSO.Steps[newStep];
				m_tutorial.FunctionsComponent.Execute(nextStep, newStep);
			}
			else
			{
				m_tutorial.UIComponent.HideBlackScreen();
			}
		}
	}
}