using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools.DebugTools
{
	public class DebugTools : MonoBehaviour
	{
		#region inspector
		[SerializeField] private bool m_useDebug;
		[SerializeField] private GameObject m_debugView;
		[SerializeField] private float intervalMaxBetweenSecretButton = 2f;
		[SerializeField] private SecretButtonDebugMode[] m_secretButtonDebugModeOrder;
		#endregion

		#region properties
		private float m_timeSinceLastSecretButton = 0f;
		private int m_currentSecretButtonIndex = 0;
		private bool m_isDebugViewOpen = false;
		#endregion

		private enum SecretButtonDebugMode
		{
			Left,
			Right
		}

		public void Setup()
		{
			CloseDebugView();
		}

		public void Init()
		{

		}

		void Update()
		{
			if (m_useDebug)
			{
				m_timeSinceLastSecretButton += Time.deltaTime;
				if (m_timeSinceLastSecretButton >= intervalMaxBetweenSecretButton)
				{
					m_currentSecretButtonIndex = 0;
				}

#if UNITY_EDITOR
				if (Input.GetKeyDown(KeyCode.Escape))
				{
					if (m_isDebugViewOpen)
						CloseDebugView();
					else
						OpenDebugView();
				}

				if (Input.GetKeyDown(KeyCode.Z))
					Time.timeScale = 1f;
				if (Input.GetKeyDown(KeyCode.X) && Time.timeScale - 2f > 0f)
					Time.timeScale -= 2f;
				if (Input.GetKeyDown(KeyCode.C))
					Mathf.Clamp(Time.timeScale += 2f, 0.1f, 10f);
#endif
			}

		}


		public void OpenDebugView()
		{
			m_debugView.SetActive(true);
			m_isDebugViewOpen = true;
		}

		public void CloseDebugView()
		{
			m_debugView.SetActive(false);
			m_isDebugViewOpen = false;
		}

		public void LeftSecretButton_Click()
		{
			if (m_secretButtonDebugModeOrder[m_currentSecretButtonIndex] == SecretButtonDebugMode.Left)
			{
				m_timeSinceLastSecretButton = 0f;
				m_currentSecretButtonIndex++;
				if (m_currentSecretButtonIndex >= m_secretButtonDebugModeOrder.Length)
				{
					OpenDebugView();
					m_currentSecretButtonIndex = 0;
				}
			}
			else
			{
				m_currentSecretButtonIndex = 0;
			}
		}

		public void RightSecretButton_Click()
		{
			if (m_secretButtonDebugModeOrder[m_currentSecretButtonIndex] == SecretButtonDebugMode.Right)
			{
				m_timeSinceLastSecretButton = 0f;
				m_currentSecretButtonIndex++;
				if (m_currentSecretButtonIndex >= m_secretButtonDebugModeOrder.Length)
				{
					OpenDebugView();
					m_currentSecretButtonIndex = 0;
				}
			}
			else
			{
				m_currentSecretButtonIndex = 0;
			}
		}


		public void BackToMainMenu()
		{
			CloseDebugView();
		}

		public void Add100Gold()
		{
			// AppContext.A.PlayerProfile.DataComponent.AddGold(100);
			CloseDebugView();
		}
	}
}