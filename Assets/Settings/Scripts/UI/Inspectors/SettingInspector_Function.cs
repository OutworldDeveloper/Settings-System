using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingInspector_Function : SettingInspector<Setting_Function>
{

    [SerializeField] private Button executeButton;
    
    protected override void Present(Setting_Function settingsVariable) 
    {
        executeButton.GetComponentInChildren<Text>().text = settingsVariable.ButtonText;
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
        settingsVariable.Execute();
    }

}