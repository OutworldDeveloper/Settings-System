using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Settings Group", menuName = "Settings/Group")]
public class SettingsGroup : ScriptableObject
{

    [SerializeField] private string _displayName = "Name";
    [SerializeField] private List<BaseSetting> _settings = new List<BaseSetting>();
    [SerializeField] private int _priority;

    public string DisplayName => _displayName;
    public List<BaseSetting> Settings => _settings;
    public int Priority => _priority;

}