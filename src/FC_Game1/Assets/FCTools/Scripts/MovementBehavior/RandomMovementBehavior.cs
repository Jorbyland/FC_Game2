using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools.MovementBehavior
{
	public class RandomMovementBehavior : MovementBehavior
	{
		#region properties
		private float m_noiseScale;
		private int m_noiseSeed;
		private Vector3 RandomDirection
		{
			get
			{
				float sample = Mathf.PerlinNoise(m_noiseSeed, Time.timeSinceLevelLoad * m_noiseScale);
				float angle = 4f * Mathf.PI * sample;
				return new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)).normalized;
			}
		}
		#endregion

		public override void Setup(Vector3[] a_directions)
		{
			base.Setup(a_directions);
		}

		public void Init(float a_noiseScale, float a_noiseSeed = -1)
		{
			m_noiseScale = a_noiseScale;
			if (a_noiseSeed == -1)
			{
				m_noiseSeed = Random.Range(0, 999);
			}
		}
		public override float[] Compute()
		{
			float[] result = new float[directions.Length];

			Vector3 dir = RandomDirection;
			for (int i = 0; i < directions.Length; i++)
			{
				result[i] = Vector3.Dot(directions[i], dir);
				if (result[i] < 0) result[i] = 0;
			}
			return result;
		}
	}
}