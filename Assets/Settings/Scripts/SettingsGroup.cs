using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Settings Group", menuName = "Settings/Group")]
public class SettingsGroup : ScriptableObject
{

    [SerializeField] private string displayName = "Name";
    [SerializeField] private List<BaseSetting> settings = new List<BaseSetting>();
    [SerializeField] private int priority;

    public string DisplayName => displayName;
    public List<BaseSetting> Settings => settings;
    public int Priority => priority;

}