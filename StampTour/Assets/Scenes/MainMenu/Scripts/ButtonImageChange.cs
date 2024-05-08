using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonImageChange : MonoBehaviour
{
    [SerializeField]
    Sprite changedImage;
    Sprite orgineImage;
    // Start is called before the first frame update
    void Start()
    {
        orgineImage = GetComponent<Button>().image.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameManager.GetIsSceneFinished(gameObject.name))
        {
            GetComponent<Button>().image.sprite = changedImage;
        }
        else
        {
            GetComponent<Button>().image.sprite = orgineImage;
        }
    }
}
