using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RivalArcher
{
	public class HorizontalRaycastInput : MonoBehaviour
	{
		public delegate void OnInputChangeDelegate(float a_valueX);
		public delegate void OnInputStartDelegate();
		#region inspector
		[SerializeField] private LayerMask m_terrainLayer;
		[SerializeField] private float m_maxRayDistance = 100;
		#endregion

		#region properties
		private Camera m_camera;
		public OnInputStartDelegate onInputStart;
		public OnInputChangeDelegate onInputChange;
		#endregion

		public void Start()
		{
			m_camera = Camera.main;
		}
		void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				onInputStart?.Invoke();
			}
			else if (Input.GetMouseButton(0))
			{
				RaycastHit hit;
				Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast(ray, out hit, m_maxRayDistance, m_terrainLayer))
				{
					// Transform objectHit = hit.transform;

					onInputChange?.Invoke(hit.point.x);
				}
			}
		}
	}
}