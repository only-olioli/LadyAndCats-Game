#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.SceneManagement;
using Utility;

[InitializeOnLoad]
public static class DirectPlay
{
    static DirectPlay()
    {
        EditorApplication.playModeStateChanged += PlayModeStateChangedHandle;
    }

    private static void PlayModeStateChangedHandle(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode && SceneManager.GetActiveScene().name != "MainMenu")
        {
            GameObjectExtentions.CreateInstance<SettingsMediator>();
        }
    }
}
#endif
