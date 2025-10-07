using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools.Menus
{
	public class MainMenu_Controller : MonoBehaviour
	{
		#region inspector

		#endregion

		#region properties
		private MainMenu m_mainMenu;
		#endregion

		public void Setup(MainMenu a_mainMenu)
		{
			m_mainMenu = a_mainMenu;
			m_mainMenu.SubMenuComponent.onOptionSelected += OnOptionSelected;
		}
		public void Init()
		{

		}

		public void DoOnDisable()
		{
			m_mainMenu.SubMenuComponent.onOptionSelected -= OnOptionSelected;
		}

		private void OnOptionSelected(MainMenu.Option a_option)
		{

		}
	}
}