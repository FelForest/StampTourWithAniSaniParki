using System.Collections;
using TMPro;
using UnityEngine;

namespace JicsawPuzzle
{
    public class GuideUIController : BaseInitializeObject
    {
        public Animator guideAnimator;
        public TMP_Text guideText;
        public float TypeTime = 1.0f;

        [Header("SFX")]
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip talkSFX;

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
            BubbleSetActive(isActive);
        }

        public void BubbleSetActive (bool isActive)
        {
            guideText.transform.parent.gameObject.SetActive(isActive);
        }

        public void Talk(string inputText)
        {
            // Debug.Log(inputText);
            SetActive(true);
            guideText.text = inputText;
            guideText.maxVisibleCharacters = 0;
            StartCoroutine(TypeAnimation(guideText));
        }

        public IEnumerator TalkIE(string inputText)
        {
            // Debug.Log(inputText);
            SetActive(true);
            guideText.text = inputText;
            guideText.maxVisibleCharacters = 0;
            yield return TypeAnimation(guideText);
        }

        IEnumerator TypeAnimation(TMP_Text tMP_Text)
        {
            int maxVisible = tMP_Text.text.Length;
            int curVisible = 0;
            float curTime = 0.0f;

            audioSource.PlayOneShot(talkSFX);

            while (curTime < 1.0f)
            {
                curTime += Time.deltaTime / TypeTime;

                curVisible++;
                tMP_Text.maxVisibleCharacters = Mathf.FloorToInt(maxVisible * curTime);

                yield return null;
            }
            
            tMP_Text.maxVisibleCharacters = maxVisible;
            audioSource.Stop();
        }

        public bool CheckCurrentStringSame(string inputString)
        {
            return inputString.Equals(guideText.text);
        }

        public void SwingAnimation(bool isActive)
        {
            guideAnimator.SetBool("handswing", isActive);
            Debug.Log(guideAnimator.GetBool("handswing"));
        }

        public void OAnimation()
        {
            guideAnimator.SetTrigger("O");
        }
    }
}

