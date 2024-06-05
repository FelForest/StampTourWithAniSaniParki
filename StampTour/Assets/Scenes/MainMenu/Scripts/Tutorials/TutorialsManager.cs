using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialsManager : MonoBehaviour
{
    [SerializeField]
    private List<TutorialBase> tutorials;

    private TutorialBase currentTutorial = null;
    private int currentIndex = -1;


    // Start is called before the first frame update
    void Start()
    {
        SetNextTutorial();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTutorial != null)
        {
            currentTutorial.Execute(this);
        }
    }

    public void SetNextTutorial()
    {
        if(currentTutorial != null)
        {
            currentTutorial.Exit();
            SetTutorial(false);
        }

        if (currentIndex >= tutorials.Count - 1)
        {
            CompletedAllTutorials();
            return;
        }

        currentIndex++;
        currentTutorial = tutorials[currentIndex];

        SetTutorial(true);
        currentTutorial.Enter();
    }

    public void CompletedAllTutorials()
    {
        currentTutorial = null;
        GameManager.Instance.SetIsSceneFinished("Tutorial", true);
        Debug.Log("TutorialComplted");
        GameManager.LoadScene("MainScene");
    }

    private void SetTutorial(bool isVisible)
    {
        currentTutorial.gameObject.SetActive(isVisible);
    }
       


}
