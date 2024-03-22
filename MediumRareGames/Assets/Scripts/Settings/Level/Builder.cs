/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   Settings.Builder
       - The settings accessed by level building tools

   Details:
       - Keeps things consistent accross all tools because there is only one
         data file used at a time
       - Can easily modify values and see them immediately take affect
       - Could possibly set it up so we can easily swap in and out different
         data instances
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    namespace Level
    {
        [CreateAssetMenu(fileName ="LevelBuilderSettings", menuName = Builder.Directory)]
        public class Builder : Settings.Asset
        {
            private new const string Directory = Asset.Directory + "Level/Builder";

            #region Singleton Setup
            static private Builder m_instance = null;
            static public Builder Get
            {
                get
                {
                    //Find instance in resources if there is not one
                    if (m_instance == null)
                        m_instance = Resources.Load<Builder>("Settings/Level/BuilderSettings");// Resources.FindObjectsOfTypeAll<Builder>()[0];

                    //Return the builder settings
                    return m_instance;
                }
            }
            #endregion

            #region Classes
            /// <summary>A simple class to group an elevation with a material</summary>
            [System.Serializable]
            public class ElevationColor
            {
                public ElevationColor(int _Elevation, Material _Material)
                {
                    elevation = _Elevation;
                    material = _Material;
                }

                public int elevation;
                public Material material;
            }
            #endregion

            [SerializeField] private GameObject m_tilePrefab;
            [SerializeField] private int m_elevationY = 3; //The distance between a single elevation change
            [SerializeField] private int m_minElevation = -1;
            [SerializeField] private int m_maxElevation = 1;
            [SerializeField] private int m_expandToDepth = -5; //To watch depth the tiles will stretch down
            //[SerializeField] private List<ElevationColor> m_elevationColors; //Elevations and their materials
            [SerializeField] private List<Material> m_tileMaterials; //Available materials for tiles

            public GameObject TilePrefab { get { return m_tilePrefab; } }
            public int ElevationY { get { return m_elevationY; } }
            public int MinElevation { get { return m_minElevation; } }
            public int MaxElevation { get { return m_maxElevation; } }
            public int ExpandToDepth { get { return m_expandToDepth; } }
            //public List<ElevationColor> ElevationColors { get { return m_elevationColors; } }
            public Vector3 TileSize { get { return new Vector3(m_tilePrefab.transform.localScale.x, m_elevationY, m_tilePrefab.transform.localScale.z); } }
            public List<Material> TileMaterials { get { return m_tileMaterials; } }

            #region Public Methods
            ///// <summary>Gets the material associated with the elevation passed</summary>
            ///// <param name="_Elevation">What elevation</param>
            ///// <returns>The material for an elevation</returns>
            //public Material GetMaterial(int _Elevation)
            //{
            //    if (_Elevation < m_minElevation || _Elevation > m_maxElevation)
            //        Debug.LogError("_Elevation not in range");

            //    return m_elevationColors[_Elevation + (m_minElevation * -1)].material;
            //}
            #endregion

            #region Unity Messages
            /// <summary>When the object is enabled</summary>
            private void OnEnable()
            {
                //Make sure things are updated
                OnValidate();
            }

            /// <summary>When the inspector is changed/whatever else</summary>
            private void OnValidate()
            {
                //UpdateElevationColors();
                ClampDepth();
            }
            #endregion

            #region Helper Methods
            /// <summary>Makes it so that the expandToDepth will never be above minElevation</summary>
            private void ClampDepth()
            {
                if (m_expandToDepth >= m_minElevation)
                    m_expandToDepth = m_minElevation - 1;
            }

            ///// <summary>Resizes the elevationColors list while keeping data that is still relevant</summary>
            //private void UpdateElevationColors()
            //{
            //    //Get copy of the previous elevations/colors before overriding it
            //    List<ElevationColor> prev = m_elevationColors;

            //    //Allocate right amount of memory before adding
            //    m_elevationColors = new List<ElevationColor>(Mathf.Abs(m_minElevation) + Mathf.Abs(m_maxElevation) + 1);

            //    //Add all elevations in order
            //    for (int i = m_minElevation; i <= m_maxElevation; i++)
            //    {
            //        m_elevationColors.Add(new ElevationColor(i, null));
            //    }

            //    //For every elevation/color in the previous list check if that elevation is in the new range
            //    foreach (ElevationColor prevEc in prev)
            //    {
            //        foreach (ElevationColor currentEc in m_elevationColors)
            //        {
            //            //If the elevation is still in range copy over the previous color
            //            if (currentEc.elevation == prevEc.elevation)
            //                currentEc.material = prevEc.material;
            //        }
            //    }
            //}
            #endregion
        }
    }
}
