using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools.Tutorial
{
	[CreateAssetMenu(fileName = "TutorialStep", menuName = "FCTools/Tutorial/TutorialStep")]
	public class TutorialStepScriptable : ScriptableObject
	{
		#region inspector
		[SerializeField] private int m_functionId;
		public int FunctionId => m_functionId;
		[SerializeField] private string m_message;
		public string Message => m_message;
		#endregion
	}
}