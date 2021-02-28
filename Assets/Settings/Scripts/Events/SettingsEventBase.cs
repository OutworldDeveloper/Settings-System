using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class SettingsEventBase : MonoBehaviour
{

    private void OnEnable()
    {
        Settings.OnSettingsChanged += SettingsUpdated;
    }

    private void OnDisable()
    {
        Settings.OnSettingsChanged -= SettingsUpdated;
    }

    private void Awake()
    {
        SettingsUpdated();
    }

    protected abstract void SettingsUpdated();

}