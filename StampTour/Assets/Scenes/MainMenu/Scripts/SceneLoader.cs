using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    //이럴거면 싱글톤으로 사용하는게 더 나을듯 -> 이건 확정
    static string nextSceneName;

    [SerializeField]
    Slider progressBar;

    public static void LoadScene(string sceneName)
    {
        LoadScene(sceneName, LoadSceneMode.Additive);
    }
    public static void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Additive)
    {

        nextSceneName = sceneName;
        SceneManager.LoadSceneAsync("LoadingScene", mode);
    }

    IEnumerator LoadSceneProgress()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;

            progressBar.value = asyncLoad.progress;
            
        }
        Debug.Log("Loading complete");

        SceneManager.UnloadSceneAsync("LoadingScene");
    }

    private void Start()
    {
        if(progressBar != null)
        {
            StartCoroutine(LoadSceneProgress());
        }
    }

    public static void RollbackMainScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }
}
