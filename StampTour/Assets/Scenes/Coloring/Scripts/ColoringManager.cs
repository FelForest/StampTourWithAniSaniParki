using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColoringManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Color selectedColor;
    public Image currentColor;
    void Start()
    {
        selectedColor = Color.white;
        currentColor.color = Color.white;
    }
    public void SelectColor()
    {
        GameObject thisBtn = EventSystem.current.currentSelectedGameObject;
        selectedColor = thisBtn.GetComponent<Image>().color;
        currentColor.color = selectedColor;

    }
    public void ChangeColor()
    {
        GameObject thisBtn = EventSystem.current.currentSelectedGameObject;
        thisBtn.GetComponent<Image>().color = selectedColor;
    }


}
