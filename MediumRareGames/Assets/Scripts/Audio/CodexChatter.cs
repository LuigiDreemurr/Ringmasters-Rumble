/*
 -----------------------------------------------------------------------------
        Created By Brandon Vout
 -----------------------------------------------------------------------------
    CodexChatter.cs
        - Keeps track of the announcer's codex chatter.

    Details:
        - Set playlists for the playlist manager using clearer variable names
        - Directly choose each playlist
 -----------------------------------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlaylistManager))]
public class CodexChatter : MonoBehaviour
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

    [SerializeField] private PlaylistManager playlistManager;

    [SerializeField] private bool playOnce = true;  // Only play one clip when starting playlist

    [SerializeField] private AudioPlaylist tikiPlaylist;
    [SerializeField] private AudioPlaylist banditPlaylist;
    [SerializeField] private AudioPlaylist spacePlaylist;
    [SerializeField] private AudioPlaylist blockPlaylist;
    [SerializeField] private AudioPlaylist chickenPlaylist;

    [SerializeField] private AudioPlaylist gumballPlaylist;
    [SerializeField] private AudioPlaylist sharkPlaylist;
    [SerializeField] private AudioPlaylist wallPlaylist;

    #endregion  // Private

    #endregion  // Variables

    //---------------------------------------------------------------------------
    #region MonoBehaviour 

    /// <summary> Use this for initialization </summary>
    private void Start()
    {
        if (playlistManager != null)
            playlistManager.ClipFinished += Clip_Finished;
    }

    #endregion  // MonoBehaviour 

    //---------------------------------------------------------------------------
    #region Functions

    #region Public

    #region Characters

    public void Play_Tiki()
    {
        SetPlaylist(tikiPlaylist);
    }
    public void Play_Bandit()
    {
        SetPlaylist(banditPlaylist);
    }
    public void Play_Space()
    {
        SetPlaylist(spacePlaylist);
    }
    public void Play_Block()
    {
        SetPlaylist(blockPlaylist);
    }
    public void Play_Chicken()
    {
        SetPlaylist(chickenPlaylist);
    }

    #endregion  // Characters

    #region Weapons

    public void Play_Gumball()
    {
        SetPlaylist(gumballPlaylist);
    }
    public void Play_Shark()
    {
        SetPlaylist(sharkPlaylist);
    }
    public void Play_Wall()
    {
        SetPlaylist(wallPlaylist);
    }

    #endregion  // Weapons

    #endregion  // Public

    #region Private

    /// <param name="_ForcePlay"> Ignore playlist manager's inturrupt and chatter's speaking chance settings </param>
    private void SetPlaylist(AudioPlaylist _Playlist, bool _ForcePlay = false)
    {
        if (_ForcePlay)
            playlistManager.SetCurrentPlaylist(_Playlist, true);
    }

    #endregion  // Private

    #endregion  // Functions

    //---------------------------------------------------------------------------
    #region EventsAndHandlers

    #region EventHandlers



    #endregion  // Event Handlers

    #region Events

    private void Clip_Finished(object _Sender, EventArgs _Args)
    {
        if (playOnce)
            playlistManager.Btn_StopCurrentPlaylist();
    }

    #endregion  // Events

    #endregion  // Events & Handlers
}