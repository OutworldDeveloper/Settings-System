using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;

public class SettingsWizzard : EditorWindow
{

    [MenuItem("Window/Settings Wizzard")]
    public static void ShowSettingsWizzard()
    {
        GetWindow<SettingsWizzard>("Settings Wizzard");
    }

    private List<SettingsGroup> groups;
    private Editor selectedEditor;
    private ReorderableList reorderableList;

    private void OnEnable()
    {
        groups = new List<SettingsGroup>();
        RefreshGroupsList();
        reorderableList = new ReorderableList(groups, typeof(SettingsGroup));
        reorderableList.drawHeaderCallback += DrawHeader;
        reorderableList.onSelectCallback += OnItemSelected;
        reorderableList.onRemoveCallback += OnItemRemoved;
        reorderableList.onAddCallback += OnItemAdded;

        maxSize = new Vector2(730f, 600f);
        minSize = maxSize;
    }

    private void RefreshGroupsList()
    {
        groups.Clear();
        foreach (var item in Settings.GetGroups())
            groups.Add(item);
    }

    private void OnItemAdded(ReorderableList list)
    {
        AddNewGroup();
    }

    private void OnItemRemoved(ReorderableList list)
    {
        if (!EditorUtility.DisplayDialog("Are you sure?", "Delete " +
            groups[list.index].DisplayName + "? All the variables will be deleted too!", "Delete", "Cancel"))
            return;

        foreach (BaseSetting setting in groups[list.index].Settings)
        {
            string settingPath = AssetDatabase.GetAssetPath(setting);
            AssetDatabase.DeleteAsset(settingPath);
        }

        string path = AssetDatabase.GetAssetPath(groups[list.index]);
        AssetDatabase.DeleteAsset(path);
        AssetDatabase.Refresh();

        selectedEditor = null;

        RefreshGroupsList();
    }

    private void OnItemSelected(ReorderableList list)
    {
        selectedEditor = null;

        if (groups[list.index])
            selectedEditor = Editor.CreateEditor(groups[list.index]);

        foreach (var group in groups)
        {
            SerializedObject so = new SerializedObject(group);
            so.FindProperty("priority").intValue = groups.IndexOf(group);
            so.ApplyModifiedProperties();
        }
    }

    private void DrawHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "Groups");
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical(GUILayout.Width(300));
        DrawFirstColum();
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(10);
        EditorGUILayout.BeginVertical(GUILayout.Width(420));
        DrawSecondColum();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    private void DrawSecondColum()
    {
        selectedEditor?.OnInspectorGUI();
    }

    private void DrawFirstColum()
    {
        reorderableList.DoLayoutList();
    }

    private void AddNewGroup()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Settings"))
            AssetDatabase.CreateFolder("Assets/", "Settings");

        if (!AssetDatabase.IsValidFolder("Assets/Settings/Resources"))
            AssetDatabase.CreateFolder("Assets/Settings", "Resources");

        if (!AssetDatabase.IsValidFolder("Assets/Settings/Resources/Groups"))
            AssetDatabase.CreateFolder("Assets/Settings/Resources", "Groups");

        string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Settings/Resources/Groups/NewSettingsGroup.asset");

        AssetDatabase.CreateAsset(CreateInstance<SettingsGroup>(), path);

        RefreshGroupsList();
    }

}