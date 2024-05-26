using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag
{
    private bool _isSceneLoaded = false;
    private bool _isSceneFinished = false;

    public bool isSceneLoaded
    {
        get { return _isSceneLoaded; }
        set { _isSceneLoaded = value; }
    }

    public bool isSceneFinished
    {
        get { return _isSceneFinished; }
        set { _isSceneFinished = value; }
    }

    public void ResetFlag()
    {
        isSceneLoaded = false;
        isSceneFinished = false;   
    }


}
