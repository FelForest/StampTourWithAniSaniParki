using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class DialogueSystemEvent : MonoBehaviour
{
    public UnityEvent ForceEndEvent;

    [SerializeField]
    private List<Dialogue> dialogues = new List<Dialogue>();
    [SerializeField]
    private GameObject dialogueWindow;

    [SerializeField]
    private AudioClip clip;
    [SerializeField]
    private int currentDialogueIndex;
    private bool isFirst;

    public void Setup()
    {
        if(dialogueWindow != null) dialogueWindow.gameObject.SetActive(true);
        currentDialogueIndex = -1;
        isFirst = true;
    }

    public bool UpdateDialogue()
    {
        if (isFirst)
        {
            SetNextDialogue();
            isFirst = false;
        }
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                return HandelInput();
            }
        }
        else if(Input.GetMouseButtonDown(0))
        {
            return HandelInput();
        }
        return false;
    }

    private bool HandelInput()
    {
        if(dialogues.Count > currentDialogueIndex + 1)
        {
            SetNextDialogue();
        }
        else
        {
            EndDialogue();
            return true;
        }
        return false;
    }

    private void SetNextDialogue()
    {
        currentDialogueIndex++;
        GameManager.Instance.PlaySFXOneShot(clip);
        ForceEndEvent?.Invoke();
        Debug.Log(dialogues.Count);
        dialogues[currentDialogueIndex].PlayEvent?.Invoke();
    }

    private void EndDialogue()
    {
        if (dialogueWindow != null) dialogueWindow.gameObject.SetActive(false);
    }

    [System.Serializable]
    public struct Dialogue
    {
        public UnityEvent PlayEvent;
    }
}


