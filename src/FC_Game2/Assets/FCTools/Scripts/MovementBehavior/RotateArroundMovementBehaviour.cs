using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools.MovementBehavior
{
	public class RotateArroundMovementBehaviour : MovementBehavior
	{
		#region properties
		private Transform m_target;
		private bool m_haveTarget;
		#endregion

		public override void Setup(Vector3[] a_directions)
		{
			base.Setup(a_directions);
		}
		public void Init(Transform a_target)
		{
			m_target = a_target;
			m_haveTarget = a_target != null;
		}

		public override float[] Compute()
		{
			if (m_haveTarget)
			{
				float[] result = new float[directions.Length];
				Vector3 dirToTarget = Vector3.Normalize(m_target.position - transform.position);
				Vector3 dirToMove = Vector3.Cross(dirToTarget, Vector3.up);
				for (int i = 0; i < directions.Length; i++)
				{
					result[i] = Vector3.Dot(directions[i], dirToMove);
				}
				return result;
			}
			return new float[directions.Length];
		}
	}
}