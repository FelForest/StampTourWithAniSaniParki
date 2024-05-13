using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonImageChange : MonoBehaviour
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
        if (GameManager.gameManager.GetIsSceneFinished(gameObject.name))
        {
            GetComponent<Image>().sprite = changedImage;
        }
        else
        {
            GetComponent<Image>().sprite = orgineImage;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
