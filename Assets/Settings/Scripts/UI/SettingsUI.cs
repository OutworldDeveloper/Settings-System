using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{

    [SerializeField] private SettingsGroupPresenter settingGroupPresenterPrefab;
    [SerializeField] private BaseSettingPresenter[] presentersPrefabs;
    [SerializeField] private Transform presentersParent;

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        foreach (var group in Settings.GetGroups())
        {
            Instantiate(settingGroupPresenterPrefab, presentersParent).Setup(group);
            foreach (var parameter in group.Settings)
            {
                bool found = false;
                foreach (var presenterPrefab in presentersPrefabs)
                {
                    if (presenterPrefab.TargetType != parameter.GetType()) 
                        continue;
                    Instantiate(presenterPrefab, presentersParent).Setup(parameter);
                    found = true;
                    break;
                }
                if (found)
                    continue;
                foreach (var presenterPrefab in presentersPrefabs)
                {
                    if(!parameter.GetType().IsSubclassOf(presenterPrefab.TargetType)) 
                        continue;
                    Instantiate(presenterPrefab, presentersParent).Setup(parameter);
                    break;
                }
            }      
        }
    }

}