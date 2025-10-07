using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools
{
	[CreateAssetMenu(fileName = "GameRanks", menuName = "FCTools/ScriptableObjects/Game Ranks")]
	public class GameRanksScriptables : ScriptableObject
	{
		public enum Rank
		{
			Bronze,
			Silver,
			Gold,
			Platinum,
			Diamond,
			Champion,
			GrandChampion,
			Elite
		}

		[System.Serializable]
		public class GameRank
		{
			public Rank rank;
			public Vector2 trophiesRange;
			public string name;
			public Color color;
			public LeaderboardScriptable data;
		}

		[SerializeField] private List<GameRank> m_ranks;
		public List<GameRank> Ranks => m_ranks;

		public GameRank GetRank(int a_trophies)
		{
			if (a_trophies <= 0)
			{
				return m_ranks.Find(x => x.rank == Rank.Bronze);
			}
			else if (a_trophies >= m_ranks.Find(x => x.rank == Rank.Elite).trophiesRange.y)
			{
				return m_ranks.Find(x => x.rank == Rank.Elite);
			}
			else
			{
				foreach (var rank in m_ranks)
				{
					if (a_trophies >= rank.trophiesRange.x && a_trophies <= rank.trophiesRange.y)
					{
						return rank;
					}
				}
			}

			return null;
		}
	}
}