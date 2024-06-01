using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    _instance = new GameObject("GameManager").AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    private Dictionary<string, Flag> flags = new Dictionary<string, Flag>();
    public int SceneCount
    {
        get { return SceneManager.sceneCountInBuildSettings; }
    }

    private bool _selectedTab;
    public bool SelectedTab
    {
        get { return _selectedTab; }
        set { _selectedTab = value; }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        SetFlags();
        Debug.Log($"Total scenes in build settings: {SceneCount}");
    }

    private void SetFlags()
    {
        string path;

        for (int i = 0; i < SceneCount; i++)
        {
            path = SceneUtility.GetScenePathByBuildIndex(i);
            AddScene(System.IO.Path.GetFileNameWithoutExtension(path));
        }
        AddScene("TV");
    }

    private void AddScene(string key)
    {
        if (!flags.ContainsKey(key))
        {
            flags.Add(key, new Flag());
        }
    }

    private bool IsContainKey(string key)
    {
        if (flags.ContainsKey(key))
        {
            return true;
        }
        else
        {
            Debug.LogWarning($"Key '{key}' not found in flags");
            return false;
        }
    }

    public void SetIsSceneLoaded(string key, bool isLoaded)
    {
        if (IsContainKey(key))
        {
            flags[key].isSceneLoaded = isLoaded;
        }
    }

    public void SetIsSceneFinished(string key, bool isFinished)
    {
        if (IsContainKey(key))
        {
            flags[key].isSceneFinished = isFinished;
        }
    }

    public bool GetIsSceneLoaded(string key)
    {
        if (IsContainKey(key))
        {
            return flags[key].isSceneLoaded;
        }
        return false;
    }

    public bool GetIsSceneFinished(string key)
    {
        if (IsContainKey(key))
        {
            return flags[key].isSceneFinished;
        }
        return false;
    }

    public static void LoadScene(Scene scene)
    {
        LoadScene(scene.SceneName, LoadSceneMode.Single);
    }
    public static void LoadScene(string sceneName)
    {
        LoadScene(sceneName, LoadSceneMode.Single);
    }

    public static void LoadScene(int sceneNum)
    {
        string path = SceneUtility.GetScenePathByBuildIndex(sceneNum);
        LoadScene(System.IO.Path.GetFileNameWithoutExtension(path));
    }

    public static void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
    {
        if(!Instance.IsContainKey(sceneName)) return;

        SceneLoader.nextSceneName = sceneName;
        Instance.SetIsSceneLoaded(sceneName, true);

        Debug.Log($"{sceneName} is Loaded: {Instance.GetIsSceneLoaded(sceneName)}");
        
        SceneManager.LoadSceneAsync("LoadingScene", mode);
    }

    public static void RollbackMainScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        Instance.SetIsSceneLoaded(currentSceneName, false);
        
        LoadScene("MainScene");
    }
}