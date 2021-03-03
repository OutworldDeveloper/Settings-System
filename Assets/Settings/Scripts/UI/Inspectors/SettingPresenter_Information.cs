﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPresenter_Information : SettingPresenter<Setting_Information>
{

    protected override void Present(Setting_Information setting) { }

    protected override string GetDisplayName()
    {
        return targetSetting.DisplayName;
    }

}