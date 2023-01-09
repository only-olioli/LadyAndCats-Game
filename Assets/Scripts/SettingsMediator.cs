using Settings;
using UnityEngine;
using Resolution = Settings.Resolution;

public sealed class SettingsMediator : MonoBehaviour
{
    private static bool _initialized;

    private void Awake()
    {
        if (_initialized)
        {
            return;
        }
        _initialized = true;
        DontDestroyOnLoad(gameObject);
        ChangeFullScreen(FullScreen.Value);
        ChangeResolution(Resolution.Value);
    }

    private void OnEnable()
    {
        FullScreen.OnValueChanged += ChangeFullScreen;
        Resolution.OnValueChanged += ChangeResolution;
    }

    private void ChangeFullScreen(bool context)
    {
        Screen.fullScreen = context;
    }

    private void ChangeResolution(UnityEngine.Resolution resolution)
    {
        Screen.SetResolution(resolution.width, resolution.height, FullScreen.Value);
    }

    private void OnDisable()
    {
        FullScreen.OnValueChanged -= ChangeFullScreen;
        Resolution.OnValueChanged -= ChangeResolution;
    }
}
