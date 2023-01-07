using UnityEngine;

namespace Settings
{
    public sealed class GameSettings : ScriptableObject
    {
        [SerializeField] private string savePath;
        [SerializeField] private ScriptableObject[] settings;

        public string SavePath => Application.dataPath + savePath + "/settings.xml";

        public TSetting Get<TSetting>() where TSetting : ScriptableObject, ISetting
        {
            foreach (var setting in settings)
            {
                if (setting is TSetting matched)
                {
                    return matched;
                }
            }
            throw new System.Exception($"{typeof(TSetting).Name} is not in {nameof(GameSettings)}");
        }

        public ISetting this[System.Index i] => (ISetting)settings[i];

        public int Length => settings.Length;
    }
}
