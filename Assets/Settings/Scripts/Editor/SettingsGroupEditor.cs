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

    public Editor SelectedEditor => targetEditor;

    private SettingsGroup targetSettingsGroup;
    private Type[] settingsTypes;
    private string[] settingsTypesNames;
    private SerializedProperty displayNameProperty;
    private SerializedProperty settingsProperty;
    private ReorderableList reorderableList;
    private int selection = -1;
    private Editor targetEditor;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUILayout.Label("Filename: ");
        string oldFilename = target.name;
        string newFilename = EditorGUILayout.DelayedTextField(oldFilename);
        if (oldFilename != newFilename)
            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(target), newFilename);

        //EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Name"));

        EditorGUILayout.PropertyField(displayNameProperty);

        DrawHorizontalBorder();

        int i = EditorGUILayout.Popup(0, settingsTypesNames);

        if (i != 0)
        {
            BaseSetting setting = CreateInstance(settingsTypes[i - 1]) as BaseSetting;

            if (!AssetDatabase.IsValidFolder("Assets/Settings"))
                AssetDatabase.CreateFolder("Assets/", "Settings");

            if (!AssetDatabase.IsValidFolder("Assets/Settings/Resources"))
                AssetDatabase.CreateFolder("Assets/Settings", "Resources");

            if (!AssetDatabase.IsValidFolder("Assets/Settings/Resources/Variables"))
                AssetDatabase.CreateFolder("Assets/Settings/Resources", "Variables");

            string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Settings/Resources/Variables/NewSetting.asset");

            AssetDatabase.CreateAsset(setting, path);

            settingsProperty.arraySize++;
            settingsProperty.GetArrayElementAtIndex(settingsProperty.arraySize - 1).objectReferenceValue =
                AssetDatabase.LoadAssetAtPath(path, typeof(BaseSetting));

            RefreshEditors();

            serializedObject.ApplyModifiedProperties();
            return;
        }

        reorderableList.DoLayoutList();

        DrawHorizontalBorder();

        if (selection != -1 && settingsProperty.GetArrayElementAtIndex(selection).objectReferenceValue != null)
        {
            GUILayout.Label("Filename:");

            string name = settingsProperty.GetArrayElementAtIndex(selection).objectReferenceValue.name;
            string path = AssetDatabase.GetAssetPath(settingsProperty.GetArrayElementAtIndex(selection).objectReferenceValue);

            string newName = EditorGUILayout.DelayedTextField(name);

            if (newName != name)
                AssetDatabase.RenameAsset(path, newName);
        }

        EditorGUILayout.Space(10);

        //targetEditor?.OnInspectorGUI();

        serializedObject.ApplyModifiedProperties();
    }

    private static void DrawHorizontalBorder()
    {
        EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);

        EditorGUILayout.Space(15);
    }

    private void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        if (!targetSettingsGroup.Settings[index])
        {
            settingsProperty.DeleteArrayElementAtIndex(index);
            return;
        }
        EditorGUI.LabelField(new Rect(rect.x, rect.y, 300, EditorGUIUtility.singleLineHeight),
            (targetSettingsGroup.Settings[index].DisplayName + " (" +
            targetSettingsGroup.Settings[index].GetType() + ")"));
    }

    private void DrawHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, displayNameProperty.stringValue);
    }

    private void OnItemSelected(ReorderableList list)
    {
        selection = list.index;
        RefreshEditors();
    }

    private void OnItemRemoved(ReorderableList list)
    {
        if (!EditorUtility.DisplayDialog("Are you sure?", "Delete " + 
            targetSettingsGroup.Settings[list.index].DisplayName + "?", "Delete", "Cancel"))
            return;

        string path = AssetDatabase.GetAssetPath(settingsProperty.GetArrayElementAtIndex(list.index).objectReferenceValue);

        settingsProperty.DeleteArrayElementAtIndex(list.index);

        AssetDatabase.DeleteAsset(path);
        AssetDatabase.Refresh();

        selection = -1;

        RefreshEditors();
    }

    private Type[] GetInheritedClasses(Type MyType)
    {
        return Assembly.GetAssembly(MyType).GetTypes().
            Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType.IsSubclassOf(MyType)).ToArray();
    }

    private void OnEnable()
    {
        settingsTypes = GetInheritedClasses(typeof(BaseSetting));

        settingsTypesNames = new string[settingsTypes.Length + 1];
        settingsTypesNames[0] = "Add";
        for (int i = 0; i < settingsTypes.Length; i++)
            settingsTypesNames[i + 1] = settingsTypes[i].ToString();

        targetSettingsGroup = target as SettingsGroup;

        settingsProperty = serializedObject.FindProperty("settings");
        displayNameProperty = serializedObject.FindProperty("displayName");

        reorderableList = new ReorderableList(serializedObject, settingsProperty, true, false, false, true);
        reorderableList.drawElementCallback += DrawListItems;
        reorderableList.drawHeaderCallback += DrawHeader;
        reorderableList.onSelectCallback += OnItemSelected;
        reorderableList.onRemoveCallback += OnItemRemoved;

        RefreshEditors();
    }

    private void RefreshEditors()
    {
        targetEditor = null;

        if (selection == -1)
            return;

        UnityEngine.Object selectedObject = settingsProperty.GetArrayElementAtIndex(selection).objectReferenceValue;
        targetEditor = CreateEditor(selectedObject);
    }

}