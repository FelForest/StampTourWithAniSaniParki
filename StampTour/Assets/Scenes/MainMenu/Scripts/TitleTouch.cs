using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TitleTouch : MonoBehaviour
{
    private bool isTouchEnable = false;

    void OnEnable()
    {
        isTouchEnable = true;
    }

    // Update is called once per frame
    void Update()
    {
        // 화면터치시/ PC에서는 확인용을 마우스 클릭
        if(isTouchEnable)
        {
            if(Input.touchCount > 0 || Input.GetMouseButton(0))
            {
                isTouchEnable = false;
                GameManager.LoadScene("MainScene");
            }
        }
    }
}
