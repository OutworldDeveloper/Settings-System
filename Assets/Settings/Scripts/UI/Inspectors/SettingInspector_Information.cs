using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingInspector_Information : SettingInspector<Setting_Information>
{

    protected override void Present(Setting_Information settingsVariable) { }

    protected override string GetDisplayName()
    {
        return settingsVariable.DisplayName;
    }

}