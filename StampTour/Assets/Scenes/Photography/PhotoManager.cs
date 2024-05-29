using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class PhotoManager : MonoBehaviour
{
    public RenderTexture characterTexture;
    public string characterName;
    public Image Image;
    Texture2D texture;
    private void Start()
    {
       // LoadPNG();
    }
    public void LoadPNG()
    {
        characterName = this.gameObject.name;
        string path = Path.Combine(Application.persistentDataPath, characterName + ".png");
        byte[] fileBytes = File.ReadAllBytes(path);
        texture = new Texture2D(characterTexture.width, characterTexture.height);
        texture.LoadImage(fileBytes);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        //Image.sprite = sprite;
        this.GetComponent<Image>().sprite = sprite; 
        SaveToGallery();
    }
    public void SaveToGallery()
    {
        // do something with texture
        NativeGallery.Permission permission = NativeGallery.CheckPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image);
        if (permission == NativeGallery.Permission.Denied)
        {
            if (NativeGallery.CanOpenSettings())
            {
                NativeGallery.OpenSettings();
            }
        }
        NativeGallery.SaveImageToGallery(texture, "default", "아니사니바기와 함께 사진찍기");
    }
    
}
