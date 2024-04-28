using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JicsawPuzzleController : MonoBehaviour
{
    public GameObject Board;

    private void Start() {
        Board.GetComponent<BoardGen>().Generate(this);

    }

    public void Pass()
    {
        Debug.Log("Pass");
    }
}
