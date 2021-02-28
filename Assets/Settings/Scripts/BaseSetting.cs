using UnityEngine;

public abstract class BaseSetting : ScriptableObject
{

    [SerializeField] private string displayName;

    public string DisplayName => displayName;

    protected void SettingsChanged()
    {
        Settings.SettingsChanged();
    }

}
