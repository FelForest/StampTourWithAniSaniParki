using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialZoom : TutorialBase
{
    [SerializeField]
    private GameObject zoomObject;

    private Vector3 perScale;
    public override void Enter()
    {
        perScale = zoomObject.transform.localScale;
        isCompleted = false;
        panel.SetActive(true);
    }

    public override void Execute(TutorialsManager manager)
    {
        Vector3 currentScale = zoomObject.transform.localScale;
        if (currentScale != perScale)
        {
            isCompleted = true;
        }
        if (isCompleted)
        {
            manager.SetNextTutorial();
        }
    }

    public override void Exit()
    {
        panel.SetActive(false);
    }
}
