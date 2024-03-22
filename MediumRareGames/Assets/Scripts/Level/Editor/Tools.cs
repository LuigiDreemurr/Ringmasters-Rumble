/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   Tools
       - The class that handle's the simple level builder tools
          
   Details:
       - All menu items are in the "Tools/LevelBuilder/" directory
       - Done using menu items and static functions
       - Contains tools for easy tile manipulation while building levels
-----------------------------------------------------------------------------
*/

using System;
using System.Collections.Generic;
//using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Level
{
    namespace Tools
    {
        [System.Serializable]
        public static class Tools
        {
            private const string path = "Tools/Level/";

            #region Helper Methods
            /// <summary>A simple wrapper that calls a function on every tile's transform</summary>
            /// <param name="_Action">A function that takes a transform and returns a result bool (unsuccessful will break out of the tile loop)</param>
            static private void AllTiles(Settings.Level.Builder _Data, Func<Transform, bool> _Action) 
            {
                //For all tiles
                foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Tile"))
                {
                    //Call action with their transform. If something goes wrong, exit the loop
                    if(!_Action(obj.transform))
                        break;
                }
            }
            #endregion

            #region Menu Item
            /// <summary>Will cleanup all the tiles by rounding their positions and updating their material</summary>
            [MenuItem(path + "Cleanup Tiles")]
            static public void CleanupTiles()
            {
                Settings.Level.Builder data = Settings.Level.Builder.Get;

                AllTiles(data, (Transform _Transform) =>
                {
                    //Round off all axes
                    float x = Mathf.Round(_Transform.position.x / data.TilePrefab.transform.localScale.x) * data.TilePrefab.transform.localScale.x;
                    float y = Mathf.Round(_Transform.position.y / data.ElevationY);
                    float z = Mathf.Round(_Transform.position.z / data.TilePrefab.transform.localScale.z) * data.TilePrefab.transform.localScale.z;

                    //Get the elevation (clamping it)
                    int elevation = (int)Mathf.Clamp(y, data.MinElevation, data.MaxElevation);

                    //Set rotation (important that this is first)
                    _Transform.rotation = data.TilePrefab.transform.rotation;

                    //Set position
                    _Transform.position = new Vector3(x, elevation * data.ElevationY, z);

                    return true;
                });
            }

            /// <summary>Simple method that utilizes 'FindFilteredTiles' to select all player spawn points</summary>
            [MenuItem(path + "Select/Player Spawn Points")]
            static public void SelectPlayerSpawnPoints()
            {
                //Find spawn point tiles
                List<GameObject> tiles = Utility.FindFilteredTiles((GameObject _Tile) => { return _Tile.GetComponent<Tile>().PlayerSpawnPoint; });

                if(tiles.Count > 0)
                    Selection.objects = tiles.ToArray();
            }

            /// <summary>Simple method that utilizes 'FindFilteredTiles' to select all weapon drop points</summary>
            [MenuItem(path + "Select/Weapon Drop Points")]
            static public void SelectWeaponDropPoints()
            {
                //Find drop point tiles
                List<GameObject> tiles = Utility.FindFilteredTiles((GameObject _Tile) => { return _Tile.GetComponent<Tile>().WeaponDropPoint; });

                if(tiles.Count > 0)
                    Selection.objects = tiles.ToArray();
            }

            /// <summary>Will select all of the overlapping tiles for the user</summary>
            [MenuItem(path + "Select/Overlap")]
            static public void SelectOverlappedTiles()
            {
                Settings.Level.Builder data = Settings.Level.Builder.Get;

                //Get all tiles
                GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

                //Initialize hashset to use for storing the tiles with overlap
                HashSet<GameObject> toSelect = new HashSet<GameObject>();

                //Calculate the max ray distance (so we don't shoot rays more than needed)
                int totalElevationCount = 5; //disabled elevation colors so hard coding 5 //data.ElevationColors.Count;
                float maxRayDistance = totalElevationCount * data.ElevationY;

                //Go through every tile
                foreach(GameObject tile in tiles)
                {

                    //Ray cast upwards from the tiles position
                    RaycastHit hitInfo;
                    if(Physics.Raycast(tile.transform.position, Vector3.up, out hitInfo, maxRayDistance))
                    {
                        //If there is a tile above add both this and that tile to the hash set
                        if(hitInfo.collider.CompareTag("Tile"))
                        {
                            toSelect.Add(tile);
                            toSelect.Add(hitInfo.collider.gameObject);
                        }
                    }
                }

                //Put the hash set into an array
                GameObject[] selection = new GameObject[toSelect.Count];
                toSelect.CopyTo(selection);

                //Assign that array to the selection
                Selection.objects = selection;
            }

            /// <summary>Will create a wall between the two tiles selected</summary>
            [MenuItem(path + "Wall %W")]
            static public void MakeWall()
            {
                Settings.Level.Builder data = Settings.Level.Builder.Get;

                //Get selected objects
                GameObject[] objs = Selection.gameObjects;

                //Make sure there is only two selected and that the two are tiles
                if(objs.Length != 2 || !objs[0].CompareTag("Tile") || !objs[1].CompareTag("Tile"))
                {
                    Debug.LogWarning("Can only create wall with two selected tiles");
                    return;
                }

                //Get the transforms for convienence
                Transform tile1 = objs[0].transform;
                Transform tile2 = objs[1].transform;

                float dist = Vector3.Distance(tile1.position, tile2.position);

                //Makes a cube that is transformed into a wall

                //Calculate the cube transform
                Vector3 pos = (tile1.position + tile2.position) / 2; //Center between 
                Vector3 scale = new Vector3(0.5f, 5, dist);
                Vector3 rot = new Vector3();
        
                //Make the cube
                GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                wall.name = "Wall";
                wall.transform.position = pos;
                wall.transform.localScale = scale;
                wall.transform.eulerAngles = rot;
                wall.transform.LookAt(tile1.transform.position);

                //Set the selection to the wall
                Selection.objects = new GameObject[] { wall };
            }
            #endregion
        }
    }
}
