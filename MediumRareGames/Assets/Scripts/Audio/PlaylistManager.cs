/*
 -----------------------------------------------------------------------------
        Created By Brandon Vout
 -----------------------------------------------------------------------------
    Playlist Manager
        - For managing multiple playlists and/or branching dialogue

    Details:
        - Used to decide which playlist is attached to a specific audio source
        - Triggers to control playlist changes
        - Has Clip Finished Event
 -----------------------------------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaylistManager : MonoBehaviour
{

    #region Classes



    #endregion  // Classes

    //---------------------------------------------------------------------------
    #region Variables

    #region Constants

    const int NO_PLAYLIST = -1;
    const int UNLISTED_PLAYLIST = -2;

    #endregion  // Constants

    #region Public



    #endregion  // Public

    #region Private

    [SerializeField] [Lockable] private AudioSource masterAudioSource;      // The source all playlists managed will use
    [SerializeField] [Lockable] private bool useMasterAudioSource = true;   // Disable to have each playlist use its own source
    [SerializeField] [Lockable] private AudioPlaylist[] playlists;          // Playlists being managed

    [SerializeField] [Lockable] private bool playFirstListAtStart = true;
    [SerializeField] [Lockable] private bool stopAllAudioSources = true;

    [SerializeField] [Lockable] private bool shuffle = false;

    [SerializeField] [Lockable] private bool interruptEnabled = true;   // If enabled, playing audio is cut off, else new audio waits or is dropped
    [SerializeField] [Lockable] private float interruptDelay = 0.2f;
    [SerializeField] [Lockable] private bool waitForFinished = false;   // If enabled, new audio will play after old audio finishes playing, only way to change playlists when inturrupt is disabled

    private bool interrupting = false;
    private float interruptTimer = 0;
    private AudioPlaylist interruptPlaylist;    // Stores new playlist for after interrupt

    private int currentPlaylistIndex = NO_PLAYLIST;
    private AudioPlaylist unlistedPlaylist;
    private AudioPlaylist volatilePlaylist;     // New master playlist scripts, are created and destroyed on gameObject

    [SerializeField] [Lockable] private int masterMinBuffer = 1;    // Buffers for when playling all playlists at once
    [SerializeField] [Lockable] private int masterMaxBuffer = 3;

    #endregion  // Private

    #endregion  // Variables

    //---------------------------------------------------------------------------
    #region MonoBehaviour

    /// <summary> Use this for initialization </summary>
    void Start()
    {
        if (useMasterAudioSource)
        {
            if (masterAudioSource == null)
                return;

            masterAudioSource.Stop();
            foreach (AudioPlaylist playlist in playlists)
            {
                playlist.SetSource(masterAudioSource);
                playlist.Shuffle = shuffle;
            }
        }
        else if (stopAllAudioSources)
        {
            foreach (AudioPlaylist playlist in playlists)
            {
                if (playlist.IsPlaying)
                    playlist.StopAudio();
                playlist.Shuffle = shuffle;
            }
        }

        if (playFirstListAtStart)
        {
            currentPlaylistIndex = 0;
            if (currentPlaylistIndex > NO_PLAYLIST && currentPlaylistIndex < playlists.Length && playlists.Length > 0)
            {
                SetCurrentPlaylistByIndex(currentPlaylistIndex);
                playlists[currentPlaylistIndex].PlayAudio();
            }
            else
                currentPlaylistIndex = NO_PLAYLIST;
        }
    }

    #endregion  // MonoBehaviour 

    //---------------------------------------------------------------------------
    #region Functions

    #region Public

    /// <summary> Destroy tempObject, handle case where it is holding the current playlist </summary>
    public void DestroyVolatilePlaylist()
    {
        if (volatilePlaylist != null)
        {
            if (unlistedPlaylist != null)
                if (unlistedPlaylist == volatilePlaylist && currentPlaylistIndex == UNLISTED_PLAYLIST)
                {
                    StopCurrentPlaylist();
                    currentPlaylistIndex = NO_PLAYLIST;
                    unlistedPlaylist = null;
                }
            Destroy(volatilePlaylist);
        }
    }

    public float GetInterruptDelay() { return interruptDelay; }
    public void SetInterruptDelay(float _Delay) { interruptDelay = _Delay; }
    public bool GetInterruptEnabled() { return interruptEnabled; }
    public void SetInterruptEnabled(bool _Enabled) { interruptEnabled = _Enabled; }
    public bool GetWaitForFinished() { return waitForFinished; }
    public void SetWaitForFinnished(bool _WaitForFinished) { waitForFinished = _WaitForFinished; }

    public void Btn_StopCurrentPlaylist() { StopCurrentPlaylist(true); }
    public bool StopCurrentPlaylist(bool _ForceStop = false)
    {
        bool currentPlaylistStopped = false;

        if (currentPlaylistIndex > NO_PLAYLIST)
        {
            if (!playlists[currentPlaylistIndex].IsSourcePlayling())
            {
                playlists[currentPlaylistIndex].StopAudio();
                currentPlaylistStopped = true;
            }
        }
        else if (currentPlaylistIndex == UNLISTED_PLAYLIST && !unlistedPlaylist.IsSourcePlayling())
        {
            unlistedPlaylist.StopAudio();
            currentPlaylistStopped = true;
        }

        if (currentPlaylistStopped)
        {
            if (currentPlaylistIndex == UNLISTED_PLAYLIST)
                unlistedPlaylist.ClipFinished -= Clip_Finished;
            else if (currentPlaylistIndex != NO_PLAYLIST)
                playlists[currentPlaylistIndex].ClipFinished -= Clip_Finished;
            currentPlaylistIndex = NO_PLAYLIST;
        }

        return currentPlaylistStopped;
    }

    public void SetMasterMinBuffer(int _MinBuffer) { masterMinBuffer = _MinBuffer; }
    public void SetMasterMaxBuffer(int _MaxBuffer) { masterMaxBuffer = _MaxBuffer; }

    public void Btn_PlayAllPlaylists()
    {
        CreateMasterPlaylist();
        SetPlaylist(unlistedPlaylist);
    }

    public void Btn_SetCurrentPlaylist(AudioPlaylist _Playlist) { SetCurrentPlaylist(_Playlist); }
    public void Btn_ForceSetCurrentPlaylist(AudioPlaylist _Playlist) { SetCurrentPlaylist(_Playlist, true); }
    /// <summary> Passes playlist class to current playlist. </summary>
    /// <param name="_Playlist"> AudioPlaylist Class </param>
    /// <param name="_ForcePlay"> Ignore Inturrupt rules if true </param>
    public void SetCurrentPlaylist(AudioPlaylist _Playlist, bool _ForcePlay = false)
    {
        DestroyVolatilePlaylist();
        SetPlaylist(_Playlist, _ForcePlay);
    }

    public void Btn_SetCurrentPlaylistByIndex(int _Index) { SetCurrentPlaylistByIndex(_Index); }
    public void Btn_ForceSetCurrentPlaylistByIndex(int _Index) { SetCurrentPlaylistByIndex(_Index, true); }
    /// <summary> Selects playlist from Index. </summary>
    /// <param name="_Index"></param>
    /// <param name="_ForcePlay"> Ignore Inturrupt rules if true </param>
    public void SetCurrentPlaylistByIndex(int _Index, bool _ForcePlay = false)
    {
        if (_Index > NO_PLAYLIST && _Index < playlists.Length)
        {
            DestroyVolatilePlaylist();
            SetPlaylist(playlists[_Index], _ForcePlay);
        }
    }

    #endregion  // Public

    #region Private

    private IEnumerator WaitForInterrupt()
    {
        while (interrupting)
        {
            interruptTimer += Time.fixedDeltaTime;
            if (interruptTimer >= interruptDelay)
            {
                SetPlaylist(interruptPlaylist);
                interruptPlaylist = null;
                interrupting = false;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary> Merge all playlists into unlistedPlaylist. </summary>
    private void CreateMasterPlaylist()
    {
        DestroyVolatilePlaylist();
        volatilePlaylist = gameObject.AddComponent<AudioPlaylist>();
        unlistedPlaylist = volatilePlaylist;
        unlistedPlaylist.SetSource(masterAudioSource);
        for (int i = 0; i < playlists.Length; i++)
            unlistedPlaylist.MergePlaylist(playlists[i]);
        unlistedPlaylist.ResetPlaylist();
        unlistedPlaylist.SetMinMaxBuffers(masterMinBuffer, masterMaxBuffer);
    }

    private void SetPlaylist(AudioPlaylist _Playlist, bool _ForcePlay = false)
    {
        if (_Playlist != null)
        {
            bool currentPlaylistStopped = StopCurrentPlaylist(_ForcePlay);

            if (_ForcePlay || ((interruptTimer >= interruptDelay && !waitForFinished) || currentPlaylistStopped) && interruptEnabled)
            {
                interrupting = false;

                _Playlist.ClipFinished += Clip_Finished;
                _Playlist.SetSourceClip();
                _Playlist.PlayAudio();

                currentPlaylistIndex = UNLISTED_PLAYLIST;
                for (int i = 0; i < playlists.Length; i++)  // Check if playlist is in playlist array
                {
                    if (playlists[i] == _Playlist)
                    {
                        currentPlaylistIndex = i;
                        break;
                    }
                }
                if (currentPlaylistIndex == UNLISTED_PLAYLIST && _Playlist != unlistedPlaylist)
                    unlistedPlaylist = _Playlist;
            }
            else if (interruptEnabled && !waitForFinished)
            {
                interruptPlaylist = _Playlist;
                interrupting = true;
                interruptTimer = 0;
                StartCoroutine(WaitForInterrupt());
            }
        }
    }

    #endregion  // Private

    #endregion  // Functions

    //---------------------------------------------------------------------------
    #region EventsAndHandlers

    #region EventHandlers

    public EventHandler ClipFinished;

    #endregion  // Event Handlers

    #region Events

    private void Clip_Finished(object _Sender, EventArgs _Args)
    {
        AudioPlaylist _Playlist = (AudioPlaylist)_Sender;

        if (_Playlist.ClipFinished != null && interruptPlaylist != null && waitForFinished)
        {
            SetPlaylist(interruptPlaylist);
        }
        ClipFinished?.Invoke(this, EventArgs.Empty);
    }

    #endregion  // Events

    #endregion  // Events & Handlers
}