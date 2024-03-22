/*
 -----------------------------------------------------------------------------
        Created By Brandon Vout
 -----------------------------------------------------------------------------
    SettingsCanvasController
        - Currently for setting the canvas controllers (sliders, toggles, etc.) to loaded values

    Details:
        - 
 -----------------------------------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsCanvasController : MonoBehaviour
{

    #region Classes



    #endregion  // Classes

    //---------------------------------------------------------------------------
    #region Variables

    #region Constants



    #endregion  // Constants

    #region Public



    #endregion  // Public

    #region Private

    [SerializeField] [Lockable] private SystemSettingsController sysSettings;

    [SerializeField] [Lockable] private Slider volumeSlider;
    [SerializeField] [Lockable] private Toggle muteToggle;
    [SerializeField] [Lockable] private Slider announcerVolumeSlider;
    [SerializeField] [Lockable] private Toggle announcerMuteToggle;
    [SerializeField] [Lockable] private Dropdown resDropdown;
    [SerializeField] [Lockable] private Toggle fullscreenToggle;

    #endregion  // Private

    #endregion  // Variables

    //---------------------------------------------------------------------------
    #region MonoBehaviour 

    /// <summary> Use this for initialization </summary>
    IEnumerator Start()
    {
        // wait for settings to load
        while (sysSettings.Settings == null)
            yield return null;
        
        announcerVolumeSlider.value = sysSettings.Settings.announcerVolume;
        announcerMuteToggle.isOn = sysSettings.Settings.announcerMute;
        fullscreenToggle.isOn = sysSettings.Settings.fullScreen;
        for (int i = 0; i < resDropdown.options.Count; i++)
        {
            if (resDropdown.options[i].text == sysSettings.Settings.width + "x" + sysSettings.Settings.height)
            {
                resDropdown.value = i;
                break;
            }
        }
    }

    #endregion  // MonoBehaviour 

    //---------------------------------------------------------------------------
    #region Functions

    #region Public



    #endregion  // Public

    #region Private



    #endregion  // Private

    #endregion  // Functions

    //---------------------------------------------------------------------------
    #region EventsAndHandlers

    #region EventHandlers



    #endregion  // Event Handlers

    #region Events



    #endregion  // Events

    #endregion  // Events & Handlers
}