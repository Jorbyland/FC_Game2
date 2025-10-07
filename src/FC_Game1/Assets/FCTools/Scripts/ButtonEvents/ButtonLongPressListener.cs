using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FCTools
{
    [RequireComponent(typeof(Button))]
    public class ButtonLongPressListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public delegate void OnPressedRatioChangeDelegate(float a_ratio);

        #region inspector
        [Tooltip("Hold duration in seconds")]
        [Range(0.3f, 5f)] public float holdDuration = 0.5f;
        public UnityEvent onLongPress;
        public UnityEvent onCancel;
        #endregion

        #region properties
        public OnPressedRatioChangeDelegate onPressedRatioChange;
        private bool isPointerDown = false;
        private bool isLongPressed = false;
        private DateTime pressTime;

        private Button button;

        private WaitForSeconds delay;
        #endregion

        private void Awake()
        {
            button = GetComponent<Button>();
            delay = new WaitForSeconds(0.02f);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isPointerDown = true;
            pressTime = DateTime.Now;
            StartCoroutine(Timer());
        }


        public void OnPointerUp(PointerEventData eventData)
        {
            if (!isLongPressed)
            {
                onCancel?.Invoke();
            }
            isPointerDown = false;
            isLongPressed = false;
        }

        private IEnumerator Timer()
        {
            while (isPointerDown && !isLongPressed)
            {
                double elapsedSeconds = (DateTime.Now - pressTime).TotalSeconds;

                if (elapsedSeconds >= holdDuration)
                {
                    isLongPressed = true;
                    if (button.interactable)
                    {
                        onLongPress?.Invoke();
                        isPointerDown = false;
                        isLongPressed = false;
                    }

                    yield break;
                }
                else
                {
                    onPressedRatioChange?.Invoke((float)elapsedSeconds / holdDuration);
                }

                yield return delay;
            }
        }
    }
}
