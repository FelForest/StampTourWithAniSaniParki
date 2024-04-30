using System.Collections;
using System.Collections.Generic;
using JicsawPuzzle;
using UnityEngine;

public class JicsawPuzzleUIController : MonoBehaviour
{
    public ResultUIComponent ResultUIComponent;

    private void Start() {
        if (ResultUIComponent == null)
        {
            ResultUIComponent = GetComponentInChildren<ResultUIComponent>();
        }

        StartCoroutine(Play());
    }

    IEnumerator Play()
    {
        yield return new WaitForSeconds(1);
        yield return ResultUIComponent.Play();
    }
}
