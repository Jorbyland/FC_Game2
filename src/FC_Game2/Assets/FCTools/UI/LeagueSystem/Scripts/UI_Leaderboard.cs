using System;
using System.Collections;
using System.Collections.Generic;
// using Actopolus.FakeLeaderboard.Src;
using FCTools.Tween;
using RivalArcher;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FCTools
{
	public class UI_Leaderboard : MonoBehaviour
	{
	// 	#region inspector
	// 	[SerializeField] private Leaderboard m_leaderboard;
	// 	[SerializeField] private Button m_closeBtn;

	// 	[SerializeField] private TextMeshProUGUI m_title;
	// 	[SerializeField] private UI_Counter playerRankCounter;

	// 	// Player item in front of leaderboard
	// 	[SerializeField] private UI_LeaderboardItem playerItem;

	// 	// Player item container
	// 	[SerializeField] private GameObject playerItemContainer;

	// 	// All leaders
	// 	[SerializeField] private UI_LeaderboardItem[] allLeaders;

	// 	// Popup transform
	// 	[SerializeField] private Transform popup;

	// 	// Scroll view
	// 	[SerializeField] private ScrollRect scrollView;

	// 	// Content rect transform
	// 	[SerializeField] private RectTransform contentRect;
	// 	#endregion

	// 	#region properties
	// 	private LeaderboardScriptable m_data;
	// 	#endregion

	// 	public void Setup()
	// 	{
	// 		m_closeBtn.onClick.AddListener(CloseLeaderboard);
	// 	}
	// 	public void Init(LeaderboardScriptable a_data)
	// 	{
	// 		m_data = a_data;
	// 	}

	// 	public void Show(int oldRankPosition, int newRankPosition, Action onComplete = null)
	// 	{
	// 		var isUp = newRankPosition <= oldRankPosition;
	// 		var rankChangeSprite = isUp ? m_data.RankUpSprite : m_data.RankDownSprite;
	// 		var rankChangeColor = isUp ? m_data.RankUpColor : m_data.RankDownColor;

	// 		m_title.text = m_data.PopupTitle;

	// 		var playerInfo = new FCTools.PlayerInfo()
	// 		{
	// 			Username = m_data.PlayerName,
	// 			Country = rankChangeSprite
	// 		};

	// 		var index = newRankPosition < 3 ? newRankPosition - 1 : 2;
	// 		if (!isUp)
	// 		{
	// 			index = allLeaders.Length - 3;
	// 		}

	// 		var place = newRankPosition - index;

	// 		foreach (var leader in allLeaders)
	// 		{
	// 			leader.Initialize(Leaderboard.Instance.CreatePlayerInfo(), place);
	// 			place++;
	// 		}

	// 		var targetPlayerItem = allLeaders[index];

	// 		playerItemContainer.SetActive(true);
	// 		playerItem.Initialize(playerInfo, oldRankPosition);
	// 		playerItem.SetIconColor(rankChangeColor);

	// 		targetPlayerItem.Initialize(playerInfo, newRankPosition);
	// 		targetPlayerItem.SetIconColor(rankChangeColor);
	// 		targetPlayerItem.HideContent();

	// 		playerRankCounter.SetDuration(m_data.RankCounterAnimationDuration);

	// 		if (m_data.PopupShowAnimationDuration <= 0f)
	// 		{
	// 			popup.localScale = Vector3.one;
	// 			playerRankCounter.SetCount(newRankPosition);
	// 			scrollView.enabled = true;

	// 			ScrollRank(isUp, () =>
	// 			{
	// 				playerItemContainer.SetActive(false);
	// 				targetPlayerItem.ShowContent();
	// 				onComplete?.Invoke();
	// 			});
	// 			return;
	// 		}

	// 		popup.localScale = Vector3.zero;
	// 		scrollView.enabled = false;

	// 		ResizePopup(popup.localScale, Vector3.one, m_data.PopupShowAnimationDuration, () =>
	// 		{
	// 			scrollView.enabled = true;
	// 			playerRankCounter.SetCount(newRankPosition);

	// 			ScrollRank(isUp, () =>
	// 			{
	// 				playerItemContainer.SetActive(false);
	// 				targetPlayerItem.ShowContent();
	// 				onComplete?.Invoke();
	// 			});
	// 		});
	// 	}
	// 	public void Hide(Action onComplete = null)
	// 	{
	// 		if (!gameObject.activeSelf)
	// 		{
	// 			return;
	// 		}

	// 		if (m_data.PopupHideAnimationDuration <= 0f)
	// 		{
	// 			gameObject.SetActive(false);
	// 			onComplete?.Invoke();
	// 			return;
	// 		}

	// 		ResizePopup(popup.localScale, Vector3.zero, m_data.PopupHideAnimationDuration, () =>
	// 		{
	// 			gameObject.SetActive(false);
	// 			onComplete?.Invoke();
	// 		});
	// 	}
	// 	public void Reset()
	// 	{
	// 		gameObject.SetActive(true);
	// 		popup.localScale = Vector3.zero;
	// 		scrollView.horizontalNormalizedPosition = 0.5f;
	// 		scrollView.enabled = false;
	// 		contentRect.anchoredPosition = Vector2.zero;
	// 	}
		
	// 	public void CloseLeaderboard()
	// 	{
	// 		m_leaderboard.Hide();
	// 		gameObject.SetActive(false);
	// 	}


	// 	// Animates rank scrolling
	// 	private void ScrollRank(bool isUp, Action onComplete)
	// 	{
	// 		var scrollTo = isUp ? 1f : 0f;

	// 		TWAnimation.Value(this, 0.5f, scrollTo, v =>
	// 		{
	// 			scrollView.verticalNormalizedPosition = v;
	// 		}, m_data.RankCounterAnimationDuration, 0f, onComplete);
	// 	}

	// 	// Animates popup scale
	// 	private void ResizePopup(Vector3 original, Vector3 target, float duration, Action onComplete)
	// 	{
	// 		TWAnimation.Value(this, 0f, 1f, v => { popup.localScale = Vector3.Lerp(original, target, v); },
	// 			duration, 0f, onComplete);
	// 	}
	}
}