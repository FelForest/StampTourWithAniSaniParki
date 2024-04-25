using UnityEngine;

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
                _instance = GameObject.FindObjectOfType<AudioController>();
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

    public AudioClip TerminalBGM_Clip;
    public AudioClip GardenBGM_Clip;

    private void Awake()
    {
        SingletonAwake();
    }

    private void Start() {
        PlayBGM(TerminalBGM_Clip);
    }

    public void PlayBGM(AudioClip clip = null)
    {
        if (clip)
        {
            Source_BGM.clip = clip;
        }

        Source_BGM.Play();
    }

    public void PlaySFXOneShot(AudioClip clip)
    {
        if (clip)
        {
            Source_SFX.PlayOneShot(clip);
        }
    }
}
