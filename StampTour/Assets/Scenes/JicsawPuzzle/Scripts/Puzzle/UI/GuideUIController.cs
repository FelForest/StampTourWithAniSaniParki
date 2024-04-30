using System.Collections;
using TMPro;
using UnityEngine;

namespace JicsawPuzzle
{
    public class GuideUIController : BaseUIController
    {
        public Animator guideAnimator;
        public TMP_Text guideText;
        public float TypeTime = 1.0f;

        protected override void Start() {
            IsInitialized = true;
        }

        public override IEnumerator Play()
        {
            throw new System.NotImplementedException();
        }

        public override void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
        public void Talk(string inputText)
        {
            guideText.text = inputText;
            guideText.maxVisibleCharacters = 0;
            StartCoroutine(TypeAnimation(guideText));
        }

        IEnumerator TypeAnimation(TMP_Text tMP_Text)
        {
            int maxVisible = tMP_Text.text.Length;
            int curVisible = 0;
            float curTime = 0.0f;

            while (curTime < 1.0f)
            {
                curTime += Time.deltaTime / TypeTime;

                curVisible++;
                tMP_Text.maxVisibleCharacters = Mathf.FloorToInt(maxVisible * curTime);

                yield return null;
            }
            
            tMP_Text.maxVisibleCharacters = maxVisible;
        }
    }
}

