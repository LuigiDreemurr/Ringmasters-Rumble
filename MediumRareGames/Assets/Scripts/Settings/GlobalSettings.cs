/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   GlobalSettings
       - The script in the scene that contains references to all settings assets

   Details:
       - Currently only holds references to different weapons settings
       - Follows the single pattern (only one GlobalSettings can exist at a time)
-----------------------------------------------------------------------------
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    public static float ScaleSpeed(float _Speed)
    {
        return m_instance.Builder.TileSize.x;
    }

    #region Singleton Pattern
    static private GlobalSettings m_instance; //Static reference to a GlobalSettings instance

    /// <summary>Gets the instance of GlobalSettings</summary>
    static public GlobalSettings Get { get { return m_instance; } }

    /// <summary>Called once, before Start</summary>
    private void Awake()
    {
        //There is no existing GlobalSettings, use this one
        if (m_instance == null)
        {
            m_instance = this;
        }
        //There is already a GlobalSettings, destroy this
        else
        {
            Debug.LogError("Second instance of GlobalSettings, destroying gameObject");
            Destroy(gameObject);
        }

    }
    #endregion

    #region Helper Classes
    /// <summary>Simple wrapper class to group the different settings assets relevant to Weapons</summary>
    [Serializable] public class WeaponSettings
    {
        #region Data Members
        //Other settings
        [SerializeField] private Settings.GeneralWeapon m_generalSettings;
        [SerializeField] private Settings.WeaponPickup m_pickupSettings;

        //Specific weapon settings
        [SerializeField] private Settings.GumballLauncher m_gumballLauncherSettings;
        [SerializeField] private Settings.SharkZooka m_sharkZookaSettings;
        [SerializeField] private Settings.WallGun m_wallGunSettings;
        #endregion

        #region Properties
        public Settings.GeneralWeapon General { get { return m_generalSettings; } }
        public Settings.WeaponPickup Pickup { get { return m_pickupSettings; } }
        public Settings.GumballLauncher GumballLauncher { get { return m_gumballLauncherSettings; } }
        public Settings.SharkZooka SharkZooka { get { return m_sharkZookaSettings; } }
        public Settings.WallGun WallGun { get { return m_wallGunSettings; } }
        #endregion
    }

    /// <summary>Simple wrapper class to group the different settings assets relevant to gamemodes</summary>
    [Serializable] public class GamemodeSettings
    {
        #region Data Members
        [SerializeField] private Settings.LastManStanding m_lmsSettings;
        [SerializeField] private Settings.KingOfTheHill m_kothSettings;
        #endregion

        #region Properties
        public Settings.LastManStanding LastManStanding { get { return m_lmsSettings; } }
        public Settings.KingOfTheHill KingOfTheHill { get { return m_kothSettings; } }
        #endregion
    }
    #endregion

    #region Data Members
    [SerializeField] private WeaponSettings m_weaponSettings;
    [SerializeField] private GamemodeSettings m_gamemodeSettings;
    [SerializeField] private Settings.Player m_playerSettings;
    [SerializeField] private Settings.Level.Builder m_builderSettings;
    [SerializeField] private Settings.Match m_matchSettings;
    #endregion

    #region Properties
    public WeaponSettings Weapon { get { return m_weaponSettings; } }
    public GamemodeSettings Gamemode { get { return m_gamemodeSettings; } }
    public Settings.Player Player { get { return m_playerSettings; } }
    public Settings.Level.Builder Builder { get { return m_builderSettings; } }
    public Settings.Match Match { get { return m_matchSettings; } }
    #endregion
}
