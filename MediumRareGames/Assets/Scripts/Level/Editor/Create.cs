/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   Create
       - A base abstract scriptable wizard class for implementing level building
         tools

   Details:
       - Currently very barebones because not far enough into this process to
         see if there is going to be important shared behaviour between 
         creation scriptable wizards
-----------------------------------------------------------------------------
*/

using UnityEngine;
using UnityEditor;

namespace Level
{
    namespace Tools
    {
        public abstract class Create : ScriptableWizard
        {
            /// <summary>Shared behaviour for when the Create button is pressed</summary>
            protected void OnWizardCreate()
            {
                //Call on create and store reference to created object
                GameObject obj = OnCreate();

                //Select created object (wrapping the object in an array)
                Selection.objects = new GameObject[] { obj };

                //Cleanup tiles
                Tools.CleanupTiles();
            }

            /// <summary>What to do when the create button is pressed</summary>
            /// <returns>The object created (to select)</returns>
            protected abstract GameObject OnCreate();

            #region Creation Menu Items
            //The path for all creation menu items
            private const string path = "Tools/Level/Create/";

            /// <summary>Menu Item to create a rectangle</summary>
            [MenuItem(path + "Rectangle %#Q")]
            private static void CreateRectangle()
            {
                ScriptableWizard.DisplayWizard<CreateRectangle>("Rectangle Creation", "Create");
            }

            /// <summary>Menu Item to create a circle</summary>
            [MenuItem(path + "Circle %#W")]
            private static void CreateCircle()
            {
                ScriptableWizard.DisplayWizard<CreateCircle>("Circle Creation", "Create");
            }
            #endregion
        }
    }
}

