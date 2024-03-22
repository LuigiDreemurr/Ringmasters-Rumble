using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerIdentifier))]
public class PlayerIdentifierEditor : Editor
{
    private SerializedProperty m_usingFadeProp;
    private SerializedProperty m_delayProp;
    private SerializedProperty m_durationProp;

    private void OnEnable()
    {
        m_usingFadeProp = serializedObject.FindProperty("m_fadeAtStart");
        m_delayProp = serializedObject.FindProperty("m_fadeDelay");
        m_durationProp = serializedObject.FindProperty("m_fadeDuration");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        base.OnInspectorGUI();

        if(m_usingFadeProp.boolValue)
        {
            EditorGUILayout.PropertyField(m_delayProp);
            EditorGUILayout.PropertyField(m_durationProp);
        }


        serializedObject.ApplyModifiedProperties();
    }
}
