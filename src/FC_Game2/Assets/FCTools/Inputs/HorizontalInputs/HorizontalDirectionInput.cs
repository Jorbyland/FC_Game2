using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RivalArcher
{
	public class HorizontalDirectionInput : MonoBehaviour
	{
		public delegate void OnInputChangeDelegate(float a_value);
		public delegate void OnInputStartDelegate();
		#region inspector

		[SerializeField] private float m_sensibility;
		[Range(2, 20)]
		[SerializeField] private int m_framesDetection = 3;
		#endregion

		#region properties
		private bool m_isSliding;
		private Queue<float> m_lastPostionsQueue;
		private float m_delta;
		public OnInputStartDelegate onInputStart;
		public OnInputChangeDelegate onInputChange;
		#endregion


		void Update()
		{
			if (Input.GetMouseButtonDown(0) && !m_isSliding)
			{
				m_isSliding = true;
				m_lastPostionsQueue = new Queue<float>();
				m_delta = 0;
				m_lastPostionsQueue.Enqueue(Input.mousePosition.x);
				onInputStart?.Invoke();
			}
			else if (Input.GetMouseButton(0))
			{
				m_lastPostionsQueue.Enqueue(Input.mousePosition.x);
				if (m_lastPostionsQueue.Count > m_framesDetection)
				{
					m_lastPostionsQueue.Dequeue();
				}
				float delta = -(m_lastPostionsQueue.Peek() - Input.mousePosition.x);
				if (delta < -m_sensibility)
				{
					m_delta = -1;
				}
				else if (delta > m_sensibility)
				{
					m_delta = 1;
				}
				onInputChange?.Invoke(m_delta);
			}
			else if (Input.GetMouseButtonUp(0))
			{
				m_isSliding = false;
			}
		}
	}
}