/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   ReadOnlyAttribute
       - An empty attribute class to be used in ReadOnlyDrawer

   Details:
       - Giving a serialized variable the [ReadOnly] attribute will make it
         uneditable in the unity inspector
-----------------------------------------------------------------------------
*/

using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Field)]
public class ReadOnlyAttribute : PropertyAttribute
{
}
