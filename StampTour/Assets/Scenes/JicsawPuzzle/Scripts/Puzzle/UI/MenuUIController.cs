using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace JicsawPuzzle
{
    public class MenuUIController : BaseInitializeObject
    {
        public string MainSceneName = "MainScene";
        public Button BackButton;

        [Header("SFX")]
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip clickSFX;

        private Coroutine coroutine;

        protected override void Start() 
        {
            if (BackButton != null)
            {
                BackButton.onClick.RemoveListener(ClickBackButton);
                BackButton.onClick.AddListener(ClickBackButton);
            }
        }

        public void ClickBackButton()
        {
            if (coroutine==null)
                coroutine = StartCoroutine(ClickDelay());
        }

        public IEnumerator ClickDelay()
        {
            audioSource.PlayOneShot(clickSFX);
            yield return new WaitForSeconds(0.5f);
            GameManager.LoadScene(MainSceneName);
        }
    }
}
