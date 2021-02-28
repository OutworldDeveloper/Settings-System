using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "New Volume Setting", menuName = "Settings/Settings/Volume")]
public class Setting_Volume : Setting_Float, ISettingsGameStartedCallbackReciver
{

    [SerializeField] private AudioMixer targetMixer;
    [SerializeField] private string targetParameter;

    public override float MinValue => 0.001f;
    public override float MaxValue => 1f;

    protected override float getDefaultValue => 1f;

    public void OnGameStarted()
    {
        SetParameter();
    }

    protected override void OnValueChanged()
    {
        SetParameter();
    }

    private void SetParameter()
    {
        targetMixer.SetFloat(targetParameter, Mathf.Log10(GetValue()) * 20);
    }

}