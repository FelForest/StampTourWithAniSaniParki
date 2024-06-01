using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class Scene
{
    [SerializeField] private Object sceneAsset;
    [SerializeField] private string sceneName = string.Empty;
    [SerializeField] private string scenePath = string.Empty;
    public string ScenePath
    {
        get { return scenePath; }
    }


    public string SceneName
    {
        get { return sceneName; }
    }

    public static implicit operator string(Scene sceneField)
    {
        return sceneField.SceneName;
    }

#if UNITY_EDITOR
    public Object SceneAsset
    {
        get { return sceneAsset; }
        set
        {
            sceneAsset = value;
            sceneName = sceneAsset != null ? sceneAsset.name : string.Empty;
            scenePath = sceneAsset != null ? AssetDatabase.GetAssetPath(sceneAsset) : string.Empty;
        }
    }
#endif
}