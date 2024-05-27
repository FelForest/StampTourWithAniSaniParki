using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTouch : TutorialBase
{
    [SerializeField]
    private Button button;

    public override void Enter()
    {
        isCompleted = false;
        SafePanelActive(true);
        button.onClick.AddListener(TouchedButton);
    }

    public override void Execute(TutorialsManager manager)
    {
        if (isCompleted)
        {
            manager.SetNextTutorial();
        }
    }

    public override void Exit()
    {
        SafePanelActive(false);
    }

    private void TouchedButton()
    {
        isCompleted = true;
    }
}
