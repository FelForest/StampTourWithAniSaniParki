using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Button upButton;
    public Button downButton;
    public VideoClip[] videoClips;
    private int currentChannel = 0;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeRight;
        // 각 버튼에 클릭 이벤트를 연결합니다.
        upButton.onClick.AddListener(NextChannel);
        downButton.onClick.AddListener(PreviousChannel);

        // 초기 비디오 재생
        if (videoClips.Length > 0)
        {
            videoPlayer.clip = videoClips[currentChannel];
            videoPlayer.Play();
        }

        if (!GameManager.Instance.GetIsSceneFinished("TV"))
        {
            GameManager.Instance.SetIsSceneFinished("TV", true);
        }
    }

    void NextChannel()
    {
        currentChannel = (currentChannel + 1) % videoClips.Length;
        ChangeChannel(currentChannel);
    }

    void PreviousChannel()
    {
        currentChannel = (currentChannel - 1 + videoClips.Length) % videoClips.Length;
        ChangeChannel(currentChannel);
    }

    void ChangeChannel(int channelIndex)
    {
        if (channelIndex < 0 || channelIndex >= videoClips.Length)
        {
            Debug.LogError("Invalid channel index");
            return;
        }

        videoPlayer.clip = videoClips[channelIndex];
        videoPlayer.Play();
    }
}
