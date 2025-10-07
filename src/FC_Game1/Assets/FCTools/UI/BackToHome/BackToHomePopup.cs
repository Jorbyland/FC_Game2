using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RivalArcher
{
	public class BackToHomePopup : MonoBehaviour
	{
		public delegate void OnClicDelegate(bool a_confirm);
		#region inspector
		[SerializeField] private Button m_yesBtn;
		[SerializeField] private Button m_noBtn;
		#endregion

		#region properties
		public OnClicDelegate onClic;
		#endregion

		public void Setup()
		{
			m_yesBtn.onClick.AddListener(() => { OnClic(true); });
			m_noBtn.onClick.AddListener(() => { OnClic(false); });
			gameObject.SetActive(false);
		}

		private void OnClic(bool a_confirm)
		{
			gameObject.SetActive(false);
			onClic?.Invoke(a_confirm);
		}
	}
}