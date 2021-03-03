using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPresenter_Function : SettingPresenter<Setting_Function>
{

    [SerializeField] private Button executeButton;
    
    protected override void Present(Setting_Function setting) 
    {
        executeButton.GetComponentInChildren<Text>().text = setting.ButtonText;
    }

    private void OnEnable()
    {
        executeButton.onClick.AddListener(OnButtonPressed);
    }

    private void OnDisable()
    {
        executeButton.onClick.RemoveListener(OnButtonPressed);
    }

    public void OnButtonPressed()
    {
        targetSetting.Execute();
    }

}