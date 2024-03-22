/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   MaterialChangerWindow
       - A simple window that give sthe user buttons to change the material
         of selected tiles

   Details:
       - The available buttons works directly off of the level builder settings'
         TileMaterials list. Creating a button for every tile material
       - Simple select tiles, press the button, and it will change the material of the tiles
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Level
{
    namespace Tools
    {
        public class MaterialChangerWindow : EditorWindow
        {
            private Settings.Level.Builder m_settings;

            /// <summary>Menu Item for initializing the window</summary>
            [MenuItem("Tools/Level/Material Changer")]
            static private void Init()
            {
                //Get existing window, or if none, make a new one
                MaterialChangerWindow window = EditorWindow.GetWindow<MaterialChangerWindow>(false, "Material Changer");
                window.Show();
            }

            /// <summary>Draw the editor window</summary>
            private void OnGUI()
            {
                if (m_settings == null)
                    m_settings = Settings.Level.Builder.Get;

                //Draw a button for each material in TileMaterials (builder settings)
                foreach(Material material in m_settings.TileMaterials)
                {
                    //Button with material name
                    if(GUILayout.Button(material.name))
                    {
                        //Get the tiles from the selection game objects
                        List<GameObject> tiles = Utility.FindFilteredTiles(Selection.gameObjects, (GameObject _Obj) => { return _Obj.CompareTag("Tile"); });

                        //For every tile in the selection change the material
                        foreach(GameObject tile in tiles)
                        {
                            Renderer renderer = tile.GetComponent<Renderer>();

                            Material[] materialList = renderer.sharedMaterials;
                            materialList[1] = material;
                            renderer.sharedMaterials = materialList;
                        }

                    }
                }

            }
        }
    }
}
