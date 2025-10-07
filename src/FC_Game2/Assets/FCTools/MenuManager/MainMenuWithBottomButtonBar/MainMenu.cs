using System.Collections;
using System.Collections.Generic;
using RivalVillages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FCTools.Menus
{
	public class MainMenu : Menu
	{
		[System.Serializable]
		public class SubMenuOption
		{
			public Option Option => m_option;
			[SerializeField] private Option m_option;
			public Button ActivedButton => m_activedButton;
			[SerializeField] private Button m_activedButton;
			public Button DisactivedButton => m_disactivedButton;
			[SerializeField] private Button m_disactivedButton;
			public Button LockedButton => m_lockedButton;
			[SerializeField] private Button m_lockedButton;
			public Transform Notif => m_notif;
			[SerializeField] private Transform m_notif;
			public bool IsLocked => m_isLocked;
			[SerializeField] private bool m_isLocked;
			public bool HaveNotif => m_haveNotif;
			[SerializeField] private bool m_haveNotif;
			public SubMenu SubMenu => m_subMenu;
			[SerializeField] private SubMenu m_subMenu;

			private System.Action<Option> m_onSelect;

			public void Setup(System.Action<Option> a_onSelect)
			{
				m_onSelect = a_onSelect;
				m_disactivedButton.onClick.AddListener(() => { m_onSelect?.Invoke(m_option); });
			}

			public void UpdateState(bool a_isActive, bool a_isLocked, bool a_haveNotif)
			{
				m_activedButton.gameObject.SetActive(a_isActive);
				m_disactivedButton.gameObject.SetActive(!a_isActive && !a_isLocked);
				if (m_lockedButton != null)
				{
					m_lockedButton.gameObject.SetActive(a_isLocked && !a_isActive);
					m_lockedButton.interactable = false;
				}
				// if (m_notif != null)
				// {
				// 	if (m_subMenu)
				// 		Debug.Log($"UpdateState ({m_subMenu.gameObject.name})==>>  {a_haveNotif && !a_isActive}");
				// 	m_notif.gameObject.SetActive(a_haveNotif && !a_isActive);
				// }
				if (m_subMenu != null)
				{
					if (a_isActive)
					{
						m_subMenu.Open();
					}
					else if (m_subMenu.gameObject.activeSelf)
					{
						m_subMenu.Close();
					}
				}
			}
		}

		public enum Option { Shop, Cards, Battle, Quest, Leaderboard }

		public delegate void OnClicDelegate();

		#region inspector
		[SerializeField] MainMenu_Controller m_controller;
		public MainMenu_Controller Controller => m_controller;
		[SerializeField] MainMenu_SubMenuComponent m_subMenuComponent;
		public MainMenu_SubMenuComponent SubMenuComponent => m_subMenuComponent;
		[SerializeField] MainMenu_CurrenciesComponent m_currenciesComponent;
		public MainMenu_CurrenciesComponent CurrenciesComponent => m_currenciesComponent;

		[Space()]
		[SerializeField] private Button m_playBtn;
		public Button PlayBtn => m_playBtn;
		[Space()]
		[SerializeField] private SubMenu[] m_subMenus;
		public SubMenu[] SubMenus => m_subMenus;
		[Space()]
		[SerializeField] private SubMenuOption[] m_subMenuOptions;
		public SubMenuOption[] SubMenuOptions => m_subMenuOptions;
		#endregion

		#region properties
		public OnClicDelegate onClicPlay;
		public OnClicDelegate onGameStart;
		#endregion

		public override void Setup()
		{
			base.Setup();
			m_subMenuComponent.Setup(this);
			m_currenciesComponent.Setup(this);
			for (int i = 0; i < m_subMenus.Length; i++)
			{
				m_subMenus[i].Setup(this);
			}
			m_controller.Setup(this);

			m_playBtn.onClick.AddListener(() =>
			{
				HapticManager.Play(HapticManager.Type.LIGHT);
				onClicPlay?.Invoke();
			});

			Init();
		}
		public void Init()
		{
			m_subMenuComponent.Init();
			m_currenciesComponent.Init();
			for (int i = 0; i < m_subMenus.Length; i++)
			{
				m_subMenus[i].Init();
			}
			m_controller.Init();
		}

		public override void Open()
		{
			base.Open();
			for (int i = 0; i < m_subMenuOptions.Length; i++)
			{
				if (m_subMenuOptions[i].HaveNotif)
				{
					m_subMenuOptions[i].Notif.gameObject.SetActive(m_subMenuOptions[i].SubMenu.HaveNotification());
				}
			}
		}

		void OnDestroy()
		{
			m_controller.DoOnDisable();
		}
	}
}