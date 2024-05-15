using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StampImageChange : MonoBehaviour
{
    [SerializeField]
    Sprite changedImage;
    Sprite orgineImage;

    private void Awake()
    {
        orgineImage = GetComponent<Image>().sprite;
    }
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            if (GameManager.gameManager.GetIsSceneFinished(gameObject.name))
            {
                GetComponent<Image>().sprite = changedImage;
            }
            else
            {
                GetComponent<Image>().sprite = orgineImage;
            }
        }

        catch
        {
            Debug.Log("not Flag");
        }
    }
}
