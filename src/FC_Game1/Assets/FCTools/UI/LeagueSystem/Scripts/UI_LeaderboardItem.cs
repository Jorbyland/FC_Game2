using System.Collections;
using System.Collections.Generic;
using RivalArcher;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FCTools
{
	public class UI_LeaderboardItem : MonoBehaviour
	{
		[SerializeField] private GameObject content;
		[SerializeField] private Image icon;
		[SerializeField] private TextMeshProUGUI username;
		[SerializeField] private UI_Counter placeCounter;

		// Initializes item
		public void Initialize(FCTools.PlayerInfo playerInfo, int rank)
		{
			SetPlayerInfo(playerInfo);
			SetRank(rank);
		}

		// Sets color for country icon
		public void SetIconColor(Color color) => icon.color = color;

		// Hides content of item
		public void HideContent() => content.SetActive(false);

		// Shows content of item
		public void ShowContent() => content.SetActive(true);

		// Sets player info
		private void SetPlayerInfo(FCTools.PlayerInfo playerInfo)
		{
			icon.sprite = playerInfo.Country;
			username.text = playerInfo.Username;
		}

		// Sets rank position
		private void SetRank(int rank) => placeCounter.SetCountQuiet(rank);
	}
}