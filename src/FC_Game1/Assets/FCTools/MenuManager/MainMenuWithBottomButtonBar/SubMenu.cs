using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools.Menus
{
	public class SubMenu : MonoBehaviour
	{
		#region inspector
		[SerializeField] private Transform m_notifTransform;
		#endregion

		#region properties
		protected MainMenu mainMenu;
		#endregion

		public virtual void Setup(MainMenu a_mainMenu)
		{
			mainMenu = a_mainMenu;
		}
		public virtual void Init()
		{

		}

		public virtual void Open()
		{
			gameObject.SetActive(true);
			if (m_notifTransform)
			{
				m_notifTransform.gameObject.SetActive(false);
			}
		}
		public virtual void Close()
		{
			gameObject.SetActive(false);

			if (m_notifTransform)
			{
				Debug.Log("SubMenu Close: HaveNotif = " + HaveNotification());
				m_notifTransform.gameObject.SetActive(HaveNotification());
			}
		}

		public virtual bool HaveNotification()
		{
			return false;
		}
		protected virtual void UpdateNotification()
		{
			if (m_notifTransform)
			{
				Debug.Log("UpdateNotification SubMenu: HaveNotif = " + HaveNotification());
				m_notifTransform.gameObject.SetActive(HaveNotification());
			}
		}
	}
}