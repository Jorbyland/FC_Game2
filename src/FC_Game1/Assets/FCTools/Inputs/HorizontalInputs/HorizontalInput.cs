using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools
{
	public class HorizontalInput : MonoBehaviour
	{
		public delegate void OnInputChangeDelegate(float a_value);
		public delegate void OnInputStartDelegate();
		#region inspector

		[SerializeField] private float m_sensibility;
		[Range(2, 20)]
		[SerializeField] private int m_framesDetection = 5;
		#endregion

		#region properties
		private bool m_isSliding;
		private float m_startClicPositionX;
		private Queue<float> m_lastPostionsQueue;
		public OnInputStartDelegate onInputStart;
		public OnInputChangeDelegate onInputChange;
		public OnInputStartDelegate onInputReleased;
		#endregion

		void Start()
		{

		}
		void Update()
		{
			if (Input.GetMouseButtonDown(0) && !m_isSliding)
			{
				m_isSliding = true;
				m_lastPostionsQueue = new Queue<float>();
				m_startClicPositionX = Input.mousePosition.x;
				m_lastPostionsQueue.Enqueue(Input.mousePosition.x);
				onInputStart?.Invoke();
			}
			else if (Input.GetMouseButton(0))
			{
				float input = -(m_startClicPositionX - Input.mousePosition.x) * m_sensibility;
				m_lastPostionsQueue.Enqueue(Input.mousePosition.x);
				if (m_lastPostionsQueue.Count > m_framesDetection)
				{
					m_lastPostionsQueue.Dequeue();
				}
				// float input = -(m_lastPostionsQueue.Peek() - Input.mousePosition.x) * m_sensibility;
				// input = Mathf.Clamp(input, -1, 1);
				onInputChange?.Invoke(input);
			}
			else if (Input.GetMouseButtonUp(0))
			{
				m_isSliding = false;
				onInputReleased?.Invoke();
			}
		}
	}
}