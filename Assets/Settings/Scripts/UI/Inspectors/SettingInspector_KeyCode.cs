﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class SettingInspector_KeyCode : SettingInspector<Setting_KeyCode>
{

    [SerializeField] private Text keyCodeText;
    [SerializeField] private Button inputButton;

    private bool isListening;

    protected override void Present(Setting_KeyCode settingsVariable)
    {
        keyCodeText.text = settingsVariable.GetValue().ToString();
    }

    private void OnEnable()
    {
        inputButton.onClick.AddListener(StartListeningForInput);
    }

    private void OnDisable()
    {
        inputButton.onClick.RemoveListener(StartListeningForInput);
    }

    private void Update()
    {
        if (!isListening) 
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndListeningForInput();
            return;
        }

        if (Input.anyKeyDown)
        {
            foreach (KeyCode item in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(item))
                {
                    settingsVariable.SetValue(item);
                    EndListeningForInput();
                    isListening = false;
                    return;
                }
            }
        }
    }

    private void StartListeningForInput()
    {
        isListening = true;
        keyCodeText.text = "Press any key";
    }

    private void EndListeningForInput()
    {
        keyCodeText.text = settingsVariable.GetValue().ToString();
        isListening = false;
    }

}