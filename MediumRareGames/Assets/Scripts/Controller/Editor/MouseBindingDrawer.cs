/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   MouseBindingDrawer
       - A simple property drawer for ButtonBindings.MouseBinding

   Details:
       - Two enum drop downs beside eachother
-----------------------------------------------------------------------------
*/

using UnityEngine;
using UnityEditor;

namespace Controller
{
    [CustomPropertyDrawer(typeof(ButtonBindings.MouseBinding))]
    public class MouseBindingDrawer : PropertyDrawer
    {
        /// <summary>What is drawn in the inspector</summary>
        public override void OnGUI(Rect _Pos, SerializedProperty _Prop, GUIContent _Label)
        {
            //Get properties
            SerializedProperty button = _Prop.FindPropertyRelative("m_origin");
            SerializedProperty mouseButton = _Prop.FindPropertyRelative("m_link");

            //Calc width 
            float width = _Pos.width / 2;

            //Draw properties
            EditorGUI.PropertyField(new Rect(_Pos.x, _Pos.y, width, _Pos.height), button, GUIContent.none);
            EditorGUI.PropertyField(new Rect(_Pos.x + width, _Pos.y, width, _Pos.height), mouseButton, GUIContent.none);
        }
    }
}
