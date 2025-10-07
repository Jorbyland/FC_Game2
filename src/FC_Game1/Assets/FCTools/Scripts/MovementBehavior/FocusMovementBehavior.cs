using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools.MovementBehavior
{
	public class FocusMovementBehavior : MovementBehavior
	{
		#region properties
		private Vector3 m_targetPosition;
		private bool m_haveTargetPosition;
		#endregion

		public override void Setup(Vector3[] a_directions)
		{
			base.Setup(a_directions);
		}

		public void Init(Vector3 a_targetPosition)
		{
			m_targetPosition = a_targetPosition;
			m_haveTargetPosition = true;
		}

		public override float[] Compute()
		{
			if (m_haveTargetPosition)
			{
				float[] result = new float[directions.Length];
				Vector3 dir = Vector3.Normalize(m_targetPosition - transform.position);
				for (int i = 0; i < directions.Length; i++)
				{
					result[i] = Vector3.Dot(directions[i], dir);
					if (result[i] < 0) result[i] = 0;
				}
				return result;
			}
			return new float[directions.Length];
		}
	}
}