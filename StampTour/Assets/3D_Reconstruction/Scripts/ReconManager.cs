using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReconManager : MonoBehaviour
{
    [SerializeField]
    private List<ReconBase> Recons;

    private ReconBase currentTutorial = null;
    private int currentIndex = -1;


    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        SetNextTutorial();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTutorial != null)
        {
            currentTutorial.Execute(this);
        }
    }

    public void SetNextTutorial()
    {
        if (currentTutorial != null)
        {
            currentTutorial.Exit();
            SetTutorial(false);
        }

        if (currentIndex >= Recons.Count - 1)
        {
            CompletedAllTutorials();
            return;
        }

        currentIndex++;
        currentTutorial = Recons[currentIndex];

        SetTutorial(true);
        currentTutorial.Enter();
    }

    public void CompletedAllTutorials()
    {
        currentTutorial = null;

    }

    private void SetTutorial(bool isVisible)
    {
        currentTutorial.gameObject.SetActive(isVisible);
    }



}
