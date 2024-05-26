using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class PhotoCapture : MonoBehaviour
{
    public RenderTexture characterTexture; 
    public GameObject copyCharacter;
    public GameObject captureImagePos;
    public string characterName;
    public Camera camera;
    public Rect captureArea;
    public void CharacterTakeScreenShot()
    {
        //captureCanvas�� �ڽ� ������Ʈ�� ����
        if (captureImagePos.transform.childCount > 0) 
            Destroy(captureImagePos.transform.GetChild(0).gameObject);

        //����� �̹��� ����
        Instantiate(copyCharacter, captureImagePos.transform);

        RenderTexture.active = characterTexture;
        if (characterTexture != null)
        {
            //�̹��� �����
            Texture2D Image = new Texture2D(characterTexture.width, characterTexture.height);
            Image.ReadPixels(new Rect(0, 0, characterTexture.width, characterTexture.height), 0, 0);
            Image.Apply();

            byte[] pngBytes = Image.EncodeToPNG();
            SavePNG(pngBytes);
            RenderTexture.active = null;
            characterTexture.Release();
        }
    }
    public void TakeScreenShot()
    {
       
        int width = (int)captureArea.width;
        int height = (int)captureArea.height;

        // ������ ������ �´� RenderTexture ����
        RenderTexture renderTexture = new RenderTexture(width, height, 24);
        camera.targetTexture = renderTexture;
        camera.Render();

        // ���� RenderTexture�� Ȱ�� ���� ����
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTexture;

        // Texture2D�� RenderTexture ������ ����
        Texture2D screenImage = new Texture2D(width, height, TextureFormat.RGB24, false);
        // captureArea ���� ĸ��
        screenImage.ReadPixels(new Rect(0, 0, width, height), (int)captureArea.x, (int)captureArea.y);
        screenImage.Apply();

        // RenderTexture ��Ȱ��ȭ �� ī�޶� Ÿ�� �ʱ�ȭ
        camera.targetTexture = null;
        RenderTexture.active = previous;
        Destroy(renderTexture);

        // PNG�� ���ڵ� �� ����
        byte[] imageBytes = screenImage.EncodeToPNG();
        SavePNG(imageBytes);
    }
    void SavePNG(byte[] pngArray)
    {
        Debug.Log("Picture taken");
        string path = Path.Combine(Application.persistentDataPath, characterName + ".png");
        File.WriteAllBytes(path, pngArray);
        Debug.Log(path);
    }
    


}