using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Setting<T> : BaseSetting, ISettingsResetable
{

    [SerializeField] private T defaultValue;

    private T cashedValue;
    private bool isCashed;

    protected virtual T getDefaultValue => defaultValue;

    public void SetValue(T value)
    {
        isCashed = false;
        SaveValue(value);
        OnValueChanged();
        SettingsChanged();
    }

    public T GetValue()
    {
        if (isCashed)
            return cashedValue;

        if (PlayerPrefs.HasKey(name))
        {
            cashedValue = LoadValue();
            isCashed = true;
            return cashedValue;
        }

        return getDefaultValue;
    }

    public void ResetSetting()
    {
        SetValue(getDefaultValue);
    }

    protected abstract T LoadValue();

    protected abstract void SaveValue(T value);

    protected virtual void OnValueChanged() { }

}