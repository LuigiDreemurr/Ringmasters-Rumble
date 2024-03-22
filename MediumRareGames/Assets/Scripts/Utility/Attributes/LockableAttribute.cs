/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   LockableAttribute
       - An empty attribute class to be used in ReadOnlyDrawer

   Details:
       - Giving a serialized variable the [Lockable] attribute will make it
         uneditable while locked, and editable while unlocked in the unity 
         inspector
-----------------------------------------------------------------------------
*/

using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Field)]
public class LockableAttribute : PropertyAttribute
{
}