using System;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    public sealed class Resolution : Setting<Resolution, UnityEngine.Resolution>
    {
        protected override UnityEngine.Resolution DefaultValue => Screen.resolutions[^1];

        protected override (bool hasHanged, UnityEngine.Resolution newValue) CheckValue(UnityEngine.Resolution value)
        {
            if (Value.Compare(default))
            {
                return (true, value);
            }

            int newIndex = value.Index();
            return (Value.Index() != newIndex, Screen.resolutions[newIndex]);
        }

        protected override Type SaveType => typeof(string);

        public override SettingsSave.NameValue SaveData => SettingsSave.NameValue.Get<string>(this, $"{Value.width}x{Value.height}");

        protected override UnityEngine.Resolution Load(SettingsSave.NameValue saveData)
        {
            if (saveData is null || !ResolutionExtentions.TryParse(((SettingsSave.NameValue<string>)saveData).Value, out UnityEngine.Resolution resolution))
            {
                return DefaultValue;
            }
            return resolution;
        }

        public static List<string> Resolutions
        {
            get
            {
                List<string> resolutions = new();
                foreach (UnityEngine.Resolution resolution in Screen.resolutions)
                {
                    resolutions.Add($"{resolution.width}x{resolution.height}x{resolution.refreshRate}Hz");
                }
                return new(resolutions);
            }
        }
    }
}
