using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TestFlagSet : MonoBehaviour
{
    Toggle toggle;
    // Start is called before the first frame update
    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToogleValueChanged);
    }

    void OnToogleValueChanged(bool isOn)
    {
        if (isOn)
        {
            GameManager.instance.SetIsFinish(gameObject.name,true);
        }
        else
        {
            GameManager.instance.SetIsFinish(gameObject.name, false);
        }
    }




}
