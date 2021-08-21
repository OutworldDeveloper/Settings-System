using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;
using System;

//[CustomEditor(typeof(Setting_Volume))]
public class SettingsVolumeEditor : Editor
{

    private SerializedProperty targetDisplayNameProperty;
    private SerializedProperty targetMixerProperty;
    private SerializedProperty targetParameter;
    private List<string> exposedParameters = new List<string>();
    private int selectedParameter;
    private bool hasSelectedMixer;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUI.enabled = false;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
        GUI.enabled = true;

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(targetDisplayNameProperty);
        EditorGUILayout.PropertyField(targetMixerProperty);
        if (hasSelectedMixer)
            EditorGUILayout.PropertyField(targetParameter);

        if (EditorGUI.EndChangeCheck())
        {
            Refresh();
            serializedObject.ApplyModifiedProperties();
        }

        if (hasSelectedMixer)
        {
            int lastSelected = selectedParameter;
            selectedParameter = EditorGUILayout.Popup(selectedParameter, exposedParameters.ToArray());
            if (lastSelected != selectedParameter)
            {
                targetParameter.stringValue = exposedParameters[selectedParameter];
                serializedObject.ApplyModifiedProperties();
            }
            if (GUILayout.Button("Refresh Exposed Parameters"))
                Refresh();
        }
        else
            EditorGUILayout.HelpBox("Please select an Audio Mixer to continue.", MessageType.Warning);
    }

    private void OnEnable()
    {
        targetDisplayNameProperty = serializedObject.FindProperty("_displayName");
        targetMixerProperty = serializedObject.FindProperty("_targetMixer");
        targetParameter = serializedObject.FindProperty("_targetParameter");
        Refresh();
    }

    private void Refresh()
    {
        AudioMixer targetAudioMixer = targetMixerProperty.objectReferenceValue as AudioMixer;

        hasSelectedMixer = targetAudioMixer;

        selectedParameter = 0;

        if (!targetAudioMixer)
            return;

        Array parameters = (Array)targetAudioMixer.GetType().
            GetProperty("_targetParameter").GetValue(targetAudioMixer, null);

        exposedParameters.Clear();
        exposedParameters.Add("Not selected");

        for (int i = 0; i < parameters.Length; i++)
        {
            var o = parameters.GetValue(i);
            string Param = (string)o.GetType().GetField("_name").GetValue(o);
            exposedParameters.Add(Param);

            if (Param == serializedObject.FindProperty("_targetParameter").stringValue)
                selectedParameter = i + 1;
        }
    }

}