using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JicsawPuzzle;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ColoringScenario : MonoBehaviour
{
    public Image CharacterSelectUI;
    public GameObject coloring;
    public GameObject[] character = new GameObject[3];
    public GuideUIController talk_Bubble;
    public Canvas CaptureCanvas;

    public ResultUIController ResultUIController;

    bool[] isCheck = {false, false, false, false};

    private void Start() 
    {
        talk_Bubble.SwingAnimation(true);
        talk_Bubble.Talk("ªˆƒ•≥Ó¿Ã∏¶ «œ∞Ì ΩÕ¿∫ ƒ≥∏Ø≈Õ∏¶ ∞Ò∂Û¡‡!");    
    }

    public void OnPalette()
    {
        PhotoCapture photoCapture = CaptureCanvas.GetComponent<PhotoCapture>();
        GameObject click = EventSystem.current.currentSelectedGameObject;
        Debug.Log(click);
        CharacterSelectUI.gameObject.SetActive(false);
        isCheck[0] = true;
        if (click.name == "Ani")
        {
            click.GetComponent<Image>().color = Color.white;
            isCheck[1] = true;
            character[0].SetActive(true);
            photoCapture.copyCharacter = character[0];
        }
        else if (click.name == "Sani")
        {
            click.GetComponent<Image>().color = Color.white;
            isCheck[2] = true;
            character[1].SetActive(true);
            photoCapture.copyCharacter = character[1];
        }
        else if (click.name == "Bani")
        {
            click.GetComponent<Image>().color = Color.white;
            isCheck[3] = true;
            character[2].SetActive(true);
            photoCapture.copyCharacter = character[2];
        }
        coloring.SetActive(true);
        talk_Bubble.SwingAnimation(false);
        talk_Bubble.Talk("ø¯«œ¥¬ ªˆªÛ¿∏∑Œ\nƒ•«ÿ∫∏¿⁄!");

        photoCapture.characterName = click.name;

    }
    public void ReturnButton()
    {
        coloring.SetActive(false);
        character[0].SetActive(false);
        character[1].SetActive(false);
        character[2].SetActive(false);
        talk_Bubble.SwingAnimation(true);
        talk_Bubble.Talk("ªˆƒ•≥Ó¿Ã∏¶ «œ∞Ì ΩÕ¿∫ ƒ≥∏Ø≈Õ∏¶ ∞Ò∂Û¡‡!");
        CharacterSelectUI.gameObject.SetActive(true);
    }

    public void GameClear()
    {
        Debug.Log(isCheck.All(t => t));
        if(isCheck.All(t=>t))
        {
            Debug.Log("GameClear!");
            //  SceneManager.LoadScene("Photography");
            StartCoroutine(ResultUIController.Play());
        }
    }
}
