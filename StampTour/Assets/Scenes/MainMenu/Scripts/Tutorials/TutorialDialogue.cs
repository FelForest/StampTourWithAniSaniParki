using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueSystem))]
public class TutorialDialogue : TutorialBase
{
    private DialogueSystem dialogueSystem;
    public override void Enter()
    {
        isCompleted = false;
        panel.SetActive(true);

        dialogueSystem = GetComponent<DialogueSystem>();
        dialogueSystem.Setup();
    }

    public override void Execute(TutorialsManager manager)
    {
        isCompleted = dialogueSystem.UpdateDialogue();

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
