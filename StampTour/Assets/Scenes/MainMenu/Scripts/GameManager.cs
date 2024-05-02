using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private Dictionary<string, Flag> flags = new Dictionary<string, Flag>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);


        AddFlag("TestScene1");
        AddFlag("TestScene2");
        AddFlag("TestScene3");
        AddFlag("TestScene4");
    }

    private void AddFlag(string key)
    {
        if (!flags.ContainsKey(key))
        {
            Flag flag = new Flag();
            flags.Add(key, flag);
        }
    }

    public void SetIsLoaded(string key, bool isLoadding)
    {
        if (flags.ContainsKey(key))
        {
            Flag flag = flags[key];
            flag.isLoaded = isLoadding;
        }
    }

    public void SetIsFinish(string key, bool isFinish)
    {
        if (flags.ContainsKey(key))
        {
            flags[key].isFinish = isFinish;
        }
    }

    public bool GetIsLoaded(string key)
    {
        return flags[key].isLoaded;
    }

    public bool GetIsFinish(string key)
    {
        return flags[key].isFinish;
    }


}
