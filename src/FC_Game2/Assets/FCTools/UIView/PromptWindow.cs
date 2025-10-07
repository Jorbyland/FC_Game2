using TMPro;
using UnityEngine;

namespace FCTools.UIView
{
    public class PromptWindow : UIWindow
    {
        [SerializeField] private Camera m_camera;
        [SerializeField] private RectTransform m_rectTransform;
        [SerializeField] private Vector2 m_offset;
        [SerializeField] private TextMeshProUGUI m_messageTMP;
        private Transform m_target;


        public void ShowPrompt(string a_message, Transform a_target)
        {
            m_messageTMP.text = a_message;
            m_target = a_target;
            m_rectTransform.position = RectTransformUtility.WorldToScreenPoint(m_camera, a_target.position) + m_offset;
            Open();
        }
        private void LateUpdate()
        {
            if (!m_target) return;
            m_rectTransform.position = RectTransformUtility.WorldToScreenPoint(m_camera, m_target.position) + m_offset;
        }
    }
}
