using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{

    [SerializeField] private SettingsGroupInspector groupButtonPrefab;
    [SerializeField] private BaseSettingInspector[] presentersPrefabs;
    [SerializeField] private Transform variablesParent;

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        foreach (var group in Settings.GetGroups())
        {
            Instantiate(groupButtonPrefab, variablesParent).Setup(group);
            foreach (var parameter in group.Settings)
            {
                bool found = false;
                foreach (var presenterPrefab in presentersPrefabs)
                {
                    if (presenterPrefab.TargetType != parameter.GetType()) 
                        continue;
                    Instantiate(presenterPrefab, variablesParent).Setup(parameter);
                    found = true;
                    break;
                }
                if (found)
                    continue;
                foreach (var presenterPrefab in presentersPrefabs)
                {
                    if(!parameter.GetType().IsSubclassOf(presenterPrefab.TargetType)) 
                        continue;
                    Instantiate(presenterPrefab, variablesParent).Setup(parameter);
                    break;
                }
            }      
        }
    }

}