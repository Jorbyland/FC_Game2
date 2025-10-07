using TMPro;
using UnityEngine;

namespace Game
{
    public class GUIPrompts : GUIPanel
    {

        [SerializeField] private Camera m_camera;
        [SerializeField] private RectTransform m_rectTransform;
        [SerializeField] private Vector2 m_offset;
        [SerializeField] private TextMeshProUGUI m_messageTMP;
        private Transform m_target;

        public override void Bind(GameContextScriptable context, GameState state)
        {
            gameObject.SetActive(false);
        }
        public override void Refresh() { }

        public void Show(string a_message, Transform a_target)
        {
            m_messageTMP.text = a_message;
            m_target = a_target;
            m_rectTransform.position = RectTransformUtility.WorldToScreenPoint(m_camera, a_target.position) + m_offset;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Clear() => Hide();

        private void LateUpdate()
        {
            if (!m_target) return;
            m_rectTransform.position = RectTransformUtility.WorldToScreenPoint(m_camera, m_target.position) + m_offset;
        }
    }
}
