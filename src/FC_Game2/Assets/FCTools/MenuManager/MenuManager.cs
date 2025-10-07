using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools.Menus
{
	public class MenuManager : MonoBehaviour
	{
		static public MenuManager _MM { get; private set; }

		#region inspector
		public Menu StartingMenu => m_startingMenu;
		[SerializeField] private Menu m_startingMenu;

		public Menu[] Menus => m_menus;
		[SerializeField] private Menu[] m_menus;
		#endregion

		#region properties
		static public Menu CurrentMenu => _MM.m_currentMenu;
		private Menu m_currentMenu;
		private readonly Stack<Menu> m_history = new Stack<Menu>();
		#endregion

		public void Setup()
		{
			if (_MM != null && _MM != this)
				Destroy(this);
			else
				_MM = this;
		}

		public void Init()
		{
			for (int i = 0; i < m_menus.Length; i++)
			{
				m_menus[i].Setup();
				m_menus[i].CloseImmediate();
			}
			if (m_startingMenu != null)
			{
				OpenMenu(m_startingMenu, true);
			}
		}

		static public T GetMenu<T>() where T : Menu
		{
			for (int i = 0; i < _MM.m_menus.Length; i++)
			{
				if (_MM.m_menus[i] is T tMenu)
				{
					return tMenu;
				}
			}
			return null;
		}

		static public void OpenMenu<T>(bool a_remember = true) where T : Menu
		{
			if (_MM.m_currentMenu is T)
			{
				return;
			}
			for (int i = 0; i < _MM.m_menus.Length; i++)
			{
				if (_MM.m_menus[i] is T)
				{
					if (_MM.m_currentMenu != null)
					{
						if (a_remember)
						{
							_MM.m_history.Push(_MM.m_currentMenu);
						}
						_MM.m_currentMenu.Close();
					}
					_MM.m_menus[i].Open();
					_MM.m_currentMenu = _MM.m_menus[i];
				}
			}
		}

		static public void OpenMenu(Menu a_menu, bool a_remember = true)
		{
			if (_MM.m_currentMenu != null)
			{
				if (a_remember)
				{
					_MM.m_history.Push(_MM.m_currentMenu);
				}
				_MM.m_currentMenu.Close();
			}
			a_menu.Open();

			_MM.m_currentMenu = a_menu;
		}

		static public void OpenLast()
		{
			if (_MM.m_history.Count != 0)
			{
				OpenMenu(_MM.m_history.Pop(), false);
			}
		}
		static public void CloseMenu()
		{
			if (_MM.m_currentMenu != null)
			{
				_MM.m_currentMenu.Close();
				_MM.m_currentMenu = null;
			}
		}
		static public void ForceCloseMenu()
		{
			if (_MM.m_currentMenu != null)
			{
				_MM.m_currentMenu.CloseImmediate();
				_MM.m_currentMenu = null;
			}
		}
	}
}