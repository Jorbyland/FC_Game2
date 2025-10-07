using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace FCTools
{
	public class CameraSwitcher : MonoBehaviour
	{
		private List<CinemachineCamera> m_virtualCameras = new List<CinemachineCamera>();
		[SerializeField] private CinemachineBlenderSettings m_jumpBlend;
		[SerializeField] private CinemachineBlenderSettings m_smoothBlend;
		[SerializeField] private CinemachineBrain m_cameraBrain;

		public void Setup()
		{
			m_virtualCameras = new List<CinemachineCamera>();
		}
		public void Init()
		{
		}

		public void SwitchCamera(CinemachineCamera a_virtualCamera, bool a_jump = false)
		{
			m_cameraBrain.CustomBlends = a_jump ? m_jumpBlend : m_smoothBlend;
			a_virtualCamera.Priority = 10;
			foreach (var item in m_virtualCameras)
			{
				if (item != a_virtualCamera)
				{
					item.Priority = 0;
				}
			}
		}

		public void Register(CinemachineCamera a_virtualCamera)
		{
			if (!m_virtualCameras.Contains(a_virtualCamera))
			{
				m_virtualCameras.Add(a_virtualCamera);
			}
		}
		public void Unregister(CinemachineCamera a_virtualCamera)
		{
			if (m_virtualCameras.Contains(a_virtualCamera))
			{
				m_virtualCameras.Remove(a_virtualCamera);
			}
		}
	}
}