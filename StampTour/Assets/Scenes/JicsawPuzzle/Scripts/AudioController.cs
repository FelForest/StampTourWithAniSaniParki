using UnityEngine;

namespace JicsawPuzzle
{
    public class AudioController : MonoBehaviour
    {
    #region Singleton
        private static AudioController _instance;
        public static AudioController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<AudioController>();

                    if (_instance == null)
                    {
                        GameObject obj = new GameObject("AudioController");
                        _instance = obj.AddComponent<AudioController>();

                        DontDestroyOnLoad(obj);
                    }
                }

                return _instance;
            }
        }

        private void SingletonAwake() 
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);

                return;
            }
        }
    #endregion

        public AudioSource Source_BGM;
        public AudioSource Source_SFX;

        private void Awake()
        {
            SingletonAwake();
        }

        /// <summary>
        /// Change and Play BGM AudioSource using clip.
        /// </summary>
        /// <param name="clip">Target AudioClip</param>
        public void PlayBGM(AudioClip clip = null)
        {
            if (clip)
            {
                Source_BGM.clip = clip;
            }

            Source_BGM.Play();
        }

        /// <summary>
        /// Change and Play OneShot SFX AudioSource using clip.
        /// </summary>
        /// <param name="clip">Target AudioClip</param>
        public void PlaySFXOneShot(AudioClip clip)
        {
            if (clip)
            {
                Source_SFX.PlayOneShot(clip);
            }
        }
    }
}