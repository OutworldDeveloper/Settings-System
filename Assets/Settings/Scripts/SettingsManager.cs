using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class SettingsManager : MonoBehaviour
{

    private const string ContainerPath = "SettingsContainer";
    private const string ManagerName = "Settings Manager";

    public static event Action OnSettingsChanged;
    public static SettingsContainer Container { get; private set; }

    private static SettingsManager _instance;
    private static bool _gameStartedCallbackSended;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnBeforeSceneLoadRuntimeMethod()
    {
        if (_instance)
            return;

        Container = Resources.Load<SettingsContainer>(ContainerPath);

        _instance = new GameObject().AddComponent<SettingsManager>();
        _instance.gameObject.name = ManagerName;
        DontDestroyOnLoad(_instance);
    }

    public static void SettingsChanged()
    {
        OnSettingsChanged?.Invoke();
    }

    public static void ResetSettings()
    {
        ForEachSetting(setting => setting.Reset());
    }

    public static void ForEachSetting(Action<BaseSetting> action)
    {
        Container.Groups.ForEach(group =>
        {
            group.Settings.ForEach(setting => action.Invoke(setting));
        });
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

        ForEachSetting(setting => setting.OnGameStarted());

        _gameStartedCallbackSended = true;
    }

}