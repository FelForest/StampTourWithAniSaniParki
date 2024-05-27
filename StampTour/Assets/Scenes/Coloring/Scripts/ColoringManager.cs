using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColoringManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Color selectedColor;
    public Image currentColor;
    public AudioClip coloringAudio;
    public AudioClip pickAudio;
    public GameObject test1;
    public GameObject test2;
    AudioSource audioSource;

   
    void Start()
    {
        selectedColor = Color.white;
        currentColor.color = Color.white;
        audioSource = GetComponent<AudioSource>();

    }
    public void SelectColor()
    {
        GameObject thisBtn = EventSystem.current.currentSelectedGameObject;
        selectedColor = thisBtn.GetComponent<Image>().color;
        currentColor.color = selectedColor;
        audioSource.clip = pickAudio;
        audioSource.Play();

    }
    public void ChangeColor()
    {
        GameObject thisBtn = EventSystem.current.currentSelectedGameObject;
        if(thisBtn.name == "test1") test1.GetComponent<Material>().color = selectedColor;
        if(thisBtn.name=="test2") test2.GetComponent<Material>().color = selectedColor;
        thisBtn.GetComponent<Image>().color = selectedColor;
        audioSource.clip = coloringAudio;
        audioSource.Play();
    }
    

}
