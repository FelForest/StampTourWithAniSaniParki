using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialComplete : TutorialBase
{
    [SerializeField]
    Scene completeScene;

    [SerializeField]
    Scene nextScene;
    public override void Enter()
    {
        GameManager.Instance.SetIsSceneFinished(completeScene.SceneName,true);
    }

    public override void Execute(TutorialsManager manager)
    {
        manager.SetNextTutorial();
    }

    public override void Exit()
    {
        GameManager.LoadScene(nextScene.SceneName);
    }

}
