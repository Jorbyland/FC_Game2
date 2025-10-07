using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace FCTools.MovementBehavior
{
	public class AvoidMovementBehavior : MovementBehavior
	{
		#region properties
		private LayerMask m_layerMask;
		private float m_raycastDistance;

		private float[] result;
		#endregion

		public override void Setup(Vector3[] a_directions)
		{
			base.Setup(a_directions);
		}
		public void Init(LayerMask a_layerMask, float a_raycastDistance)
		{
			m_layerMask = a_layerMask;
			m_raycastDistance = a_raycastDistance;
		}

		public override float[] Compute()
		{
			result = new float[directions.Length];
			List<int> hitDirectionIndex = new List<int>();
			for (int i = 0; i < directions.Length; i++)
			{
				if (Physics.Raycast(transform.position, directions[i], out RaycastHit hit,
						 m_raycastDistance, m_layerMask, QueryTriggerInteraction.Ignore))
				{
					result[i] = Mathf.InverseLerp(0, 1, hit.distance / m_raycastDistance);
					hitDirectionIndex.Add(i);
				}
				else
				{
					result[i] = 1;
				}
			}
			return result;
		}
	}
}