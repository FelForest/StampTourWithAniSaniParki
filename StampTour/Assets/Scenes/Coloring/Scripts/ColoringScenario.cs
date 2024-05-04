using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

public class ColoringScenario : MonoBehaviour
{
    public Image CharacterSelectUI;
    public GameObject coloring;
    public GameObject[] character = new GameObject[3];
    public TextMeshProUGUI talk_Bubble;

    public void OnPalette()
    {
        string click = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log(click);
        CharacterSelectUI.gameObject.SetActive(false);
        if (click == "Ani")
            character[0].SetActive(true);
        else if (click == "Sani")
            character[1].SetActive(true);
        else if (click == "Bani")
            character[2].SetActive(true);
        coloring.SetActive(true);
        talk_Bubble.text = "원하는 색상으로\n칠해보자!";
    }
    public void ReturnButton()
    {
        coloring.SetActive(false);
        character[0].SetActive(false);
        character[1].SetActive(false);
        character[2].SetActive(false);
        talk_Bubble.text = "색칠놀이를 하고 싶은 캐릭터를 골라줘!";
        CharacterSelectUI.gameObject.SetActive(true);



    }
}
