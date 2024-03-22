/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   AxisBindingDrawer
       - A simple property drawer for AxisBindings.AxisBinding

   Details:
       - An enum drop down and a text field beside eachother
-----------------------------------------------------------------------------
*/

using UnityEngine;
using UnityEditor;

namespace Controller
{
    [CustomPropertyDrawer(typeof(AxisBindings.AxisBinding))]
    public class AxisBindingDrawer : PropertyDrawer
    {
        /// <summary>What is drawn in the inspector</summary>
        public override void OnGUI(Rect _Pos, SerializedProperty _Prop, GUIContent _Label)
        {
            //Get properties
            SerializedProperty controllerAxis = _Prop.FindPropertyRelative("m_origin");
            SerializedProperty stringAxis = _Prop.FindPropertyRelative("m_link");

            //Calc width 
            float width = _Pos.width / 2;

            //Draw properties
            EditorGUI.PropertyField(new Rect(_Pos.x, _Pos.y, width, _Pos.height), controllerAxis, GUIContent.none);
            EditorGUI.PropertyField(new Rect(_Pos.x + width, _Pos.y, width, _Pos.height), stringAxis, GUIContent.none);
        }
    }
}
