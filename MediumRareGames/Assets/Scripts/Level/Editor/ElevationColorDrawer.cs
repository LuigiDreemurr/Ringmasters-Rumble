/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   ElevationColorDrawer
       - The drawer ElevationColor (LevelBuilderData subclass)

   Details:
       - Giving a serialized variable the [Lockable] attribute will make it
         uneditable while locked, and editable while unlocked in the unity 
         inspector
-----------------------------------------------------------------------------
*/

using UnityEditor;
using UnityEngine;

namespace Level
{
    namespace Tools
    {
        [CustomPropertyDrawer(typeof(Settings.Level.Builder.ElevationColor))]
        public class ElevationColorDrawer : PropertyDrawer
        {
            #region Constants
            private const int elevationWidth = 50; //Int field width
            private const int space = 2; //Space between int field and property field
            private const int rightPadding = 20; //Spacing right of the property field
            #endregion

            /// <summary>What is the height of the property</summary>
            public override float GetPropertyHeight(SerializedProperty _Prop, GUIContent _Label)
            {
                //Sets it up so the children fold out properly
                return EditorGUI.GetPropertyHeight(_Prop, _Label, true);
            }

            /// <summary>What is drawn</summary>
            public override void OnGUI(Rect _Pos, SerializedProperty _Prop, GUIContent _Label)
            {
                SerializedProperty elevation = _Prop.FindPropertyRelative("elevation");
                SerializedProperty material = _Prop.FindPropertyRelative("material");

                //We don't want user's to change the elevation from the ElevationColor
                GUI.enabled = false;
                elevation.intValue = EditorGUI.IntField(new Rect(_Pos.x, _Pos.y, elevationWidth, _Pos.height), elevation.intValue);
                GUI.enabled = true;

                EditorGUI.PropertyField(new Rect(_Pos.x + elevationWidth + space, _Pos.y, _Pos.width - elevationWidth - rightPadding, _Pos.height), material, GUIContent.none);
            }

        }
    }
}