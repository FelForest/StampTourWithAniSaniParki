using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PuzzleController : MonoBehaviour
{
    public GameObject Puzzle;

    private void Start() {
        Puzzle.GetComponent<BoardGen>().Generate(this);

    }

    public void Pass()
    {
        Debug.Log("Pass");
    }
}
