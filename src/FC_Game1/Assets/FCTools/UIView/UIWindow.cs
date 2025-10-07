using UnityEngine;

namespace FCTools.UIView
{
    public abstract class UIWindow : MonoBehaviour, IUIWindow
    {
        [SerializeField] private string m_id;
        public string Id => m_id;

        protected CanvasGroup canvasGroup;

        public bool IsOpen { get; private set; }

        protected virtual void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
            CloseImmediate();
        }

        public virtual void Open()
        {
            gameObject.SetActive(true);
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            IsOpen = true;
        }

        public virtual void Close()
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            gameObject.SetActive(false);
            IsOpen = false;
        }

        private void CloseImmediate()
        {
            gameObject.SetActive(false);
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            IsOpen = false;
        }
    }
}
