using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FCTools.Tutorial
{
	public class Tutorial_UIComponent : MonoBehaviour
	{
		#region inspector
		[SerializeField] private TextMeshProUGUI m_messageTMP;
		[SerializeField] private RectTransform m_messageBG;
		[SerializeField] private Button m_okBtn;
		[SerializeField] private RectTransform m_handDragAndDrop;
		[SerializeField] private RectTransform m_handClic;
		[SerializeField] private RectTransform[] m_arrowRectTransform;
		[SerializeField] private MMF_Player m_blackScreenFaedOut;
		[SerializeField] private MMF_Player m_popIn;
		#endregion

		#region properties
		private Tutorial m_tutorial;
		private Camera m_camera;
		private RectTransform m_canvasRect;
		public bool BlackScreenIsVisible => m_blackScreenIsVisible;
		private bool m_blackScreenIsVisible;
		#endregion

		public void Setup(Tutorial a_tutorial)
		{
			m_tutorial = a_tutorial;
			m_camera = Camera.main;
			m_canvasRect = GetComponent<RectTransform>();
			m_messageTMP.gameObject.SetActive(false);
			m_messageBG.gameObject.SetActive(false);
			m_handDragAndDrop.gameObject.SetActive(false);
			m_handClic.gameObject.SetActive(false);
			m_blackScreenFaedOut.Initialization();
			m_blackScreenFaedOut.gameObject.SetActive(true);
			ClearArrows();
			m_okBtn.gameObject.SetActive(false);
			m_blackScreenIsVisible = true;
		}
		public void Init()
		{
		}

		public void DisplayMessage(string a_message)
		{
			m_popIn.PlayFeedbacks();
			m_messageTMP.text = a_message;
			m_messageTMP.gameObject.SetActive(true);
			m_messageBG.gameObject.SetActive(true);
			m_okBtn.gameObject.SetActive(false);
		}

		public void ClearMessage()
		{
			m_messageTMP.gameObject.SetActive(false);
			m_messageBG.gameObject.SetActive(false);
			m_okBtn.gameObject.SetActive(false);
		}
		public void DisplayClicHand(RectTransform a_uiElement)
		{
			m_handClic.position = a_uiElement.position;
			m_handClic.gameObject.SetActive(true);
		}

		public void UpdateClicHandPosition(RectTransform a_uiElement)
		{
			m_handClic.position = a_uiElement.position;
		}
		public void DisplayDragAndDropHand(RectTransform a_uiElement)
		{
			m_handDragAndDrop.position = a_uiElement.position;
			m_handDragAndDrop.gameObject.SetActive(true);
		}
		public void DisplayDragAndDropHand(Vector3 a_worldPosition)
		{
			Vector2 viewportPosition = m_camera.WorldToViewportPoint(a_worldPosition);
			Vector2 screenPosition = new Vector2(
			((viewportPosition.x * m_canvasRect.sizeDelta.x) - (m_canvasRect.sizeDelta.x * 0.5f)),
			((viewportPosition.y * m_canvasRect.sizeDelta.y) - (m_canvasRect.sizeDelta.y * 0.5f)));
			m_handDragAndDrop.anchoredPosition = screenPosition;
			m_handDragAndDrop.gameObject.SetActive(true);
		}

		public void DisplayOkayButton(UnityEngine.Events.UnityAction a_onClicOk)
		{
			m_okBtn.onClick.RemoveAllListeners();
			m_okBtn.onClick.AddListener(a_onClicOk);
			m_okBtn.gameObject.SetActive(true);
		}


		public void DisplayArrow(RectTransform a_uiElement)
		{
			m_arrowRectTransform[0].position = a_uiElement.position;
			m_arrowRectTransform[0].gameObject.SetActive(true);
		}
		public void DisplayArrow(Vector3[] a_worldPosition)
		{
			for (int i = 0; i < a_worldPosition.Length; i++)
			{
				Vector2 viewportPosition = m_camera.WorldToViewportPoint(a_worldPosition[i]);
				Vector2 screenPosition = new Vector2(
				((viewportPosition.x * m_canvasRect.sizeDelta.x) - (m_canvasRect.sizeDelta.x * 0.5f)),
				((viewportPosition.y * m_canvasRect.sizeDelta.y) - (m_canvasRect.sizeDelta.y * 0.5f)));
				m_arrowRectTransform[i].anchoredPosition = screenPosition;
				m_arrowRectTransform[i].gameObject.SetActive(true);
			}
		}

		public void ClearArrows()
		{
			for (int i = 0; i < m_arrowRectTransform.Length; i++)
			{
				m_arrowRectTransform[i].gameObject.SetActive(false);
			}
		}
		public void ClearHand()
		{
			m_handDragAndDrop.gameObject.SetActive(false);
			m_handClic.gameObject.SetActive(false);
		}

		public void HideBlackScreen()
		{
			m_blackScreenFaedOut.PlayFeedbacks();
			m_blackScreenIsVisible = false;
		}
	}
}