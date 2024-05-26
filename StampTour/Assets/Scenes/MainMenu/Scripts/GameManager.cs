using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation.VisualScripting;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEditor;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    private Dictionary<string, Flag> flags = new Dictionary<string, Flag>();

    
    private bool _selectedTab;
    public bool SelectedTab
    {
        get { return _selectedTab; }
        set { _selectedTab = value;}
    }
    public enum Scene
    {
        MainScene,
        JicsawPuzzle
    }
    private void Awake()
    {
        GameManagerAwake();

        AddScene("MainScene");
        AddScene("Tutorial");
        AddScene("TestScene1");
    }

    private void GameManagerAwake()
    {
        if (gameManager == null)
        {
            gameManager = this;
        }
        else if (gameManager != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
    private void AddScene(string key)
    {
        if (!flags.ContainsKey(key))
        {
            flags.Add(key, new Flag());
        }
    }

    

    public void SetIsSceneLoaded(string key, bool isLoaded)
    {
        if (flags.ContainsKey(key))
        {
            flags[key].isSceneLoaded = isLoaded;
        }
    }

    public void SetIsSceneFinished(string key, bool isFinished)
    {
        if (flags.ContainsKey(key))
        {
            flags[key].isSceneFinished = isFinished;
        }
    }

    public bool GetIsSceneLoaded(string key)
    {
        return flags[key].isSceneLoaded;
    }

    public bool GetIsSceneFinished(string key)
    {
        return flags[key].isSceneFinished;
    }

    public void LoadScene(string sceneName)
    {
        LoadScene(sceneName,LoadSceneMode.Single);
    }

    public static void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
    {
        SceneLoader.nextSceneName = sceneName;
        gameManager.SetIsSceneLoaded(sceneName, true);
        Debug.Log($"{sceneName} is Loaded : {gameManager.GetIsSceneLoaded(sceneName)}");
        SceneManager.LoadSceneAsync("LoadingScene", mode);
    }

    public static void RollbackMainScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        gameManager.SetIsSceneLoaded(currentSceneName, false);
        LoadScene("MainScene");
    }
}
