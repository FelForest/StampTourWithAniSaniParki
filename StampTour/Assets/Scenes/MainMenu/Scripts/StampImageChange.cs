using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class StampImageChange : MonoBehaviour
{
    [SerializeField]
    Sprite changedImage;

    [SerializeField]
    string[] flagNames;
    Sprite orgineImage;

    private void Awake()
    {
        orgineImage = GetComponent<Image>().sprite;
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach(string flagName in flagNames)
        {
            try
            {
                if (GameManager.Instance.GetIsSceneFinished(flagName))
                {
                    GetComponent<Image>().sprite = changedImage;
                    break;
                }
                else
                {
                    GetComponent<Image>().sprite = orgineImage;
                }
            }

            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }
        
    }
}
