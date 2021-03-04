using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;
using System;
using UnityEditorInternal;

[CustomEditor(typeof(SettingsGroup))]
public class SettingsGroupEditor : Editor
{

    public event Action<BaseSetting> SettingSelected;

    private SettingsGroup targetSettingsGroup;

    private Type[] settingsTypes;
    private string[] settingsTypesNames;

    private SerializedProperty displayNameProperty;
    private SerializedProperty settingsArrayProperty;
    
    private ReorderableList settingsReorderableList;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(displayNameProperty);

        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();

        settingsReorderableList.DoLayoutList();

        int selectedSettingToCreate = EditorGUILayout.Popup(0, settingsTypesNames);
        if (selectedSettingToCreate != 0)
        {
            BaseSetting setting = SettingsWizzard.CreateSetting(settingsTypes[selectedSettingToCreate - 1]);
            settingsArrayProperty.arraySize++;
            settingsArrayProperty.GetArrayElementAtIndex(settingsArrayProperty.arraySize - 1).objectReferenceValue = setting;
            serializedObject.ApplyModifiedProperties();
        }
    }

    private void OnEnable()
    {
        settingsTypes = GetInheritedClasses(typeof(BaseSetting));

        settingsTypesNames = new string[settingsTypes.Length + 1];
        settingsTypesNames[0] = "Add";
        for (int i = 0; i < settingsTypes.Length; i++)
            settingsTypesNames[i + 1] = settingsTypes[i].ToString();

        targetSettingsGroup = target as SettingsGroup;

        displayNameProperty = serializedObject.FindProperty("displayName");
        settingsArrayProperty = serializedObject.FindProperty("settings");

        CreateReorderableList();
    }

    private void CreateReorderableList()
    {
        settingsReorderableList = new ReorderableList(serializedObject, settingsArrayProperty, true, false, false, true);
        settingsReorderableList.drawHeaderCallback += DrawHeader;
        settingsReorderableList.drawElementCallback += DrawListItems;
        settingsReorderableList.onSelectCallback += OnItemSelected;
        settingsReorderableList.onRemoveCallback += OnItemRemoved;
    }

    private void DrawHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, displayNameProperty.stringValue);
    }

    private void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        string text = targetSettingsGroup.Settings[index].DisplayName + " (" + 
            targetSettingsGroup.Settings[index].GetType() + ")";
        EditorGUI.LabelField(new Rect(rect.x, rect.y, 300, EditorGUIUtility.singleLineHeight), text);
    }

    private void OnItemSelected(ReorderableList list)
    {
        BaseSetting selectedSetting = targetSettingsGroup.Settings[list.index];   
        SettingSelected?.Invoke(selectedSetting);
    }

    private void OnItemRemoved(ReorderableList list)
    {
        if (!EditorUtility.DisplayDialog("Are you sure?", "Delete " +
            targetSettingsGroup.Settings[list.index].DisplayName + "?", "Delete", "Cancel"))
            return;

        UnityEngine.Object targetObject = settingsArrayProperty.GetArrayElementAtIndex(list.index).objectReferenceValue;

        settingsArrayProperty.arraySize--;
        serializedObject.ApplyModifiedProperties();

        string path = AssetDatabase.GetAssetPath(targetObject);
        AssetDatabase.DeleteAsset(path);

        AssetDatabase.Refresh();

        CreateReorderableList();

        SettingSelected?.Invoke(null);
    }

    private Type[] GetInheritedClasses(Type targetType)
    {
        return Assembly.GetAssembly(targetType).GetTypes().
            Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType.IsSubclassOf(targetType)).ToArray();
    }

}