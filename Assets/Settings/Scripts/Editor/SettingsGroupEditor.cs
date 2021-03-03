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

    public SettingsGroup TargetSettingsGroup { get; private set; }
    public Editor SelectedSettingEditor { get; private set; }

    private Type[] settingsTypes;
    private string[] settingsTypesNames;

    private SerializedProperty displayNameProperty;
    private SerializedProperty settingsArrayProperty;
    
    private ReorderableList settingsReorderableList;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        /*
        GUILayout.Label("Filename: ");
        string oldFilename = target.name;
        string newFilename = EditorGUILayout.DelayedTextField(oldFilename);
        if (oldFilename != newFilename)
            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(target), newFilename);
        */

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(displayNameProperty);

        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();

        settingsReorderableList.DoLayoutList();

        int selectedSettingToCreate = EditorGUILayout.Popup(0, settingsTypesNames);
        if (selectedSettingToCreate != 0)
        {
            BaseSetting setting = CreateSetting(settingsTypes[selectedSettingToCreate - 1]);
            settingsArrayProperty.arraySize++;
            settingsArrayProperty.GetArrayElementAtIndex(settingsArrayProperty.arraySize - 1).objectReferenceValue = setting;
            serializedObject.ApplyModifiedProperties();
        }

        /*
        if (selection != -1 && settingsProperty.GetArrayElementAtIndex(selection).objectReferenceValue != null)
        {
            GUILayout.Label("Filename:");

            string name = settingsProperty.GetArrayElementAtIndex(selection).objectReferenceValue.name;
            string path = AssetDatabase.GetAssetPath(settingsProperty.GetArrayElementAtIndex(selection).objectReferenceValue);

            string newName = EditorGUILayout.DelayedTextField(name);

            if (newName != name)
                AssetDatabase.RenameAsset(path, newName);
        }
        */
    }

    private void OnEnable()
    {
        if (target == null)
        {
            DestroyImmediate(this);
            return;
        }
        settingsTypes = GetInheritedClasses(typeof(BaseSetting));

        settingsTypesNames = new string[settingsTypes.Length + 1];
        settingsTypesNames[0] = "Add";
        for (int i = 0; i < settingsTypes.Length; i++)
            settingsTypesNames[i + 1] = settingsTypes[i].ToString();

        TargetSettingsGroup = target as SettingsGroup;

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
        string text = TargetSettingsGroup.Settings[index].DisplayName + " (" + 
            TargetSettingsGroup.Settings[index].GetType() + ")";
        EditorGUI.LabelField(new Rect(rect.x, rect.y, 300, EditorGUIUtility.singleLineHeight), text);
    }

    private void OnItemSelected(ReorderableList list)
    {
        BaseSetting selectedSetting = TargetSettingsGroup.Settings[list.index];
        SelectedSettingEditor = CreateEditor(selectedSetting);
    }

    private void OnItemRemoved(ReorderableList list)
    {
        if (!EditorUtility.DisplayDialog("Are you sure?", "Delete " + 
            TargetSettingsGroup.Settings[list.index].DisplayName + "?", "Delete", "Cancel"))
            return;

        UnityEngine.Object targetObject = settingsArrayProperty.GetArrayElementAtIndex(list.index).objectReferenceValue;

        settingsArrayProperty.arraySize--;
        serializedObject.ApplyModifiedProperties();

        string path = AssetDatabase.GetAssetPath(targetObject);
        AssetDatabase.DeleteAsset(path);

        AssetDatabase.Refresh();

        CreateReorderableList();

        SelectedSettingEditor = null;
    }

    private Type[] GetInheritedClasses(Type targetType)
    {
        return Assembly.GetAssembly(targetType).GetTypes().
            Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType.IsSubclassOf(targetType)).ToArray();
    }

    // Could be static in the SettingsWizzaard
    private BaseSetting CreateSetting(Type targetType)
    {
        BaseSetting setting = CreateInstance(targetType) as BaseSetting;

        if (!AssetDatabase.IsValidFolder("Assets/Settings"))
            AssetDatabase.CreateFolder("Assets/", "Settings");

        if (!AssetDatabase.IsValidFolder("Assets/Settings/Resources"))
            AssetDatabase.CreateFolder("Assets/Settings", "Resources");

        if (!AssetDatabase.IsValidFolder("Assets/Settings/Resources/Variables"))
            AssetDatabase.CreateFolder("Assets/Settings/Resources", "Variables");

        string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Settings/Resources/Variables/NewSetting.asset");

        AssetDatabase.CreateAsset(setting, path);

        return setting;
    }

}