using System;

namespace Settings
{
    public interface ISetting
    {
        public string Name { get; }

        SettingsSave.NameValue SaveData { get; }

        void Load(SettingsSave.NameValue saveData);

        Type SaveType { get; }
    }
}
