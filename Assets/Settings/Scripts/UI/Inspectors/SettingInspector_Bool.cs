using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SettingInspector_Bool : SettingInspector<Setting_Bool>
{

    [SerializeField] private Button switchButton;

    protected override void Present(Setting_Bool settingsVariable)
    {
        switchButton.GetComponentInChildren<Text>().text = settingsVariable.GetValue() ? "enabled" : "disabled";
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
        settingsVariable.SetValue(!settingsVariable.GetValue());
        switchButton.GetComponentInChildren<Text>().text = settingsVariable.GetValue() ? "enabled" : "disabled";
    }

}