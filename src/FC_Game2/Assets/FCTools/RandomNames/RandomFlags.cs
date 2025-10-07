using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools.RandomNames
{
	public class RandomFlags : MonoBehaviour
	{
		#region inspector
		public Sprite[] Flags => m_flags;
		[SerializeField] private Sprite[] m_flags;
		#endregion

		#region properties

		#endregion

		public Sprite GetRandomFlag()
		{
			int rand = Random.Range(0, m_flags.Length);
			return m_flags[rand];
		}

		public Sprite GetFlag(int a_id)
		{
			return m_flags[a_id >= m_flags.Length ? 0 : a_id];
		}
	}
}