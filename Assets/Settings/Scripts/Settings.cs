using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class Settings : MonoBehaviour
{

    private const string GroupsPath = "Groups";
    private const string ManagerName = "Settings Manager";

    public static event Action OnSettingsChanged;
    public static SettingsGroup[] Groups { get; private set; } 

    private static Settings _instance;
    private static bool _gameStartedCallbackSended;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnBeforeSceneLoadRuntimeMethod()
    {
        if (_instance)
            return;

        Groups = LoadAndSortSettingsGroups();

        _instance = new GameObject().AddComponent<Settings>();
        _instance.gameObject.name = ManagerName;
        DontDestroyOnLoad(_instance);
    }

    public static void SettingsChanged()
    {
        OnSettingsChanged?.Invoke();
    }

    public static void ResetSettings()
    {
        foreach (var group in Groups)
        {
            group.Settings.ForEach(setting => setting.Reset());
        }
    }

    private static SettingsGroup[] LoadAndSortSettingsGroups()
    {
        SettingsGroup[] groups = Resources.LoadAll<SettingsGroup>(GroupsPath);
        return groups.OrderBy(group => group.Priority).ToArray();
    }

    private void Awake()
    {
        if (_instance)
            Destroy(gameObject);
        else
        {
            _instance = this;
            DontDestroyOnLoad(_instance);
        }
    }

    private void Start()
    {
        if (_gameStartedCallbackSended)
            return;

        foreach (var group in Groups)
        {
            group.Settings.ForEach(setting => setting.OnGameStarted());
        }

        _gameStartedCallbackSended = true;
    }

}