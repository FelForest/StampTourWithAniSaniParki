using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace JicsawPuzzle
{
    public class ChoiceElement : MonoBehaviour
    {
        public Button ChoiceButton;
        public TMP_Text ChoiceText;
        [HideInInspector]
        public RectTransform RectTransform;

        private Coroutine delayCoroutin;

        /// <summary>
        /// Initialize Element.
        /// (ResetButton)
        /// </summary>
        public void Init() {
            if (ChoiceButton == null)
            {
                ChoiceButton = GetComponent<Button>();
            }
            if (ChoiceText == null)
            {
                ChoiceText = GetComponentInChildren<TMP_Text>();
            }

            RectTransform = GetComponent<RectTransform>();

            ResetButton();
        }

        public void SetButton(string text, UnityAction action)
        {
            // Debug.Log($"[Test] ChoiceElement : SetButton text Value - {text}");
            SetText(text);
            ButtonAddAction(action);
        }

        public void ResetButton()
        {
            SetText("");
            ButtonRemoveAllAction();
        }

        public void ButtonAddAction(UnityAction action)
        {
            ChoiceButton.onClick.AddListener(action);
        }

        public void ButtonRemoveAction(UnityAction action)
        {
            ChoiceButton.onClick.RemoveListener(action);
        }

        public void ButtonRemoveAllAction()
        {
            ChoiceButton.onClick.RemoveAllListeners();
            ChoiceButton.onClick.AddListener(InputDelay);
        }

        public void SetText(string text)
        {
            ChoiceText.text = text;
        }

        protected void InputDelay()
        {
            if (delayCoroutin == null)
                delayCoroutin = StartCoroutine(InputDelayInternal());
        }

        protected IEnumerator InputDelayInternal()
        {
            ChoiceButton.interactable = false;
            yield return new WaitForSeconds(0.5f);
            ChoiceButton.interactable = true;
            delayCoroutin = null;
        }
    }
}