using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLoadScene : MonoBehaviour
{
    public string SceneName;
    [ContextMenu("LoadScene")]
    public void LoadScene()
    {
        GameManager.LoadScene(SceneName);
    }
}
