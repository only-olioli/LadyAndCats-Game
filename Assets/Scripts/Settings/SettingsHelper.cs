using UnityEngine;
using Utility;

namespace Settings
{
    internal static class SettingsHelper
    {
        private static readonly DataInitializer<GameSettings> _gameSettings;

        static SettingsHelper()
        {
            _gameSettings = new(() =>
            {
                return Resources.Load<GameSettings>(nameof(GameSettings));
            });
        }

        public static TSetting Get<TSetting, TValue>() where TSetting : Setting<TSetting, TValue>
        {
            return ((GameSettings)_gameSettings).Get<TSetting>();
        }

        public static void Save()
        {
            SettingsSave.Save();
        }
    }
}
