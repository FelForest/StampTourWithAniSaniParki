using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TitleTouch : MonoBehaviour
{
    private bool isTouchEnable = false;

    void OnTitleSceneLoaded()
    {
        isTouchEnable = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Screan touched / Check mouse button for PC
        if(isTouchEnable)
        {
            if(Input.touchCount > 0 || Input.GetMouseButton(0))
            {
                isTouchEnable = false;
                if (GameManager.gameManager.GetIsSceneFinished("Tutorial"))
                {
                    GameManager.LoadScene("MainScene");
                }
                else
                {
                    GameManager.LoadScene("Tutorial");
                }
                
            }
        }
    }
}
