using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

namespace FCTools.JoystickInput
{
	public class JoystickInput : MonoBehaviour
	{
		private static JoystickInput _JI;
		#region inspector
		[SerializeField] private RectTransform m_movementJoystickRectT;
		[SerializeField] private Vector2 m_defaultJoystickAnchor;
		[SerializeField] private MMTouchRepositionableJoystick m_touchRepositionableJoystick;
		[SerializeField] private Transform m_knob;
		#endregion

		#region properties
		static public Vector2 MoveInput { get { return _JI.m_moveInput; } }
		private Vector2 m_moveInput;
		#endregion

		void Awake()
		{
			_JI = this;
		}

		void OnEnable()
		{
			m_touchRepositionableJoystick.Initialize();
			m_touchRepositionableJoystick.ResetJoystick();
			m_moveInput = Vector2.zero;
			ReplaceJoystick();
			m_knob.transform.localPosition = Vector3.zero;
		}
		void OnDisable()
		{
			m_moveInput = Vector2.zero;
		}
		public void OnMove(Vector2 a_value)
		{
			if (gameObject.activeSelf)
			{
				m_moveInput = a_value;
			}
		}

		public void ReplaceJoystick()
		{
			m_movementJoystickRectT.anchoredPosition = m_defaultJoystickAnchor;
		}
	}
}