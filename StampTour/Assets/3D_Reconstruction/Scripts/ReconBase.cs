using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ReconBase : MonoBehaviour
{
    [SerializeField]
    protected GameObject panel;

    protected bool isCompleted;
    public abstract void Enter();

    public abstract void Execute(ReconManager manager);

    public abstract void Exit();

    protected void SafePanelActive(bool active)
    {
        if (panel != null) panel.SetActive(active);
    }
}
