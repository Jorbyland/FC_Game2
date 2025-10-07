using System.Collections;
using System.Collections.Generic;
using RivalVillages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FCTools.Menus
{
	public class MainMenu_PvPComponent : SubMenu
	{
		public const string PLAYER_PREF_KEY_OPPONENT_NAME = "PLAYER_PREF_KEY_OPPONENT_NAME";
		public delegate void OnAnimationCompletedDelegate();
		#region inspector
		[SerializeField] private TextMeshProUGUI m_opponentName;
		[SerializeField] private Image m_opponentFlag;
		[SerializeField] private RandomNames.RandomFlags m_randomFlags;
		#endregion

		#region properties
		public string CurrentOpponentName => m_currentOpponentName;
		private string m_currentOpponentName;
		#endregion

		public override void Setup(MainMenu a_mainMenu)
		{
			base.Setup(a_mainMenu);
			mainMenu.onClicPlay += Open;
		}
		public override void Init()
		{
			base.Init();
		}

		void OnDestroy()
		{
			mainMenu.onClicPlay -= Open;
		}

		public override void Open()
		{
			base.Open();
			StartCoroutine(FindOpponentAnimation());
		}

		private IEnumerator FindOpponentAnimation()
		{
			// m_currentOpponentName = RandomNames.RandomNames.GetRandomNames(1)[0];
			// m_opponentName.text = m_currentOpponentName;
			// m_opponentFlag.sprite = m_randomFlags.GetRandomFlag();
			// PlayerPrefs.SetString(PLAYER_PREF_KEY_OPPONENT_NAME, m_currentOpponentName);
			// yield return new WaitForSeconds(4.2f);
			// mainMenu.onGameStart?.Invoke();
			// gameObject.SetActive(false);

			yield return new WaitForSeconds(2f);
			mainMenu.onGameStart?.Invoke();
			gameObject.SetActive(false);
		}
	}
}