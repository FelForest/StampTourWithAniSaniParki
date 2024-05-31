using System.Collections;
using TMPro;
using UnityEngine;

namespace JicsawPuzzle
{
    public class ResultInfoComponent : MonoBehaviour, IPlay
    {
        public Animator StampAnimator;
        protected int stampOutInAnimHash = Animator.StringToHash("Stamp_OutIn");
        public TMP_Text BodyText;
        public float TypeTime = 2.0f;

        [Header("SFX")]
        [SerializeField] AudioSource audioSourceStamp;
        [SerializeField] AudioSource audioSourceType;
        [SerializeField] AudioClip stampSFX;
        [SerializeField] AudioClip typeSFX;


        private void Start() {
            if (StampAnimator == null)
            {
                StampAnimator = GetComponent<Animator>();
            }

            if (BodyText == null)
            {
                BodyText = GetComponentsInChildren<TMP_Text>()[1];
            }

            Init();
        }

        public void Init()
        {
            StampAnimator.transform.localScale = new Vector3(0,0,1);
            BodyText.maxVisibleCharacters = 0;
        }

        public IEnumerator Play()
        {
            yield return OnAnimating(StampAnimator, stampOutInAnimHash);
            yield return TypeAnimation(BodyText);
        }

        IEnumerator OnAnimating(Animator animator, int animationHash)
        {
            animator.Play(animationHash);
            audioSourceStamp.PlayOneShot(stampSFX);
            yield return new WaitForEndOfFrame();
 
            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                yield return null;
        }

        IEnumerator TypeAnimation(TMP_Text tMP_Text)
        {
            int maxVisible = tMP_Text.text.Length;
            int curVisible = 0;
            float curTime = 0.0f;

            audioSourceType.PlayOneShot(typeSFX);

            while (curTime < 1.0f)
            {
                curTime += Time.deltaTime / TypeTime;

                curVisible++;
                tMP_Text.maxVisibleCharacters = Mathf.FloorToInt(maxVisible * curTime);

                yield return null;
            }
            
            tMP_Text.maxVisibleCharacters = maxVisible;
            audioSourceType.Stop();
        }
    }
}
