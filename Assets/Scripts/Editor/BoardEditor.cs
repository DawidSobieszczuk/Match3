using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(Board))]
public class BoardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);
        GUI.enabled = Application.isPlaying;
        if (GUILayout.Button("Mix board"))
        {
            MethodInfo mix = typeof(Board).GetMethod("Mix", BindingFlags.NonPublic | BindingFlags.Instance);
            mix.Invoke(target, null);
        }
        GUI.enabled = true;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Gem Prefabs", EditorStyles.boldLabel);
        SerializedProperty gemPrefabs = serializedObject.FindProperty("gemPrefabs");
        gemPrefabs.arraySize = 6;
        EditorGUILayout.PropertyField(gemPrefabs.GetArrayElementAtIndex(0), new GUIContent("Blue Gem Prefab"));
        EditorGUILayout.PropertyField(gemPrefabs.GetArrayElementAtIndex(1), new GUIContent("Green Gem Prefab"));
        EditorGUILayout.PropertyField(gemPrefabs.GetArrayElementAtIndex(2), new GUIContent("Orange Gem Prefab"));
        EditorGUILayout.PropertyField(gemPrefabs.GetArrayElementAtIndex(3), new GUIContent("Purple Gem Prefab"));
        EditorGUILayout.PropertyField(gemPrefabs.GetArrayElementAtIndex(4), new GUIContent("Red Gem Prefab"));
        EditorGUILayout.PropertyField(gemPrefabs.GetArrayElementAtIndex(5), new GUIContent("Teal Gem Prefab"));

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("SFX", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("matchAudioClip"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("errorAudioClip"));

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Others", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("size"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cellSize"));

        serializedObject.ApplyModifiedProperties();
    }
}
