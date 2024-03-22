/*
 -----------------------------------------------------------------------------
        Created By Brandon Vout
 -----------------------------------------------------------------------------
    SystemSettings
        - Holds system settings (volume, resolution, etc.)

    Details:
        - Stores System Settings Data
        - Does not allow volume float to be outside of MAX and MIN range
 -----------------------------------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemSettings
{

    #region Variables

    #region Constants
    
    public const float MIN_VOLUME = 0.0f;
    public const float MAX_VOLUME = 1.0f;

    #endregion  // Constants

    #region Public
    
    public float announcerVolume;
    public bool announcerMute;

    public float aspectRatio;
    public int width;
    public int height;
    public bool fullScreen;

    #endregion  // Public

    #region private
    
    //private float m_volume;

    #endregion  // Private

    #endregion  // Variables

    //---------------------------------------------------------------------------

    #region ConstructorsAndDestructors

    #region Constructors

    public SystemSettings()
    {
        announcerVolume = 1.0f;
        announcerMute = false;

        width = 1920;
        height = 1080;
        aspectRatio = (float)width / height;
        fullScreen = true;
    }

    #endregion  // Constructors

    #region Destructors



    #endregion  // Destructors

    #endregion  // ConstructorsAndDestructors

    //---------------------------------------------------------------------------

    #region Functions

    #region Public



    #endregion  // Public

    #region private



    #endregion  // Private

    #endregion  // Functions
}
