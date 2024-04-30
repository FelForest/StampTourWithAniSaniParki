using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColoringManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Color selectedColor;
    void Start()
    {
        selectedColor = Color.white;
    }
    public void SelectColor()
    {
        GameObject thisBtn = EventSystem.current.currentSelectedGameObject;
        selectedColor = thisBtn.GetComponent<Image>().color;
    }
    public void ChangeColor()
    {
        GameObject thisBtn = EventSystem.current.currentSelectedGameObject;
        thisBtn.GetComponent<Image>().color = selectedColor;
    }


}
