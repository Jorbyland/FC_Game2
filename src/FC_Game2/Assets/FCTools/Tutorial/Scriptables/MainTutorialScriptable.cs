using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools.Tutorial
{
	[CreateAssetMenu(fileName = "MainTutorial", menuName = "FCTools/Tutorial/MainTutorial")]
	public class MainTutorialScriptable : ScriptableObject
	{
		#region inspector
		[SerializeField] private TutorialStepScriptable[] m_steps;
		public TutorialStepScriptable[] Steps => m_steps;
		#endregion

		public int GetStepIdByFunctionId(int a_functionId)
		{
			for (int i = 0; i < m_steps.Length; i++)
			{
				if (m_steps[i].FunctionId == a_functionId)
				{
					return i;
				}
			}
			return -1;
		}
	}
}