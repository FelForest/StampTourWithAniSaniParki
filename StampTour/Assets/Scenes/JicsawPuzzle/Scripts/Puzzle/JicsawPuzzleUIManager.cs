using System.Collections;
using JicsawPuzzle;
using UnityEngine;
using UnityEngine.UI;

public class JicsawPuzzleUIManager : BaseInitializeObject
{
    [SerializeField, Tooltip("Using Top Img.")]
    protected Image UIBackground;

    [Header("Controller")]
    public GuideUIController GuideUIController;
    public ChoiceUIController ChoiceUIController;
    public MarkerUIController MarkerUIController;
    public ResultUIController ResultUIController;
    public MenuUIController MenuUIController;

    protected override void Start() {
        Init();
    }

    public void Init()
    {
        if (GuideUIController == null)
        {
            GuideUIController = GetComponentInChildren<GuideUIController>(true);
        }
        if (ChoiceUIController == null)
        {
            ChoiceUIController = GetComponentInChildren<ChoiceUIController>(true);
        }
        if (MarkerUIController == null)
        {
            MarkerUIController = GetComponentInChildren<MarkerUIController>(true);
        }
        if (ResultUIController == null)
        {
            ResultUIController = GetComponentInChildren<ResultUIController>(true);
        }
        if (MenuUIController == null)
        {
            MenuUIController = GetComponentInChildren<MenuUIController>(true);
        }

        UIBackground.gameObject.SetActive(true);
        UIBackground.enabled = true;

        StartCoroutine(InitInternal());
    }

    IEnumerator InitInternal()
    {
        yield return ActiveObject(GuideUIController);
        yield return ActiveObject(ChoiceUIController);
        yield return ActiveObject(MarkerUIController);
        yield return ActiveObject(ResultUIController);

        IsInitialized = true;
    }

    public void SetActiveUIBackground(bool isActive)
    {
        UIBackground.enabled = isActive;
    }

    #region Sequence
    public override IEnumerator Play()
    {
        yield return Sequence0();
    }

    private void CallSequence(IEnumerator sequenceIE)
    {
        if (sequenceIE != null)
        {
            StartCoroutine(sequenceIE);
        }
    }

    /// <summary>
    /// 첫 시나리오
    /// </summary>
    /// <returns></returns>
    public IEnumerator Sequence0()
    {
        GuideUIController.SetActive(true);
        ChoiceUIController.CrearChoice();
        yield return GuideUIController.TalkIE("전시장에 도착했어?\n<color=#FF5C00>( 1 전시장 )</color>");
        ChoiceUIController.SetActive(true);
        ChoiceUIController.GenerateChoice("응", ()=>CallSequence(Sequence2()));
        ChoiceUIController.GenerateChoice("아니", ()=>CallSequence(Sequence1()));
        yield return null;
    }

    /// <summary>
    /// 장소 미도착 시, 시나리오
    /// </summary>
    /// <returns></returns>
    public IEnumerator Sequence1()
    {
        ChoiceUIController.CrearChoice();
        yield return GuideUIController.TalkIE("도착하면 말해줘\n<color=#FF5C00>( 1 전시장 )</color>");
        ChoiceUIController.GenerateChoice("도착했어", ()=>CallSequence(Sequence2()));
        yield return null;
    }

    /// <summary>
    /// 카메라 탐색 시나리오
    /// </summary>
    /// <returns></returns>
    public IEnumerator Sequence2()
    {
        ChoiceUIController.CrearChoice();
        ChoiceUIController.SetActive(false);
        yield return GuideUIController.TalkIE("저렇게 생긴 마커를 찾아야 해!");
        MarkerUIController.SetActive(true);
        MarkerUIController.SetActivePreviewPanel(true);
        JicsawPuzzleManager.Instance.SetActiveARCamera(true);
        yield return new WaitUntil(()=>!MarkerUIController.PreviewActiveSelf());
        yield return GuideUIController.TalkIE("이미지를 다시 확인할려면, \"목표\"아래의 이미지를 눌러!");
        SetActiveUIBackground(false);
        JicsawPuzzleManager.Instance.CheckMarker();
    }

    /// <summary>
    /// 마커 확인중, 시나리오
    /// </summary>
    /// <returns></returns>
    public IEnumerator Sequence3()
    {
        GuideUIController.Talk("지금 마커를 확인중이야~");
        yield return null;
    }

    /// <summary>
    /// 마커 실패, 시나리오
    /// </summary>
    /// <returns></returns>
    public IEnumerator Sequence4()
    {
        MarkerUIController.SetActiveFailedPanel(true);
        yield return GuideUIController.TalkIE("이게 아닌거 같아\n다시 한 번 찾아보자");
        yield return new WaitForSeconds(2.0f);
    }

    /// <summary>
    /// 마커 성공, 시나리오
    /// </summary>
    /// <returns></returns>
    public IEnumerator Sequence5()
    {
        MarkerUIController.SetActive(false);
        yield return GuideUIController.TalkIE("좋아 지도를 발견했어~");
        yield return new WaitForSeconds(2.0f);
        yield return GuideUIController.TalkIE("지도를 눌러서 미션을 완료하자구");
        yield return new WaitForSeconds(2.0f);
    }

    /// <summary>
    /// Puzzle Start
    /// </summary>
    /// <returns></returns>
    public IEnumerator Sequence6()
    {
        yield return GuideUIController.TalkIE("이런!\n지도가 산산조각 나버렸어");
        yield return new WaitForSeconds(2.0f);
        yield return GuideUIController.TalkIE("망가진 지도를 다시 맞춰줘");
    }

    /// <summary>
    /// Puzzle Clear
    /// </summary>
    /// <returns></returns>
    public IEnumerator Sequence7()
    {
        yield return GuideUIController.TalkIE("좋아!\n완벽해");
        yield return new WaitForSeconds(2.0f);
        JicsawPuzzleManager.Instance.SetActiveARCamera(false);
        yield return ResultUIController.Play();
    }

    #endregion

}
