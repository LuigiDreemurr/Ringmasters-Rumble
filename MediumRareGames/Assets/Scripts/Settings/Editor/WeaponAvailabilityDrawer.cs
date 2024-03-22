using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Settings.WeaponPickup.WeaponAvailability))]
public class WeaponAvailabilityDrawer : PropertyDrawer
{
    public override void OnGUI(Rect _Pos, SerializedProperty _Prop, GUIContent _Label)
    {
        SerializedProperty type = _Prop.FindPropertyRelative("m_type");
        SerializedProperty prefab = _Prop.FindPropertyRelative("m_prefab");
        SerializedProperty available = _Prop.FindPropertyRelative("m_available");

        _Pos.width /= 3;
        EditorGUI.PropertyField(_Pos, type, GUIContent.none);
        _Pos.x += _Pos.width;

        EditorGUI.PropertyField(_Pos, prefab, GUIContent.none);
        _Pos.x += _Pos.width;
        EditorGUI.PropertyField(_Pos, available, GUIContent.none);
    }

}
