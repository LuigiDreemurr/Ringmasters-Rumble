/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   ReadOnlyDrawer
       - The drawer for the [ReadOnly] attribute

   Details:
       - Giving a serialized variable the [ReadOnly] attribute will make it
         uneditable in the unity inspector
-----------------------------------------------------------------------------
*/

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    /// <summary>What is the height of the property</summary>
    public override float GetPropertyHeight(SerializedProperty _Prop, GUIContent _Label)
    {
        //Sets it up so the children fold out properly
        return EditorGUI.GetPropertyHeight(_Prop, _Label, true);
    }

    /// <summary>What is drawn</summary>
    public override void OnGUI(Rect _Pos, SerializedProperty _Prop, GUIContent _Label)
    {
        GUI.enabled = false; //Disable it
        EditorGUI.PropertyField(_Pos, _Prop, _Label, true); //Draw the property
        GUI.enabled = true; //Enable it
    }

}
