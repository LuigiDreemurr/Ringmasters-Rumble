using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Level
{
    public static class Utility
    {
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
    }
}
