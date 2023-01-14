using Settings;
using UnityEngine;
using UnityEngine.UIElements;
using Resolution = Settings.Resolution;

namespace UI
{
    [RequireComponent(typeof(UIDocument))]
    public sealed class SettingsUI : MonoBehaviour
    {
        private Toggle _fullScreen;
        private DropdownField _resolution;
        private Button _back;

        private void Awake()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            _fullScreen = root.Q<Toggle>(nameof(FullScreen));
            _fullScreen.value = FullScreen.Value;

            _resolution = root.Q<DropdownField>(nameof(Resolution));
            _resolution.choices = Resolution.Resolutions;
            _resolution.index = Resolution.Value.Index();

            _back = root.Q<Button>(nameof(Back));
        }

        private void OnEnable()
        {
            _fullScreen.RegisterValueChangedCallback(ChangeFullScreen);
            _resolution.RegisterValueChangedCallback(ChangeResolution);
            _back.clicked += Back;
        }

        private void ChangeFullScreen(ChangeEvent<bool> context)
        {
            FullScreen.Value = context.newValue;
        }

        private void ChangeResolution(ChangeEvent<string> _)
        {
            Resolution.Value = Screen.resolutions[_resolution.index];
        }

        private void Back()
        {
            SettingMarshal.Save();
        }

        private void OnDisable()
        {
            _fullScreen.UnregisterValueChangedCallback(ChangeFullScreen);
            _resolution.UnregisterValueChangedCallback(ChangeResolution);
            _back.clicked -= Back;
        }
    }
}
