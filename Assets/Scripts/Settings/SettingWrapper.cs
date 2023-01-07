using System;
using Utility;

namespace Settings
{
    public sealed class SettingWrapper<TSetting, TValue> : IValueController<TValue> where TSetting : Setting<TSetting, TValue>
    {
        private readonly TSetting _setting;

        internal SettingWrapper(TSetting setting)
        {
            _setting = setting;
        }

        public TValue Value
        {
            get => ((IValueController<TValue>)_setting).Value;
            set => ((IValueController<TValue>)_setting).Value = value;
        }

        public event Action<TValue> OnValueChanged
        {
            add => ((IValueController<TValue>)_setting).OnValueChanged += value;
            remove => ((IValueController<TValue>)_setting).OnValueChanged -= value;
        }
    }
}
