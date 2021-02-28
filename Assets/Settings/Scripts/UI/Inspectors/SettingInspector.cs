using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class SettingInspector<T> : BaseSettingInspector where T : BaseSetting
{

    [SerializeField] private Text variableNameText;

    public override Type TargetType => typeof(T);

    protected T settingsVariable;

    public override void Setup(BaseSetting settingsVariable)
    {
        this.settingsVariable = settingsVariable as T;
        variableNameText.text = GetDisplayName();
        Present(settingsVariable as T);
    }

    protected abstract void Present(T settingsVariable);

    protected virtual string GetDisplayName()
    {
        return settingsVariable.DisplayName;
    }

    protected void UpdateName()
    {
        variableNameText.text = GetDisplayName();
    }

    private void Start()
    {
        Settings.OnSettingsChanged += Settings_OnSettingsChanged;
    }

    private void OnDestroy()
    {
        Settings.OnSettingsChanged -= Settings_OnSettingsChanged;
    }

    private void Settings_OnSettingsChanged()
    {
        Present(settingsVariable);
    }

}