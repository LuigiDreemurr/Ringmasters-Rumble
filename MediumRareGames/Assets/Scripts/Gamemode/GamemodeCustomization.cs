/*
 -----------------------------------------------------------------------------
        Created By Brandon Vout
 -----------------------------------------------------------------------------
    GamemodeCustomization.cs
        - Handles customization options for the different game modes

    Details:
        - 
 -----------------------------------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GamemodeCustomization : MonoBehaviour
{
    #region Variables

    #region Constants



    #endregion

    #region Public



    #endregion

    #region Private

    [SerializeField] bool m_loadDefaultsOnStart = false;    // For debugging, reset values to defaults

    [SerializeField] private Settings.Match m_match;
    [SerializeField] private Settings.GeneralWeapon m_weapon;
    [SerializeField] private Settings.LastManStanding m_lms;
    [SerializeField] private Settings.KingOfTheHill m_koth;

    [SerializeField] private Settings.DefaultSettings m_default;

    #region DATA

    bool m_matchChanged;
    bool m_weaponChanged;
    bool m_lmsChanged;
    bool m_kothChanged;

    private int m_matchRoundsToWin;

    private Weapon.Type m_weaponDefaultWeapon;

    private int m_lmsLives;
    private float m_lmsRespawnTime;
    private float m_lmsTimeLimit;

    private int m_kothPointsToCapture;
    private int m_kothPossiblePoints;
    private float m_kothRespawnTime;
    private float m_kothCountdownTime;

    #endregion  // DATA

    #endregion

    #endregion

    //---------------------------------------------------------------------------
    #region Monobehaviour



    #endregion

    //---------------------------------------------------------------------------
    #region Functions

    #region Public

    /// <summary> Gamemode Customization Initialization (use instead of Start()) </summary>
    public void Initialize()
    {
        LoadSettings();
        if (m_loadDefaultsOnStart)
            SetToDefaultSettings();

        Initialize_Finished();
    }

    #region Apply

    /// <summary> Add settings to setting Assets. </summary>
    public void ApplySettings()
    {
        ApplyMatch();
        ApplyWeapon();
        ApplyLMS();
        ApplyKotH();
    }

    /// <summary> Add Match settings to Match Asset. Save to file if told to. </summary>
    public void ApplyMatch()
    {
        if (m_matchChanged)
        {
            m_matchChanged = false;
            SetMatch();
        }
    }

    /// <summary> Add Weapon settings to Gameplay Asset. Save to file if told to. </summary>
    public void ApplyWeapon()
    {
        if (m_weaponChanged)
        {
            m_weaponChanged = false;
            SetWeapon();
        }
    }

    /// <summary> Add LMS settings to LMS Asset. Save to file if told to. </summary>
    public void ApplyLMS()
    {
        if (m_lmsChanged)
        {
            m_lmsChanged = false;
            SetLMS();
        }
    }

    /// <summary> Add KotH settings to KotH Asset. Save to file if told to. </summary>
    public void ApplyKotH()
    {
        if (m_kothChanged)
        {
            m_kothChanged = false;
            SetKotH();
        }
    }

    #endregion  // Apply

    #region Set

    public void SetToDefaultSettings()
    {
        m_match.SetRounds(m_default.RoundsToWin);

        m_weapon.SetWeapon(m_default.DefaultWeapon);

        m_lms.SetLives(m_default.Lives);
        m_lms.SetRespawnTime(m_default.LMSRespawnTime);
        m_lms.SetTimeLimit(m_default.TimeLimit);

        m_koth.SetPointsCapture(m_default.PointsToCapture);
        m_koth.SetPossiblePoints(m_default.PossiblePoints);
        m_koth.SetRespawn(m_default.KotHRespawnTime);
        m_koth.SetCountdown(m_default.CountdownTime);
    }

    public void SetMatchRounds(Text _Label)
    {
        m_matchRoundsToWin = int.Parse(_Label.text);
        if (m_matchRoundsToWin != m_match.RoundsToWin)
            m_matchChanged = true;
    }

    public void SetDefaultWeapon(int _WeaponID)
    {
        m_weaponDefaultWeapon = (Weapon.Type)_WeaponID;
        if (m_weaponDefaultWeapon != m_weapon.DefaultWeapon)
            m_weaponChanged = true;
    }

    public void SetLives(Text _Label)
    {
        m_lmsLives = int.Parse(_Label.text);
        if (m_lmsLives != m_lms.Lives)
            m_lmsChanged = true;
    }

    public void SetLMSRespawnTime(Text _Label)
    {
        m_lmsRespawnTime = float.Parse(_Label.text);
        if (m_lmsRespawnTime != m_lms.RespawnTime)
            m_lmsChanged = true;
    }

    public void SetTimeLimit(Text _Label)
    {
        m_lmsTimeLimit = float.Parse(_Label.text);
        if (m_lmsTimeLimit != m_lms.TimeLimit)
            m_lmsChanged = true;
    }

    public void SetPointsToCapture(Text _Label)
    {
        m_kothPointsToCapture = int.Parse(_Label.text);
        if (m_kothPointsToCapture != m_koth.PointsToCapture)
            m_kothChanged = true;
    }

    public void SetPossiblePoints(Text _Label)
    {
        m_kothPossiblePoints = int.Parse(_Label.text);
        if (m_kothPossiblePoints != m_koth.PossiblePoints)
            m_kothChanged = true;
    }

    public void SetKotHRespawnTime(Text _Label)
    {
        m_kothRespawnTime = float.Parse(_Label.text);
        if (m_kothRespawnTime != m_koth.RespawnTime)
            m_kothChanged = true;
    }

    public void SetCountdown(Text _Label)
    {
        m_kothCountdownTime = float.Parse(_Label.text);
        if (m_kothCountdownTime != m_koth.CountdownTime)
            m_kothChanged = true;
    }

    #endregion  // Set

    #endregion

    #region Private

    /// <summary> Load default settings from Asset. </summary>
    private void LoadDefaultSettings()
    {
        m_matchRoundsToWin = m_default.RoundsToWin;

        m_weaponDefaultWeapon = m_default.DefaultWeapon;

        m_lmsLives = m_default.Lives;
        m_lmsRespawnTime = m_default.LMSRespawnTime;
        m_lmsTimeLimit = m_default.TimeLimit;

        m_kothPointsToCapture = m_default.PointsToCapture;
        m_kothPossiblePoints = m_default.PossiblePoints;
        m_kothRespawnTime = m_default.KotHRespawnTime;
        m_kothCountdownTime = m_default.CountdownTime;
    }

    private void LoadSettings()
    {
        m_matchRoundsToWin = m_match.RoundsToWin;

        m_weaponDefaultWeapon = m_weapon.DefaultWeapon;

        m_lmsLives = m_lms.Lives;
        m_lmsRespawnTime = m_lms.RespawnTime;
        m_lmsTimeLimit = m_lms.TimeLimit;

        m_kothPointsToCapture = m_koth.PointsToCapture;
        m_kothPossiblePoints = m_koth.PossiblePoints;
        m_kothRespawnTime = m_koth.RespawnTime;
        m_kothCountdownTime = m_koth.CountdownTime;
    }

    /// <summary> Return .json file text. </summary>
    private string GetJson(string _Path)
    {
        // Load settings file if it exists
        if (File.Exists(_Path))
        {
            string json = File.ReadAllText(_Path);
            return json;
        }
        else
            return "";
    }

    /// <summary> Save setting Asset values to .json files. </summary>
    private void SaveSettings(string _Path, string _Json)
    {
        // Don't save blank file
        if (_Json == "")
            return;

        // Existing settings file is deleted if it exists
        if (File.Exists(_Path))
            File.Delete(_Path);

        File.WriteAllText(_Path, _Json);
    }

    #region Set

    private void SetMatch()
    {
        m_match.SetRounds(m_matchRoundsToWin);

        Match_Updated();
    }

    private void SetWeapon()
    {
        m_weapon.SetWeapon(m_weaponDefaultWeapon);

        Weapon_Updated();
    }

    private void SetLMS()
    {
        m_lms.SetLives(m_lmsLives);
        m_lms.SetRespawnTime(m_lmsRespawnTime);
        m_lms.SetTimeLimit(m_lmsTimeLimit);

        LMS_Updated();
    }

    private void SetKotH()
    {
        m_koth.SetPointsCapture(m_kothPointsToCapture);
        m_koth.SetPossiblePoints(m_kothPossiblePoints);
        m_koth.SetRespawn(m_kothRespawnTime);
        m_koth.SetCountdown(m_kothCountdownTime);

        KotH_Updated();
    }

    #endregion  // Set

    #endregion

    #endregion  // Functions

    //---------------------------------------------------------------------------
    #region EventsAndHandlers

    #region EventHandlers

    public EventHandler Initialized;

    public EventHandler MatchUpdated;
    public EventHandler GameplayUpdated;
    public EventHandler LMSUpdated;
    public EventHandler KotHUpdated;

    #endregion  // Event Handlers

    #region Events

    private void Initialize_Finished() { Initialized?.Invoke(this, EventArgs.Empty); }

    private void Match_Updated()
    {
        MatchUpdated?.Invoke(this, EventArgs.Empty);
    }

    private void Weapon_Updated()
    {
        GameplayUpdated?.Invoke(this, EventArgs.Empty);
    }

    private void LMS_Updated()
    {
        LMSUpdated?.Invoke(this, EventArgs.Empty);
    }

    private void KotH_Updated()
    {
        KotHUpdated?.Invoke(this, EventArgs.Empty);
    }

    #endregion  // Events

    #endregion  // Events & Handlers
}
