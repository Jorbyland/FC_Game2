using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools
{
	public class LeaderboardSystem : MonoBehaviour
	{
		// private const string PLAYER_RANK_KEY = "Leaderboard.PlayerRank";
		// private const string CURRENT_LEAGUE_KEY = "Leaderboard.League";

		// #region inspector
		// [SerializeField] private GameRanksScriptables m_ranksSO;
		// [SerializeField] private UI_Leaderboard m_leaderBoard;
		// [SerializeField] private UI_League m_league;
		// #endregion

		// #region properties
		// private int m_currentLeague;
		// #endregion

		// public void Setup()
		// {
		// 	m_league.Setup(this);
		// 	m_leaderBoard.Setup();
		// }
		// public void Init()
		// {
		// 	m_currentLeague = loadLeagueId();
		// 	m_league.Init();
		// 	m_leaderBoard.Init(LeaderboardSO(m_currentLeague));
		// }

		// public PlayerInfo CreatePlayerInfo(int leagueId) => LeaderboardSO(leagueId).CreatePlayerInfo();

		// public int GetRank(int leagueId)
		// {
		// 	LeaderboardScriptable leaderboardSO = LeaderboardSO(leagueId);
		// 	if (leaderboardSO)
		// 	{
		// 		int defaultVaule = UnityEngine.Random.Range(
		// 			LeaderboardSO(leagueId).MinInitialRank,
		// 			LeaderboardSO(leagueId).MaxInitialRank);
		// 		if (ES3.KeyExists(PLAYER_RANK_KEY))
		// 		{
		// 			return ES3.Load(PLAYER_RANK_KEY, defaultVaule);
		// 		}
		// 		return defaultVaule;
		// 	}
		// 	return 9999;
		// }

		// public void Reset()
		// {
		// 	int defaultVaule = UnityEngine.Random.Range(
		// 		LeaderboardSO(0).MinInitialRank,
		// 		LeaderboardSO(0).MaxInitialRank);
		// 	ES3.Save(PLAYER_RANK_KEY, defaultVaule);
		// }

		// public void Show(Action onComplete = null)
		// {
		// 	var oldRank = GetRank(m_currentLeague);
		// 	var newRank = oldRank - UnityEngine.Random.Range(LeaderboardSO(m_currentLeague).MinRankStep, LeaderboardSO(m_currentLeague).MaxRankStep);
		// 	newRank = Mathf.Max(1, newRank);

		// 	Show(oldRank, newRank, onComplete);
		// }

		// public void Show(int oldRankPosition, int newRankPosition, Action onComplete = null)
		// {
		// 	ES3.Save(PLAYER_RANK_KEY, newRankPosition);
		// 	m_leaderBoard.gameObject.SetActive(true);
		// 	StartCoroutine(ShowCoroutine(oldRankPosition, newRankPosition, onComplete));
		// }

		// public void Hide(Action onComplete = null)
		// {
		// 	m_leaderBoard.Hide(onComplete);
		// }

		// private IEnumerator ShowCoroutine(int oldRankPosition, int newRankPosition, Action onComplete = null)
		// {
		// 	m_leaderBoard.Reset();
		// 	yield return null;
		// 	m_leaderBoard.Show(oldRankPosition, newRankPosition, onComplete);
		// }

		// private LeaderboardScriptable LeaderboardSO(int leagueId)
		// {
		// 	if (leagueId < m_ranksSO.Ranks.Count)
		// 	{
		// 		return m_ranksSO.Ranks[leagueId].data;
		// 	}
		// 	return null;
		// }

		// private int loadLeagueId()
		// {
		// 	int defaultVaule = 0;
		// 	if (ES3.KeyExists(CURRENT_LEAGUE_KEY))
		// 	{
		// 		return ES3.Load(CURRENT_LEAGUE_KEY, defaultVaule);
		// 	}
		// 	return defaultVaule;
		// }
		// private void SaveLeague()
		// {
		// 	ES3.Save(CURRENT_LEAGUE_KEY, m_currentLeague);
		// }
	}
}