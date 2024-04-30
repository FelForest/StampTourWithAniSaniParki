using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JicsawPuzzle
{
    public class NextMissionButton : MonoBehaviour, IPlay
    {
        public Button NextButton;
        public AnimationCurve WidthAnimationCurve;
        public float WidthAnimationTime = 1.0f;

        private void Start() {
            if (NextButton == null)
            {
                NextButton = GetComponent<Button>();
            }
            NextButton.onClick.AddListener(NextMission);
            gameObject.SetActive(false);
        }

        public IEnumerator Play()
        {
            Debug.Log("AAFA");
            gameObject.SetActive(true);
            NextButton.enabled = false;
            NextButton.TryGetComponent(out RectTransform nextBtnRectTransfrom);
            yield return ChangingWidthAnimation(nextBtnRectTransfrom);
            NextButton.enabled = true;
        }
        
        protected IEnumerator ChangingWidthAnimation(RectTransform rectTransform)
        {
            Vector2 originSize = rectTransform.sizeDelta;
            Vector2 curSize = new Vector2(0.0f, originSize.y);
            rectTransform.sizeDelta = curSize;

            float curTime = 0.0f;
            while (curTime < 1.0f)
            {
                curTime += Time.deltaTime / WidthAnimationTime;
                curSize.x = originSize.x * WidthAnimationCurve.Evaluate(curTime);
                rectTransform.sizeDelta = curSize;

                yield return null;
            }
            rectTransform.sizeDelta = originSize;

        }

        public void NextMission()
        {
            Debug.Log("Next Mission");
        }
    }
}
