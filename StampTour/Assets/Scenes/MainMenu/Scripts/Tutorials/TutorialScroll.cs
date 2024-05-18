using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScroll : TutorialBase
{
    [SerializeField]
    private ScrollRect scroll;
    [SerializeField]
    private float percent;

    public override void Enter()
    {
        isCompleted = false;
        panel.SetActive(true);

        percent = percent == 0.0f ? 0.5f : percent;

        scroll.onValueChanged.AddListener(OnScrollChanged);
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
        scroll.onValueChanged.AddListener(OnScrollChanged);
        panel.SetActive(false);
    }

    private void OnScrollChanged(Vector2 position)
    {
        float distance = Mathf.Max(position.x, position.y);
        if(distance > percent)
        {
            isCompleted = true;
        }
    }

    

}
