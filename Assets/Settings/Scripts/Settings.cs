using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Settings : MonoBehaviour
{

    private const string GroupsPath = "Groups";

    public static event Action OnSettingsChanged;

    private static Settings instance;
    private static bool gameStartedCallbackSended;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnBeforeSceneLoadRuntimeMethod()
    {
        if (instance)
            return;
        instance = new GameObject().AddComponent<Settings>();
        instance.gameObject.name = "Settings Manager";
        DontDestroyOnLoad(instance);
    }

    public static void SettingsChanged()
    {
        OnSettingsChanged?.Invoke();
    }

    public static void ResetSettings()
    {
        foreach (var group in GetGroups())
            foreach (var parameter in group.Settings)
                (parameter as ISettingsResetable)?.ResetSetting();
    }

    public static SettingsGroup[] GetGroups()
    {
        SettingsGroup[] groups = Resources.LoadAll<SettingsGroup>(GroupsPath);
        return groups.OrderBy(o => o.Priority).ToArray();
    }

    private void Awake()
    {
        if (instance)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
    }

    private void Start()
    {
        if (gameStartedCallbackSended)
            return;
        foreach (var group in GetGroups())
            foreach (var parameter in group.Settings)
                (parameter as ISettingsGameStartedCallbackReciver)?.OnGameStarted();
        gameStartedCallbackSended = true;
    }

}