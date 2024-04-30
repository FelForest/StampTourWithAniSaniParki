using System.Collections;
using System.Collections.Generic;
using JicsawPuzzle;
using Unity.VisualScripting;
using UnityEngine;

public class JicsawPuzzleUIManager : MonoBehaviour
{
    public GuideUIController GuideUIController;
    public ChoiceUIController ChoiceUIController;
    public MarkerUIController MarkerUIController;
    public ResultUIController ResultUIController;
    public MenuUIController MenuUIController;

    private void Start() {
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

        StartCoroutine(InitInternal());
    }

    IEnumerator InitInternal()
    {
        yield return ActiveObject(GuideUIController);
        yield return ActiveObject(ChoiceUIController);
        yield return ActiveObject(MarkerUIController);
        yield return ActiveObject(ResultUIController);

        yield return Play();
    }

    IEnumerator ActiveObject(BaseUIController controller)
    {
        controller.gameObject.SetActive(true);
        yield return new WaitUntil(()=> controller.IsInitialized);
        controller.gameObject.SetActive(false);
    }

    IEnumerator Play()
    {
        yield return new WaitForSeconds(1.0f);
        yield return ChoiceUIController.Play();
        yield return MarkerUIController.Play();
        yield return ResultUIController.Play();
    }
}
