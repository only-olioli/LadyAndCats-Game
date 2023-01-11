using Settings;
using System;
using UnityEngine;
using Utility;

public abstract class Setting<TSelf, TValue> : ScriptableObject, ISetting, IValueController<TValue> where TSelf : Setting<TSelf, TValue>
{
    private static readonly DataInitializer<TSelf> _setting;

    protected abstract TValue DefaultValue { get; }

    static Setting()
    {
        _setting = new(() =>
        {
            return SettingsHelper.Get<TSelf, TValue>();
        }, setting => SettingsSave.Load(setting));
    }

    private SettingWrapper<TSelf, TValue> _wrapper;

    internal SettingWrapper<TSelf, TValue> Wrapper
    {
        get
        {
            _wrapper ??= new((TSelf)_setting);
            return _wrapper;
        }
    }

    private TValue _value;
    private Action<TValue> _onValueChanged;

    public static TValue Value
    {
        get => ((TSelf)_setting)._value;
        set
        {
            TSelf setting = _setting;
            var (hasChanged, newValue) = setting.CheckValue(value);
            if (hasChanged)
            {
                setting._value = newValue;
                setting._onValueChanged?.Invoke(newValue);
            }
        }
    }

    private void SetValueWithoutNotify(TValue value)
    {
        TSelf setting = _setting;
        var (hasChanged, newValue) = setting.CheckValue(value);
        if (hasChanged)
        {
            setting._value = newValue;
        }
    }

    public static event Action<TValue> OnValueChanged
    {
        add => ((TSelf)_setting)._onValueChanged += value;
        remove => ((TSelf)_setting)._onValueChanged -= value;
    }

    event Action<TValue> IValueController<TValue>.OnValueChanged
    {
        add => OnValueChanged += value;
        remove => OnValueChanged -= value;
    }

    protected virtual (bool hasHanged, TValue newValue) CheckValue(TValue value) => (!value.Equals(Value), value);

    TValue IValueHolder<TValue>.Value
    {
        get => Value;
        set => Value = value;
    }

    public virtual SettingsSave.NameValue SaveData => SettingsSave.NameValue.Get(this, Value);

    string ISetting.Name { get; } = typeof(TSelf).Name;

    Type ISetting.SaveType => SaveType;

    protected abstract Type SaveType { get; }

    protected virtual TValue Load(SettingsSave.NameValue saveData)
    {
        if (saveData is null)
        {
            return DefaultValue;
        }
        return ((SettingsSave.NameValue<TValue>)saveData).Value;
    }

    void ISetting.Load(SettingsSave.NameValue saveData) => SetValueWithoutNotify(Load(saveData));
}
