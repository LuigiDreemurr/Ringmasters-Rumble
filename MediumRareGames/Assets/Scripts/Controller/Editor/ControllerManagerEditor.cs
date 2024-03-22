/*
 -----------------------------------------------------------------------------
        Created By Wesley Ducharme
 -----------------------------------------------------------------------------
    ControllerManagerEditor
        - A custom inspector for the controller manager

    Details:
        - Will draw the default inspector then draw the custom stuff
        - Will draw three reoderable lists that represent the input events
          the user wants to use.
 -----------------------------------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace XInput
{
    public class ReorderableInputList
    {
        #region Data Members
        private SerializedObject m_serializedObject;
        private ReorderableList m_list;
        private Func<int, string> m_getMenuName;

        private string m_header;
        private string m_usedProp;
        private string m_unusedProp;

        public string Header { get { return m_header; } }
        public string Used { get { return m_usedProp; } }
        public string Unused { get { return m_unusedProp; } }
        #endregion

        public ReorderableInputList(SerializedObject _SerializedObject, string _UsedProp, string _UnusedProp, string _Header, Func<int, string> _MenuName)
        {
            m_serializedObject = _SerializedObject;
            m_getMenuName = _MenuName;
            m_header = _Header;
            m_usedProp = _UsedProp;
            m_unusedProp = _UnusedProp;

            m_list = new ReorderableList(_SerializedObject, _SerializedObject.FindProperty(_UsedProp), true, true, true, true);

            m_list.drawHeaderCallback = DrawHeader;
            m_list.drawElementCallback = DrawElement;
            m_list.onAddDropdownCallback = OnAdd;
            m_list.onRemoveCallback = OnRemove;

        }

        #region Public Methods
        public void DoLayoutList()
        {
            if (EditorApplication.isPlaying)
                ToggleEditable(false);

            m_list.DoLayoutList();
            ToggleEditable(true);
        }
        #endregion

        #region Private Methods
        private void ToggleEditable(bool _CanEdit)
        {
            m_list.draggable = _CanEdit;
            m_list.displayAdd = _CanEdit;
            m_list.displayRemove = _CanEdit;
        }

        private void DrawHeader(Rect _Rect)
        {
            EditorGUI.LabelField(_Rect, m_header);
        }

        private void DrawElement(Rect _Rect, int _Index, bool _IsActive, bool _IsFocused)
        {
            //Get the element at the index
            var element = m_list.serializedProperty.GetArrayElementAtIndex(_Index);

            //Slightly adjust the position
            _Rect.y += 2;

            //Draw the enum
            GUI.enabled = false;
            EditorGUI.PropertyField(_Rect, element, GUIContent.none);
            GUI.enabled = true;
        }

        private void OnAdd(Rect _Rect, ReorderableList _List)
        {
            GenericMenu menu = new GenericMenu();
            SerializedProperty unusedList = m_serializedObject.FindProperty(m_unusedProp);
            for (int i = 0; i < unusedList.arraySize; i++)
            {
                var element = unusedList.GetArrayElementAtIndex(i);

                int enumIndex = element.enumValueIndex;

                menu.AddItem(new GUIContent(m_getMenuName(enumIndex)), false, OnAddClickHandler,
                             new RemoveInfo(i, unusedList, enumIndex, _List));
            }

            menu.ShowAsContext();
        }

        private void OnAddClickHandler(object _Target)
        {
            RemoveInfo info = (RemoveInfo)_Target;

            //Remove the value from the old list
            info.availList.DeleteArrayElementAtIndex(info.availListIndex);

            //Add value to reorderable list
            int index = info.list.serializedProperty.arraySize;
            info.list.serializedProperty.arraySize++;
            info.list.index = index;

            info.list.serializedProperty.GetArrayElementAtIndex(index).enumValueIndex = info.valueIndex;

            m_serializedObject.ApplyModifiedProperties();
        }

        private void OnRemove(ReorderableList _List)
        {
            SerializedProperty unusedList = m_serializedObject.FindProperty(m_unusedProp);

            int enumVal = _List.serializedProperty.GetArrayElementAtIndex(_List.index).enumValueIndex;
            _List.serializedProperty.DeleteArrayElementAtIndex(_List.index);

            if (_List.index == _List.serializedProperty.arraySize)
                _List.index--;

            int availIndex = unusedList.arraySize;
            unusedList.arraySize++;
            unusedList.GetArrayElementAtIndex(availIndex).enumValueIndex = enumVal;
        }
        #endregion
    }

    [CustomEditor(typeof(ControllerManager))]
    public class ControllerManagerEditor : Editor
    {
        private ReorderableInputList m_buttons;
        private ReorderableInputList m_axes;
        private ReorderableInputList m_dualAxes;

        private void OnEnable()
        {
            m_buttons = new ReorderableInputList(serializedObject, "m_usedButtons", "m_unusedButtons", "Button Events", (enumIndex) => ((Button)enumIndex).ToString());
            m_axes = new ReorderableInputList(serializedObject, "m_usedAxes", "m_unusedAxes", "Axis Events", (enumIndex) => ((Axis)enumIndex).ToString());
            m_dualAxes = new ReorderableInputList(serializedObject, "m_usedDualAxes", "m_unusedDualAxes", "DualAxis Events", (enumIndex) => ((DualAxis)enumIndex).ToString());
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            //Draw the default inspector
            DrawDefaultInspector();

            m_buttons?.DoLayoutList();
            m_axes?.DoLayoutList();
            m_dualAxes?.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }
    }

    public class RemoveInfo
    {
        public int availListIndex;
        public SerializedProperty availList;
        public int valueIndex;
        public ReorderableList list;

        public RemoveInfo(int _AvailIndex, SerializedProperty _AvailList, int _ValueIndex, ReorderableList _List)
        {
            availListIndex = _AvailIndex;
            availList = _AvailList;
            valueIndex = _ValueIndex;
            list = _List;
        }
    }
}