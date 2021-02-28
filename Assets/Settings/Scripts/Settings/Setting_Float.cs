using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Float Setting", menuName = "Settings/Settings/Float")]
public class Setting_Float : Setting<float>
{

    [SerializeField] private float minValue;
    [SerializeField] private float maxValue;

    public virtual float MinValue => minValue;
    public virtual float MaxValue => maxValue;

    protected override float LoadValue()
    {
        return PlayerPrefs.GetFloat(name);
    }

    protected override void SaveValue(float value)
    {
        PlayerPrefs.SetFloat(name, value);
    }

}