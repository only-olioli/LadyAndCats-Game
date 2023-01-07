using UnityEngine;

namespace Settings
{
    public sealed class SettingsMediator : MonoBehaviour
    {
        private bool _initialized;

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

        private void OnDestroy()
        {
            FullScreen.OnValueChanged -= ChangeFullScreen;
        }
    }
}
