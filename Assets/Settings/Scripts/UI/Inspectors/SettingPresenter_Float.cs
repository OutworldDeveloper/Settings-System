using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPresenter_Float : SettingPresenter<Setting_Float>
{

    [SerializeField] private Slider slider;

    protected override void Present(Setting_Float settings)
    {
        slider.minValue = settings.MinValue;
        slider.maxValue = settings.MaxValue;
        slider.value = settings.GetValue();
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
        targetSetting.SetValue(value);
        UpdateName();
    }

    protected override string GetDisplayName()
    {
        float difference = targetSetting.MaxValue - targetSetting.MinValue;
        float percentage = ((targetSetting.GetValue() - targetSetting.MinValue) / difference) * 100f;
        return base.GetDisplayName() + " " + percentage.ToString("0") + "%";
    }

}
