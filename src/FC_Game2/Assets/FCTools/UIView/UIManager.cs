using System.Collections.Generic;
using UnityEngine;

namespace FCTools.UIView
{
    public class UIManager : MonoBehaviour
    {
        #region Properties
        public static UIManager Instance { get; private set; }

        private Dictionary<string, IUIWindow> m_windows = new Dictionary<string, IUIWindow>();
        private Stack<IUIWindow> m_windowStack = new Stack<IUIWindow>();
        // [Header("Special UI")]
        // [SerializeField] private PromptWindow m_promptWindow;
        #endregion


        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            foreach (var window in GetComponentsInChildren<IUIWindow>(true))
            {
                if (!m_windows.ContainsKey(window.Id))
                    m_windows[window.Id] = window;
            }

            // if (m_promptWindow == null)
            //     m_promptWindow = GetComponentInChildren<PromptWindow>(true);
        }

        // ------------------
        // Fenêtres principales
        // ------------------

        public T Push<T>(string id) where T : class, IUIWindow
        {
            if (!m_windows.TryGetValue(id, out var window)) return null;

            // Fermer la fenêtre courante si nécessaire
            if (m_windowStack.Count > 0)
                m_windowStack.Peek().Close();

            window.Open();
            m_windowStack.Push(window);

            UpdatePauseState();

            return window as T;
        }

        public void Pop()
        {
            if (m_windowStack.Count == 0) return;

            var top = m_windowStack.Pop();
            top.Close();

            if (m_windowStack.Count > 0)
                m_windowStack.Peek().Open();

            UpdatePauseState();
        }

        public void ClearAll()
        {
            while (m_windowStack.Count > 0)
                m_windowStack.Pop().Close();

            UpdatePauseState();
        }

        public T Peek<T>() where T : class, IUIWindow
        {
            if (m_windowStack.Count == 0) return null;
            return m_windowStack.Peek() as T;
        }

        public bool IsOpen(string id) =>
            m_windows.TryGetValue(id, out var window) && window.IsOpen;

        // // ------------------
        // // Prompts
        // // ------------------

        // public void ShowPrompt(string message, Transform a_target)
        // {
        //     if (m_promptWindow != null)
        //         m_promptWindow.ShowPrompt(message, a_target);
        // }

        // public void HidePrompt()
        // {
        //     if (m_promptWindow != null)
        //         m_promptWindow.Close();
        // }

        // ------------------
        // Pause auto
        // ------------------

        private void UpdatePauseState()
        {
            Time.timeScale = m_windowStack.Count > 0 ? 0f : 1f;
        }
    }
}
