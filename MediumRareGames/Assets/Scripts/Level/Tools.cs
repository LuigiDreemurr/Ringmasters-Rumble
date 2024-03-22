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

            ///// <summary>A simple wrapper for setting a tile's Y position/scale</summary>
            ///// <param name="_Transform">The Tile transform being modified</param>
            ///// <param name="_PosAction">A function that takes the transform position and returns a value for Y position</param>
            ///// <param name="_ScaleAction">A function that takes the transform position and returns a value for Y scale</param>
            //static private void DepthPositioning(Transform _Transform, Func<Vector3, float> _PosAction, Func<Vector3, float> _ScaleAction)
            //{
            //    //Get copies of transform info
            //    Vector3 position = _Transform.position;
            //    Vector3 scale = _Transform.localScale;

            //    //Modify values
            //    position.y = _PosAction(_Transform.position);
            //    scale.y = _ScaleAction(_Transform.position);

            //    //Set the values back into the transform
            //    _Transform.position = position;
            //    _Transform.localScale = scale;
            //}

            ///// <summary>Will expand all the tiles to make them reach the depth while maintaining the same topside position</summary>
            //static private void ExpandToDepth()
            //{
            //    Settings.Level.Builder data = Settings.Level.Builder.Get;

            //    AllTiles(data, (Transform _Transform) =>
            //    {
            //        //Make sure we are expanding an already collapsed set of tiles
            //        if (_Transform.localScale.y != 1)
            //        {
            //            Debug.LogWarning("Only expand depth of tiles with a collapsed depth");
            //            return false;
            //        }

            //        DepthPositioning(_Transform, (Vector3 _Pos) => { return (_Pos.y + data.ExpandToDepth) / 2; }, 
            //                                     (Vector3 _Pos) => { return _Pos.y - data.ExpandToDepth + 1; });

            //        return true;
            //    });
            //}

            ///// <summary>Will collapse all the tiles to make them match their original position/scale</summary>
            //static private void CollapseFromDepth()
            //{
            //    Settings.Level.Builder data = Settings.Level.Builder.Get;

            //    AllTiles(data, (Transform _Transform) =>
            //    {
            //        if (_Transform.localScale.y == 1)
            //        {
            //            Debug.LogWarning("Only collapse depth of tiles with an expanded depth (Ctrl+Shift+D)");
            //            return false;
            //        }

            //        DepthPositioning(_Transform, (Vector3 _Pos) => { return (_Pos.y * 2) - data.ExpandToDepth; }, (Vector3 _Pos) => { return 1; });

            //        return true;
            //    });
            //}
            #endregion

            #region Menu Item
            /// <summary>Expands the tiles if they are collapsed and collapses the tiles if they are expanded</summary>
            //[MenuItem(path + "Toggle Depth %D")]
            //static public void ToggleDepth()
            //{
            //    Settings.Level.Builder data = Settings.Level.Builder.Get;

            //    if (GameObject.FindGameObjectWithTag("Tile").transform.localScale.y == data.TilePrefab.transform.localScale.y)
            //        ExpandToDepth();
            //    else
            //        CollapseFromDepth();
            //}

            /// <summary>Will cleanup all the tiles by rounding their positions and updating their material</summary>
            [MenuItem(path + "Cleanup Tiles %T")]
            static public void CleanupTiles()
            {
                Settings.Level.Builder data = Settings.Level.Builder.Get;

                AllTiles(data, (Transform _Transform) =>
                {
                    //If the tiles are expanded, collapse them
                    //if (_Transform.localScale.y != data.TilePrefab.transform.localScale.y)
                    //    CollapseFromDepth();

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

                    //Elevation materials no longer a thing
                    //Set material
                    //_Transform.GetComponent<Renderer>().material = data.GetMaterial(elevation);

                    return true;
                });
            }

            /// <summary>Simple method that utilizes 'FindFilteredTiles' to select all player spawn points</summary>
            [MenuItem(path + "Select/Player Spawn Points")]
            static public void SelectPlayerSpawnPoints()
            {
                //Find spawn point tiles
                List<GameObject> tiles = FindFilteredTiles((GameObject _Tile) => { return _Tile.GetComponent<Tile>().PlayerSpawnPoint; });

                if(tiles.Count > 0)
                    Selection.objects = tiles.ToArray();
            }

            /// <summary>Simple method that utilizes 'FindFilteredTiles' to select all weapon drop points</summary>
            [MenuItem(path + "Select/Weapon Drop Points")]
            static public void SelectWeaponDropPoints()
            {
                //Find drop point tiles
                List<GameObject> tiles = FindFilteredTiles((GameObject _Tile) => { return _Tile.GetComponent<Tile>().WeaponDropPoint; });

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

            #region Public Methods

            /// <summary>Will find all the tiles based on a given condition</summary>
            /// <param name="_Condition">The function that determines whether or not you want the tile</param>
            /// <returns>The list of filtered tile gameobjects</returns>
            static public List<GameObject> FindFilteredTiles(GameObject[] _Tiles, Func<GameObject, bool> _Condition)
            {
                //Init list for filtered game objects
                List<GameObject> filteredTiles = new List<GameObject>();

                //Get all the tiles
                GameObject[] unfilteredTiles = _Tiles;

                //Filter the unfilteredTiles using the _Condition
                foreach (GameObject tileObject in unfilteredTiles)
                {
                    if (_Condition(tileObject))
                        filteredTiles.Add(tileObject);
                }

                //Return the tiles
                return filteredTiles;
            }

            /// <summary>Will find all the tiles based on a given condition</summary>
            /// <param name="_Condition">The function that determines whether or not you want the tile</param>
            /// <returns>The list of filtered tiles</returns>
            static public List<Tile> FindFilteredTiles(GameObject[] _Tiles, Func<Tile, bool> _Condition)
            {
                //Init list for filtered game objects
                List<Tile> filteredTiles = new List<Tile>();

                //Get all the tiles
                GameObject[] unfilteredGameObjects = _Tiles;

                //Filter the unfilteredTiles using the _Condition
                foreach (GameObject tileObject in unfilteredGameObjects)
                {
                    Tile tile = tileObject.GetComponent<Tile>();
                    if (_Condition(tile))
                        filteredTiles.Add(tile);
                }

                //Return the tiles
                return filteredTiles;
            }

            /// <summary>Will find all the tiles based on a given condition</summary>
            /// <param name="_Condition">The function that determines whether or not you want the tile</param>
            /// <returns>The list of filtered tile gameobjects</returns>
            static public List<GameObject> FindFilteredTiles(Func<GameObject, bool> _Condition)
            {
                return FindFilteredTiles(GameObject.FindGameObjectsWithTag("Tile"), _Condition);
            }

            /// <summary>Will find all the tiles based on a given condition</summary>
            /// <param name="_Condition">The function that determines whether or not you want the tile</param>
            /// <returns>The list of filtered tiles</returns>
            static public List<Tile> FindFilteredTiles(Func<Tile, bool> _Condition)
            {
                return FindFilteredTiles(GameObject.FindGameObjectsWithTag("Tile"), _Condition);
            }
            #endregion
        }
    }
}
