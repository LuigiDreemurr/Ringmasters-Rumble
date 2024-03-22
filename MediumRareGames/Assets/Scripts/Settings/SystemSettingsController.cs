/*
 -----------------------------------------------------------------------------
        Created By Brandon Vout
 -----------------------------------------------------------------------------
    SystemSettingsController
        - Manages System Settings

    Details:
        - Change Volume
        - Toggle Mute
        - Toggle Fullscreen
        - Change Resolution
        - Automatically adjusts aspect ratio
        - Saves settings to .json file in Persistent storage
        - Loads and applies data on Start if it exists
 -----------------------------------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class SystemSettingsController : MonoBehaviour
{

    #region Classes



    #endregion  // Classes

    //---------------------------------------------------------------------------
    #region Variables

    #region Constants



    #endregion  // Constants

    #region Public
        
    public SystemSettings Settings
    {
        get { return settings; }
    }

    #endregion  // Public

    #region Private

    [Tooltip("Disable to manually set save function to OnClick event.")]
    [SerializeField] [Lockable] private bool autoSaveOnApply = false;
    [Tooltip("Stored settings file name in Persistent data storage.")]
    [SerializeField] [Lockable] private string settingsPath = "settings.json";

    [SerializeField] [Lockable] private Camera mainCamera;
    [SerializeField] [Lockable] private AudioSource announcerSource;
    //[SerializeField][Lockable] private MenuController menuController;

    private SystemSettings settings;
    private SystemSettings newSettings;


    #endregion  // Private

    #endregion  // Variables

    //---------------------------------------------------------------------------
    #region MonoBehaviour 

    private IEnumerator Start()
    {
        // Correct settings file is loaded on start
        Btn_LoadSettings();

        // wait for settings to load
        while (settings == null)
            yield return null;

        newSettings = settings;

        Btn_Apply();    // Apply loaded settings
    }

    #endregion  // MonoBehaviour 

    //---------------------------------------------------------------------------
    #region Functions

    #region Public

    #region ChangeSettings

    public void Stg_ChangeAnnouncerVolume(float _Volume)
    {
        newSettings.announcerVolume = _Volume;
    }

    public void Stg_ChangeAnnouncerMute(bool _Mute)
    {
        newSettings.announcerMute = _Mute;
    }

    /// <summary> Set new width and height via string (format 1920x1080) </summary>
    /// <param name="_NewRes"> Label child of dropdown UI object </param>
    public void Stg_ChangeResolution(Text _NewRes)
    {
        string newRes = _NewRes.text;
        string[] splitArray = newRes.Split(char.Parse("x"));
        newSettings.width = int.Parse(splitArray[0]);
        newSettings.height = int.Parse(splitArray[1]);
    }

    public void Stg_ChangeFullscreen(bool _Fullscreen)
    {
        newSettings.fullScreen = _Fullscreen;
    }

    public IEnumerator SetResolution()
    {
        Screen.SetResolution(settings.width, settings.height, settings.fullScreen);
        StartCoroutine(UpdateAspectRatio());

        yield return new WaitForEndOfFrame();
    }

    #endregion  // ChangeSettings

    /// <summary> Confirm setting changes, handle closing menu using MenuController </summary>
    public void Btn_Apply()
    {
        settings = newSettings;
        
        if (announcerSource != null)
        { 
            announcerSource.volume = settings.announcerVolume;
            announcerSource.mute = settings.announcerMute;
        }
        StartCoroutine(SetResolution());

        if (autoSaveOnApply)
            Btn_SaveSettings();
    }

    /// <summary> Revert settings, handle closing menu using MenuController </summary>
    public void Btn_Cancel()
    {
        newSettings = settings;
    }

    #region SaveAndLoadSettings

    /// <summary>
    /// Save settings class to settings.json AND create file if it did not exist prior
    /// </summary>
    public void Btn_SaveSettings()
    {
        // Settings Model’s properties are serialized to the file in the correct format
        string json = JsonUtility.ToJson(newSettings);
        string path = Path.Combine(Application.persistentDataPath, settingsPath);

        // Existing settings file is deleted if it exists
        if (File.Exists(settingsPath))
            File.Delete(settingsPath);

        File.WriteAllText(path, json);
    }

    public void Btn_LoadSettings()
    {
        StartCoroutine(LoadSettings());
    }

    /// <summary>
    /// Load settings.json OR construct default class if the former does not exist
    /// </summary>
    /// <returns></returns>
    public IEnumerator LoadSettings()
    {
        SystemSettings loadSettings = new SystemSettings
        {
            // Get default resolution from Unity Launcher
            width = Screen.width,
            height = Screen.height,
            fullScreen = Screen.fullScreen
        };

        // Load settings.json if it exists
        string path = Path.Combine(Application.persistentDataPath, settingsPath);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            loadSettings = JsonUtility.FromJson<SystemSettings>(json);
        }

        settings = loadSettings;
        yield return settings;
    }

    #endregion  // SaveAndLoadSettings

    #endregion  // Public

    #region Private

    /// <summary>
    /// Keep aspect ratio = targetAspectRatio
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateAspectRatio()
    {
        if (mainCamera != null)
        {
            //while (true)
            //{
            float currentAspectRatio = (float)Screen.width / Screen.height;
            float scaleHeight = currentAspectRatio / settings.aspectRatio;

            Rect rect = mainCamera.rect;

            // If the scale height is less than 1, we need to 'draw' letterbox black bars:
            if (scaleHeight < 1f)
            {
                rect.width = 1f;
                rect.height = scaleHeight;
                rect.x = 0;
                rect.y = (1f - scaleHeight) / 2f;
            }
            else    // Otherwise... pillarbox:
            {
                float scaleWidth = 1f / scaleHeight;
                rect.width = scaleWidth;
                rect.height = 1f;
                rect.x = (1 - scaleWidth) / 2f;
                rect.y = 0;
            }
            mainCamera.rect = rect;

            yield return new WaitForEndOfFrame();
            //}
        }
    }

    #endregion  // Private

    #endregion  // Functions

    //---------------------------------------------------------------------------
    #region EventsAndHandlers

    #region EventHandlers

    /// <summary> Volume has changed. </summary>
    public event EventHandler VolumeChanged;

    /// <summary> Music off/on has changed. </summary>
    public event EventHandler MuteOnOffChanged;

    #endregion  // Event Handlers

    #region Events



    #endregion  // Events

    #endregion  // Events & Handlers
}