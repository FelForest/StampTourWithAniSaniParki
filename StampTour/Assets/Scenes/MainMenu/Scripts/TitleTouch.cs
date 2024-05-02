using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TitleTouch : MonoBehaviour
{
    private bool isTouchEnable = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

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
                SceneLoader.LoadScene("MainScene",UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
        }
    }
}
