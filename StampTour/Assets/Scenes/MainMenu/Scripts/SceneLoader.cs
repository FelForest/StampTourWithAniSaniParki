using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    
    public static string nextSceneName;

    [SerializeField]
    Slider progressBar;

    

    IEnumerator LoadSceneProgress()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Single);

        while (!asyncLoad.isDone)
        {
            yield return null;

            progressBar.value = asyncLoad.progress;
            
        }
        Debug.Log("Loading complete");
    }

    private void Start()
    {
        if(progressBar != null)
        {
            StartCoroutine(LoadSceneProgress());
        }
    }
}
