using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RivalVillages
{
	public class LookCamera : MonoBehaviour
	{
		#region inspector

		#endregion

		#region properties
		private Transform m_cameraTransform;
		#endregion

		void Awake()
		{
			m_cameraTransform = Camera.main.transform;
			transform.rotation = Quaternion.Euler(m_cameraTransform.rotation.eulerAngles.x, 0, 0);
		}

		// void Update()
		// {
		// 	transform.LookAt(m_cameraTransform.transform, Vector3.up);
		// }
	}
}