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

    private List<SettingsGroup> groups = new List<SettingsGroup>();
    private ReorderableList groupsReorderableList;
    private SettingsGroupEditor settingsGroupEditor;

    private void OnEnable()
    {
        RefreshGroupsList();
        CreateReorderableList();
        minSize = new Vector2(1160f, 600f);
    }

    private void RefreshGroupsList()
    {
        groups.Clear();
        foreach (var item in Settings.GetGroups())
            groups.Add(item);
    }

    private void CreateReorderableList()
    {
        groupsReorderableList = new ReorderableList(groups, typeof(SettingsGroup));

        groupsReorderableList.drawHeaderCallback += (Rect rect) => EditorGUI.LabelField(rect, "Groups");
        groupsReorderableList.onSelectCallback += OnItemSelected;
        groupsReorderableList.onAddCallback += (ReorderableList) => CreateGroup();
        groupsReorderableList.onRemoveCallback += OnItemRemoved;
    }

    private void OnItemSelected(ReorderableList list)
    {
        settingsGroupEditor = Editor.CreateEditor(groups[list.index]) as SettingsGroupEditor;

        foreach (var group in groups)
        {
            SerializedObject so = new SerializedObject(group);
            so.FindProperty("priority").intValue = groups.IndexOf(group);
            so.ApplyModifiedProperties();
        }
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

        RefreshGroupsList();

        settingsGroupEditor = null;
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical(GUILayout.MinWidth(300));

        DrawFirstColumn();

        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(10);

        EditorGUILayout.BeginVertical(GUILayout.Width(420));

        DrawSecondColumn();

        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(10);

        EditorGUILayout.BeginVertical(GUILayout.Width(420));

        DrawThirdColumn();

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }

    private void DrawFirstColumn()
    {
        EditorGUILayout.LabelField("Groups", EditorStyles.boldLabel);
        GUILayout.Space(10);

        groupsReorderableList.DoLayoutList();
    }

    private void DrawSecondColumn()
    {
        EditorGUILayout.LabelField(settingsGroupEditor ? settingsGroupEditor.TargetSettingsGroup.DisplayName : 
            "No group selected", EditorStyles.boldLabel);
        GUILayout.Space(10);

        settingsGroupEditor?.OnInspectorGUI();
    }

    private void DrawThirdColumn()
    {
        EditorGUILayout.LabelField("Setting", EditorStyles.boldLabel);
        GUILayout.Space(10);

        settingsGroupEditor?.SelectedSettingEditor?.OnInspectorGUI();
    }

    private void CreateGroup()
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