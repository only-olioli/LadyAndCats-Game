namespace Settings
{
    public static class SettingMarshal
    {
        public static SettingWrapper<TSetting, TValue> GetSetting<TSetting, TValue>() where TSetting : Setting<TSetting, TValue>
        {
            return SettingsHelper.Get<TSetting, TValue>().Wrapper;
        }

        public static void Save()
        {
            SettingsHelper.Save();
        }
    }
}
