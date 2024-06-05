using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(Scene))]
public class SceneDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty sceneAssetProp = property.FindPropertyRelative("sceneAsset");
        SerializedProperty sceneNameProp = property.FindPropertyRelative("sceneName");
        SerializedProperty scenePathProp = property.FindPropertyRelative("scenePath");

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        if (sceneAssetProp != null)
        {
            EditorGUI.BeginChangeCheck();

            Object value = EditorGUI.ObjectField(position, sceneAssetProp.objectReferenceValue, typeof(SceneAsset), false);

            if (EditorGUI.EndChangeCheck())
            {
                sceneAssetProp.objectReferenceValue = value;
                if (value != null)
                {
                    sceneNameProp.stringValue = (value as SceneAsset).name;
                    scenePathProp.stringValue = AssetDatabase.GetAssetPath(value as SceneAsset);
                }
                else
                {
                    sceneNameProp.stringValue = string.Empty;
                    scenePathProp.stringValue = string.Empty;
                }
            }
        }

        EditorGUI.EndProperty();
    }
}
#endif