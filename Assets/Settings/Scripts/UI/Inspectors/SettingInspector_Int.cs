using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingInspector_Int : SettingInspector<Setting_Int>
{

    [SerializeField] private Slider slider;

    protected override void Present(Setting_Int settingsVariable)
    {
        slider.minValue = settingsVariable.MinValue;
        slider.maxValue = settingsVariable.MaxValue;
        slider.value = settingsVariable.GetValue();
    }

    private void OnEnable()
    {
        slider.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnDisable()
    {
        slider.onValueChanged.RemoveListener(OnValueChanged);
    }

    public void OnValueChanged(float value)
    {
        settingsVariable.SetValue((int)value);
        UpdateName();
    }

    protected override string GetDisplayName()
    {
        return base.GetDisplayName() + " " + settingsVariable.GetValue();
    }

}