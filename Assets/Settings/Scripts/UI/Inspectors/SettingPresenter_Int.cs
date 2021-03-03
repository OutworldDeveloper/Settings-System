using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPresenter_Int : SettingPresenter<Setting_Int>
{

    [SerializeField] private Slider slider;

    protected override void Present(Setting_Int setting)
    {
        slider.minValue = setting.MinValue;
        slider.maxValue = setting.MaxValue;
        slider.value = setting.GetValue();
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
        targetSetting.SetValue((int)value);
        UpdateName();
    }

    protected override string GetDisplayName()
    {
        return base.GetDisplayName() + " " + targetSetting.GetValue();
    }

}