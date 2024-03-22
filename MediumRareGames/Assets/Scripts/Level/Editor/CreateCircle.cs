/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   CreateCircle
       - A scriptable wizard that will create a circle of tiles

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
        public class CreateCircle : Create
        {
            [SerializeField] private string m_circleName = "Circle"; //Parent object name
            [SerializeField] private int m_radius = 5; //The circle radius

            /// <summary>Creates a circle of tiles in the scene</summary>
            /// <returns>Returns the parent object of the tiles</returns>
            protected override GameObject OnCreate()
            {
                Settings.Level.Builder data = Settings.Level.Builder.Get;

                //All points the circle contains (helps to prevent duplicates in the same position)
                HashSet<Vector2> circlePoints = new HashSet<Vector2>();

                //Create a parent object and set its position
                GameObject parent = new GameObject(m_circleName);
                parent.transform.position = Vector3.zero;

                //Want to spawn circle points in the radius range of 0-radius
                for(int r = m_radius; r >= 0; r--)
                {
                    //Go through every degree
                    for(int d=0; d<360; d++)
                    {
                        float rad = d * Mathf.Deg2Rad; //Convert to radians

                        //Calculate the point (rounding)
                        Vector2 point = new Vector2();
                        point.x = Mathf.Round(r * Mathf.Cos(rad));
                        point.y = Mathf.Round(r * Mathf.Sin(rad));

                        //If that point does not already exist in the circlePoints
                        if(!circlePoints.Contains(point))
                        {
                            //Add it to circle Points
                            circlePoints.Add(point);

                            //Create tile at that position
                            GameObject tile = Instantiate(data.TilePrefab, parent.transform);
                            tile.transform.position = new Vector3(point.x * data.TilePrefab.transform.localScale.x, 0, point.y * data.TilePrefab.transform.localScale.z);
                        }
                    }
                }

                return parent;
            }
        }
    }
}

