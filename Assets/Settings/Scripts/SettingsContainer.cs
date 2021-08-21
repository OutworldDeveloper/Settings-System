using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Settings Container", menuName = "Settings/Settings Container")]
public class SettingsContainer : ScriptableObject
{

    [SerializeField] private List<SettingsGroup> _settingsGroups = new List<SettingsGroup>();
    public List<SettingsGroup> Groups => _settingsGroups;

}