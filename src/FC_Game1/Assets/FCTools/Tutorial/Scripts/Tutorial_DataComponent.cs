using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools.Tutorial
{
	public class Tutorial_DataComponent : MonoBehaviour
	{
		public const string DATAKEY_TUTO = "TUTORIAL";

		[Serializable]
		public class TutorialData
		{
			public int LastCompletedStep => m_lastCompletedStep;
			[SerializeField] private int m_lastCompletedStep = -1;
			public void CompletStep(int a_stepId)
			{
				m_lastCompletedStep = a_stepId;
			}
		}

		public delegate void OnStepCompletDelegate(int a_lastCompletedStep);

		#region properties
		public OnStepCompletDelegate onStepCompleted;
		private Tutorial m_tutorial;
		public TutorialData Data { get; private set; }
		private bool m_isInitialized;
		#endregion

		public void Setup(Tutorial a_tutorial)
		{
			m_tutorial = a_tutorial;
			Data = new TutorialData();
		}
		public void Init()
		{
			if (!m_isInitialized)
			{
				Data = LoadData();
				m_isInitialized = true;
			}
		}

		public void CompletStep(int a_stepId, bool a_saveStep)
		{
			Data.CompletStep(a_stepId);
			if (a_saveStep)
			{
				SaveData(Data);
			}
			onStepCompleted?.Invoke(a_stepId);
		}



		#region Save/Load
		private TutorialData LoadData()
		{
			if (ES3.KeyExists(DATAKEY_TUTO))
			{
				return ES3.Load<TutorialData>(DATAKEY_TUTO);
			}
			return new TutorialData();

		}

		private void SaveData(TutorialData a_data)
		{
			ES3.Save(DATAKEY_TUTO, a_data);
		}
		#endregion
	}
}