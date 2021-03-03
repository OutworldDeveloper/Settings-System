using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPresenter_Options : SettingPresenter<Setting_Options>
{

    [SerializeField] private Dropdown dropdown;

    protected override void Present(Setting_Options setting)
    {
        dropdown.ClearOptions();
        foreach (var item in setting.Options.options)
        {
            Dropdown.OptionData optionData = new Dropdown.OptionData();
            optionData.text = item.displayName;
            dropdown.options.Add(optionData);
        }
        dropdown.value = setting.GetValue();
    }

    private void OnValueChanged(int value)
    {
        targetSetting.SetValue(value);
    }

    private void OnEnable()
    {
        dropdown.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnDisable()
    {
        dropdown.onValueChanged.RemoveListener(OnValueChanged);
    }

}