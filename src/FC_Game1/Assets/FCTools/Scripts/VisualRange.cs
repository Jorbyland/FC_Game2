using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RumbleGuardian
{
	public class VisualRange : MonoBehaviour
	{
		#region inspector
		[SerializeField] private float m_range;
		[SerializeField] private Color m_color;
		#endregion

		void OnDrawGizmos()
		{
			Gizmos.color = m_color;
			Gizmos.DrawWireSphere(transform.position, m_range);
		}
	}
}