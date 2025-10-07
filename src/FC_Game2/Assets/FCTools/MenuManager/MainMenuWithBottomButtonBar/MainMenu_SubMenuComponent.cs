using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FCTools.Menus
{
	public class MainMenu_SubMenuComponent : MonoBehaviour
	{
		public delegate void OnOpenMenu(MainMenu.Option a_option);
		#region inspector
		#endregion

		#region properties
		private MainMenu m_mainMenu;
		public OnOpenMenu onOptionSelected;
		private MainMenu.Option m_currentOption = MainMenu.Option.Shop;
		#endregion

		public void Setup(MainMenu a_mainMenu)
		{
			m_mainMenu = a_mainMenu;

			for (int i = 0; i < m_mainMenu.SubMenuOptions.Length; i++)
			{
				m_mainMenu.SubMenuOptions[i].Setup(SelectOption);
			}
		}
		public void Init()
		{
			SelectOption(MainMenu.Option.Battle);
		}

		public void SelectOption(MainMenu.Option a_option)
		{
			if (m_currentOption == a_option) return;

			m_currentOption = a_option;

			HapticManager.Play(HapticManager.Type.LIGHT);
			ActiveSubMenu(m_currentOption);
			onOptionSelected?.Invoke(a_option);
		}
		public void ActiveSubMenu(MainMenu.Option a_option)
		{
			for (int i = 0; i < m_mainMenu.SubMenuOptions.Length; i++)
			{
				bool isActive = m_mainMenu.SubMenuOptions[i].Option == a_option;
				bool isLocked = m_mainMenu.SubMenuOptions[i].IsLocked;
				bool haveNotif = m_mainMenu.SubMenuOptions[i].HaveNotif && m_mainMenu.SubMenuOptions[i].SubMenu.HaveNotification();
				m_mainMenu.SubMenuOptions[i].UpdateState(isActive, isLocked, haveNotif);
			}
		}
	}
}