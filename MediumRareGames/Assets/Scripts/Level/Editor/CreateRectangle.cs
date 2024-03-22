/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   CreateRectangle
       - A scriptable wizard that will create a rectangle of tiles

   Details:
       - It creates a parent object and then the tiles are put into that parent object
       - Dimensions/world position can be provided by the user
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
        public class CreateRectangle : Create
        {
            [SerializeField] private string m_rectangleName = "Rectangle"; //Parent object name
            [SerializeField] private Vector2 m_dimensions = new Vector2(1, 1); //The square dimensions

            /// <summary>Creates a rectangle of tiles in the scene</summary>
            /// <returns>Returns the parent object of the tiles</returns>
            protected override GameObject OnCreate()
            {
                Settings.Level.Builder data = Settings.Level.Builder.Get;

                //Create a parent object and set its position
                GameObject parent = new GameObject(m_rectangleName);
                parent.transform.position = Vector3.zero;

                //Create a tile for the area (LxW)
                for (int x = 0; x < m_dimensions.x; x++)
                {
                    for (int z = 0; z < m_dimensions.y; z++)
                    {
                        GameObject tile = Instantiate(data.TilePrefab, parent.transform);

                        tile.transform.position = new Vector3(x * data.TilePrefab.transform.localScale.x, 0, z * data.TilePrefab.transform.localScale.z);
                    }
                }

                return parent;
            }
        }
    }
}

