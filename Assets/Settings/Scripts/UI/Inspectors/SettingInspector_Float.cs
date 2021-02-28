using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingInspector_Float : SettingInspector<Setting_Float>
{

    [SerializeField] private Slider slider;

    protected override void Present(Setting_Float settingsVariable)
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
        settingsVariable.SetValue(value);
        UpdateName();
    }

    protected override string GetDisplayName()
    {
        return base.GetDisplayName() + " " + 
            (settingsVariable.GetValue() / settingsVariable.MaxValue * 100f).ToString("0") + "%";
    }

}
