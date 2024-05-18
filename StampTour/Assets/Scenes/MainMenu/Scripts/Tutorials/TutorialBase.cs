using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialBase : MonoBehaviour
{
    [SerializeField]
    protected GameObject panel;

    protected bool isCompleted;
    public abstract void Enter();

    public abstract void Execute(TutorialsManager manager);

    public abstract void Exit();
}
