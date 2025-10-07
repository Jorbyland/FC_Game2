using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools.Tutorial
{
	public class Tutorial_MarkerComponent : MonoBehaviour
	{
		#region inspector
		[SerializeField] private GameObject m_arrowMarker;
		#endregion

		#region properties
		private Tutorial m_tutorial;
		#endregion

		public void Setup(Tutorial a_tutorial)
		{
			m_tutorial = a_tutorial;
			m_arrowMarker.gameObject.SetActive(false);
		}
		public void Init()
		{

		}

		public void DisplayMarkerAtPosition(Vector3 a_position)
		{
			transform.position = a_position;
			m_arrowMarker.SetActive(true);
		}
		public void HideMarker()
		{
			m_arrowMarker.SetActive(false);
		}
	}
}