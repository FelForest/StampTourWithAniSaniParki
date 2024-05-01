using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    private bool _isLoaded = false;
    private bool _isFinish = false;

    public bool isLoaded
    {
        get { return _isLoaded; }
        set { _isLoaded = value; }
    }

    public bool isFinish
    {
        get { return _isFinish; }
        set { _isFinish = value; }
    }
}
