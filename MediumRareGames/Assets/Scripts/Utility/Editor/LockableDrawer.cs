/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   LockableDrawer
       - The drawer for the [Lockable] attribute

   Details:
       - Giving a serialized variable the [Lockable] attribute will make it
         uneditable while locked, and editable while unlocked in the unity 
         inspector
-----------------------------------------------------------------------------
*/

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(LockableAttribute))]
public class LockableDrawer : PropertyDrawer
{
    #region Constants
    private const int ToggleWidth = 20;
    private const int Space = 20;
    #endregion

    private bool m_locked = false; //Have it unlocked when starting

    /// <summary>What is the height of the property</summary>
    public override float GetPropertyHeight(SerializedProperty _Prop, GUIContent _Label)
    {
        //Sets it up so the children fold out properly
        return EditorGUI.GetPropertyHeight(_Prop, _Label, true);
    }

    /// <summary>What is drawn</summary>
    public override void OnGUI(Rect _Pos, SerializedProperty _Prop, GUIContent _Label)
    {
        GUI.enabled = !m_locked; //Set enabled based on the lock

        //Draw the main property
        EditorGUI.PropertyField(new Rect(_Pos.x, _Pos.y, _Pos.width - ToggleWidth, _Pos.height),
                                _Prop, _Label, true); //Draw the property

        GUI.enabled = true; //Make sure its enabled afterwards

        //Draw the lock toggle
        m_locked = EditorGUI.Toggle(new Rect(_Pos.width - ToggleWidth + Space, _Pos.y, ToggleWidth, _Pos.height), m_locked);
    }

}