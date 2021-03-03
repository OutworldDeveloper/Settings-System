using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SettingPresenter_Bool : SettingPresenter<Setting_Bool>
{

    [SerializeField] private Button switchButton;

    protected override void Present(Setting_Bool setting)
    {
        switchButton.GetComponentInChildren<Text>().text = setting.GetValue() ? "enabled" : "disabled";
    }

    private void OnEnable()
    {
        switchButton.onClick.AddListener(OnButtonPressed);
    }

    private void OnDisable()
    {
        switchButton.onClick.RemoveListener(OnButtonPressed);
    }

    public void OnButtonPressed()
    {
        targetSetting.SetValue(!targetSetting.GetValue());
        switchButton.GetComponentInChildren<Text>().text = targetSetting.GetValue() ? "enabled" : "disabled";
    }

}