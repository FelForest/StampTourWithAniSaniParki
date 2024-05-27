using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class AddSceneInBuildSetting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AddAllScenesInBuildSetting()
    {
#if UNITY_EDITOR
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        foreach (EditorBuildSettingsScene scene in scenes) 
        {
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scene.path);
        }
#endif
    }
}
