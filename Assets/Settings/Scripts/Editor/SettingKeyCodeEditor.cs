using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Setting_KeyCode))]
public class SettingKeyCodeEditor : Editor
{

    private bool isListeningToInput = false;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        if (!isListeningToInput)
        {
            if (GUILayout.Button("Listen to input"))
                isListeningToInput = true;
            return;
        }

        GUI.color = Color.red;

        if (GUILayout.Button("Cancel"))
        {
            isListeningToInput = false;
            return;
        }

        GUI.color = Color.white;

        EditorGUILayout.HelpBox("Press any key to bind...", MessageType.Info);

        Event currentEvent = Event.current;
        if (currentEvent.type == EventType.KeyDown)
        {
            if (currentEvent.keyCode == KeyCode.Escape)
            {
                isListeningToInput = false;
                return;
            }
            serializedObject.FindProperty("defaultValue").intValue = (int)currentEvent.keyCode;
            isListeningToInput = false;
            serializedObject.ApplyModifiedProperties();
        }
    }

}