using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColoringDB : MonoBehaviour
{
    #region Singleton
    protected static ColoringDB _instance;
    public static ColoringDB Instance
    {
        get 
        { 
            if (_instance == null)
            {
                _instance = FindObjectOfType<ColoringDB>();
                
                if (_instance == null)
                {
                    GameObject obj = new GameObject("ColoringDB");
                    _instance = obj.AddComponent<ColoringDB>();

                    DontDestroyOnLoad(obj);
                }
            } 

            return _instance;
        }
    }

    private void SingletonAwake() 
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);

            return;
        }
    }
#endregion

    private void Awake() 
    {
        SingletonAwake();
    }

    public Dictionary<string, Color> MaterialColorDic = new Dictionary<string, Color>();
}
