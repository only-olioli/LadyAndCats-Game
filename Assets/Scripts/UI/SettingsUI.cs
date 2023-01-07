using Settings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Resolution = Settings.Resolution;

namespace UI
{
    public sealed class SettingsUI : MonoBehaviour
    {
        [SerializeField] private Toggle fullScreen;
        [SerializeField] private TMP_Dropdown resolution;

        private void Awake()
        {
            resolution.ClearOptions();
            resolution.AddOptions(Resolution.Resolutions);
            fullScreen.isOn = FullScreen.Value;
            resolution.value = Resolution.Value.Index();
        }

        private void OnEnable()
        {
            fullScreen.onValueChanged.AddListener(b => ChangeFullScreen(b));
            resolution.onValueChanged.AddListener(i => ChangeResolution(i));
        }

        private void ChangeFullScreen(bool context)
        {
            FullScreen.Value = context;
        }

        private void ChangeResolution(int index)
        {
            Resolution.Value = Screen.resolutions[index];
        }

        private void OnDisable()
        {
            fullScreen.onValueChanged.RemoveAllListeners();
            resolution.onValueChanged.RemoveAllListeners();
        }
    }
}
