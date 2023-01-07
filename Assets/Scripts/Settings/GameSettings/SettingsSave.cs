using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using Utility.IO;

namespace Settings
{
    public static class SettingsSave
    {
        private static readonly Type[] _avilibleTypes = {
            typeof(bool),
            typeof(int),
            typeof(float),
            typeof(string)
        };

        public abstract class NameValue
        {
            public string Name { get; set; }

            public static NameValue<TSaveValue> Get<TSaveValue>(ISetting setting, TSaveValue value)
            {
                if (((IList<Type>)_avilibleTypes).Contains(typeof(TSaveValue)))
                {
                    return new NameValue<TSaveValue>()
                    {
                        Name = setting.Name,
                        Value = value
                    };
                }
                throw new NotSupportedException($"{nameof(SettingsSave)} doesn't suppoty {typeof(TSaveValue).Name} type!");
            }
        }

        public sealed class NameValue<T> : NameValue
        {
            public T Value { get; set; }
        }

        public sealed class SaveData
        {
            public List<NameValue<bool>> BooleanSettings { get; set; }
            public List<NameValue<int>> IntegerSettings { get; set; }
            public List<NameValue<float>> FloatSettings { get; set; }
            public List<NameValue<string>> StringSettings { get; set; }

            public SaveData()
            {
                BooleanSettings = new();
                IntegerSettings = new();
                FloatSettings = new();
                StringSettings = new();
            }
        }

        private static readonly DataIntializer<GameSettings> _settings;
        private static SaveData _data;

        static SettingsSave()
        {
            _settings = new(() =>
            {
                return Resources.Load<GameSettings>(nameof(GameSettings));
            });
        }

        public static void Save()
        {
            GameSettings settings = _settings;
            SaveData data = new();
            for (int i = 0; i < settings.Length; i++)
            {
                NameValue settingData = settings[i].SaveData;
                AddToData(data, settingData);
            }
            XMLSerialization.Write(data, settings.SavePath);
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
            _data = data;
        }

        private static void AddToData(SaveData data, NameValue settingData)
        {
            switch (settingData)
            {
                case NameValue<bool> boolData:
                    data.BooleanSettings.Add(boolData);
                    break;
                case NameValue<int> intData:
                    data.IntegerSettings.Add(intData);
                    break;
                case NameValue<float> floatData:
                    data.FloatSettings.Add(floatData);
                    break;
                case NameValue<string> stringData:
                    data.StringSettings.Add(stringData);
                    break;
            }
        }

        public static void Load(ISetting setting)
        {
            _data ??= XMLSerialization.Read<SaveData>(((GameSettings)_settings).SavePath);
            NameValue value = GetSettingData(setting);
            setting.Load(value);
        }

        private static NameValue GetSettingData(ISetting setting)
        {
            if (_data is null)
            {
                return null;
            }

            if (setting.SaveType == typeof(bool))
            {
                return _data.BooleanSettings.Find(nv => nv.Name == setting.Name);
            }

            if (setting.SaveType == typeof(int))
            {
                return _data.IntegerSettings.Find(nv => nv.Name == setting.Name);
            }

            if (setting.SaveType == typeof(float))
            {
                return _data.FloatSettings.Find(nv => nv.Name == setting.Name);
            }

            if (setting.SaveType == typeof(string))
            {
                return _data.StringSettings.Find(nv => nv.Name == setting.Name);
            }
            return null;
        }
    }
}
