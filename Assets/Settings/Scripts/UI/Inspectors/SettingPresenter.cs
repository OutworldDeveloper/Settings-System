using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class SettingPresenter<T> : BaseSettingPresenter where T : BaseSetting
{

    [SerializeField] private Text variableNameText;

    public override Type TargetType => typeof(T);

    protected T targetSetting;

    public override void Setup(BaseSetting setting)
    {
        targetSetting = setting as T;
        variableNameText.text = GetDisplayName();
        Present(setting as T);
    }

    protected abstract void Present(T settingsVariable);

    protected virtual string GetDisplayName()
    {
        return targetSetting.DisplayName;
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
        Present(targetSetting);
    }

}