using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FitBackground))]
public class FitBackgroundEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("type"));

        if((FitBackground.Types)serializedObject.FindProperty("type").enumValueIndex == FitBackground.Types.Frame)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("frameWidth"));
        }
        if ((FitBackground.Types)serializedObject.FindProperty("type").enumValueIndex != FitBackground.Types.BackgroundLight)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("board"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
