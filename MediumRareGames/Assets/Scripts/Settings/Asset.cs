/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   Settings.Asset
       - An abstract class that any settings should use for keeping information
         in scriptableobjects/assets

   Details:
       - Mostly empty and may stay that way if we don't want a shared behaviour
         for all settings
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    public abstract class Asset : ScriptableObject
    {
        /// <summary>The settings directory in the asset creation menu</summary>
        public const string Directory = "Settings/";

    }
}
