using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FCTools.Tutorial
{
	public class Tutorial : MonoBehaviour
	{
		#region inspector
		[SerializeField] private MainTutorialScriptable m_tutoSO;
		public MainTutorialScriptable TutoSO => m_tutoSO;


		[Space(1)]
		[Header("Components")]
		[SerializeField] private Tutorial_Controller m_controller;
		public Tutorial_Controller Controller => m_controller;
		[SerializeField] private Tutorial_DataComponent m_dataComponent;
		public Tutorial_DataComponent DataComponent => m_dataComponent;
		[SerializeField] private Tutorial_ExecutorComponent m_executorComponent;
		public Tutorial_ExecutorComponent ExecutorComponent => m_executorComponent;
		[SerializeField] private Tutorial_UIComponent m_uiComponent;
		public Tutorial_UIComponent UIComponent => m_uiComponent;
		[SerializeField] private Tutorial_FunctionsComponent m_functionsComponent;
		public Tutorial_FunctionsComponent FunctionsComponent => m_functionsComponent;

		// [Space()]
		// [SerializeField] private Tutorial_MarkerComponent m_markerComponent;
		// public Tutorial_MarkerComponent MarkerComponent => m_markerComponent;
		// public NavMeshObstacle InvisibleWall => m_invisibleWall;
		// [SerializeField] private NavMeshObstacle m_invisibleWall;


		[Space(1)]
		[Header("Settings")]
		[SerializeField] private bool m_disableTutorial;
		#endregion

		#region properties
		static public Tutorial T => _T;
		static private Tutorial _T;
		private bool m_isInitialized;
		#endregion

		public void Setup()
		{
			if (_T != null && _T != this)
				Destroy(this);
			else
				_T = this;

			m_dataComponent.Setup(this);
			m_functionsComponent.Setup(this);
			m_executorComponent.Setup(this);
			m_uiComponent.Setup(this);
			// m_markerComponent.Setup(this);
			m_controller.Setup(this);
		}

		public void Init()
		{
			m_dataComponent.Init();
			m_functionsComponent.Init();
			m_executorComponent.Init();
			m_uiComponent.Init();
			// m_markerComponent.Init();
			m_controller.Init();

			// m_invisibleWall.gameObject.SetActive(false);

			if (!m_disableTutorial)
			{
				m_executorComponent.ExecuteNextStep(m_dataComponent.Data.LastCompletedStep);
			}
			else
			{
				m_uiComponent.HideBlackScreen();
			}
			m_isInitialized = true;
		}

		void OnDisable()
		{
			if (m_isInitialized)
			{
				m_controller.DoOnDisable();
			}
		}
	}
}