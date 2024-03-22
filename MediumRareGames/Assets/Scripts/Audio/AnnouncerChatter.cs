/*
 -----------------------------------------------------------------------------
        Created By Brandon Vout
 -----------------------------------------------------------------------------
    AnnouncerChatter.cs
        - Keeps track of the announcer's chatter through events.

    Details:
        - Set playlists for the playlist manager using clearer variable names
        - Directly choose each playlist
        - Has Intro, Outro, and Clip Finished Events
 -----------------------------------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlaylistManager))]
public class AnnouncerChatter : MonoBehaviour
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

    [SerializeField] private GamemodeController m_gamemodeController;
    [SerializeField] private PlaylistManager m_playlistManager;

    [SerializeField] private bool m_playOnce = true;    // Only play one clip when starting playlist
    [Space]

    [SerializeField]
    private AudioPlaylist m_introPlaylist;
    [SerializeField] private AudioPlaylist m_introLongShortRoundsPlaylist;  // Too many long/Short rounds
    [SerializeField] private AudioPlaylist m_introFallOffPlaylist;  // Too many rounds where all but one player falls off
    [SerializeField] private AudioPlaylist m_introLMSPlaylist;  // Unique to LMS
    [SerializeField] private AudioPlaylist m_introKOTHPlaylist; // Unique to KotH
    [Space]

    [SerializeField]
    private AudioPlaylist m_outroPlaylist;
    [SerializeField] private AudioPlaylist m_outroLongRoundPlaylist;
    [SerializeField] private AudioPlaylist m_outroShortRoundPlaylist;
    [SerializeField] private AudioPlaylist m_outroFallOffPlaylist;  // All but one player fell off
    [SerializeField] private AudioPlaylist m_outroAllDeadPlaylist;  // Everyone dies
    [SerializeField] private AudioPlaylist m_outroFinalRoundPlaylist;
    [Space]

    [SerializeField]
    private AudioPlaylist m_hurtPlaylist;
    [SerializeField] private AudioPlaylist m_deadPlaylist;
    [SerializeField] private AudioPlaylist m_respawnPlaylist;
    [Space]

    [SerializeField]
    private AudioPlaylist m_pointCapturedPlaylist;
    [Space]

    [SerializeField]
    private AudioPlaylist m_weaponDropPlaylist;
    [Space]

    [SerializeField]
    private AudioPlaylist m_pickupPlaylist;
    [SerializeField] private AudioPlaylist m_pickupGumballPlaylist;
    [SerializeField] private AudioPlaylist m_pickupSharkPlaylist;
    [SerializeField] private AudioPlaylist m_pickupWallPlaylist;
    [Space]

    [SerializeField]
    private AudioPlaylist m_timeLowPlaylist;
    [Space]

    [SerializeField]
    private AudioPlaylist m_randomPlaylist;    // Plays if game goes too long without commentary
    [Space]

    [Tooltip("Minimum time announcer pauses between dialogue. Actual pause is randomized at end of Clip.")]
    [SerializeField]
    private float m_minPauseTimer = 1.0f;   // How long the announcer pauses after he stops talking (avoid constant noise)
    [Tooltip("Maximum time announcer pauses between dialogue. Actual pause is randomized at end of Clip.")]
    [SerializeField]
    private float m_maxPauseTimer = 2.0f;   // How long the announcer pauses after he stops talking (avoid constant noise)
    [Space]

    [Tooltip("Minimum time announcer goes without speaking. Actual silence is randomized at end of Clip. Starts counting after pause.")]
    [SerializeField]
    private float m_minSilenceTimer = 8.0f;     // How long the announcer goes without speaking
    [Tooltip("Maximum time announcer goes without speaking. Actual silence is randomized at end of Clip. Starts counting after pause.")]
    [SerializeField]
    private float m_maxSilenceTimer = 12.0f;    // How long the announcer goes without speaking
    [Space]

    [Tooltip("Chance of dialogue being spoken when function called. Percentage value between 0 & 100.")]
    [SerializeField]
    private int m_speakingChance = 30;  // Chance of dialogue being spoken (e.g. function called, 30% chance of clip being played)

    private bool m_playingIntro = false;
    private bool m_playingOutro = false;
    private bool m_pausing = false;
    private bool m_silence = true;

    #endregion  // Private

    #endregion  // Variables

    //---------------------------------------------------------------------------
    #region MonoBehaviour 

    /// <summary> Use this for initialization </summary>
    private void Awake()
    {
        if (m_minPauseTimer < 0)
            m_minPauseTimer = 0;
        if (m_maxPauseTimer < m_minPauseTimer)
            m_maxPauseTimer = m_minPauseTimer + 1;

        if (m_speakingChance < 0)
            m_speakingChance = 0;
        else if (m_speakingChance > 100)
            m_speakingChance = 100;
    }

    /// <summary> Use this for initialization </summary>
    private void Start()
    {
        if (m_playlistManager != null)
            m_playlistManager.ClipFinished += Clip_Finished;
        Play_Intro();
    }

    #endregion  // MonoBehaviour 

    //---------------------------------------------------------------------------
    #region Functions

    #region Public

    public void Play_Intro()
    {
        if (MatchHandler.FallOffRounds > 2) // Too many rounds where everyone fell off
        {
            MatchHandler.ResetFall();   // Reset fall counters
            m_playingIntro = true;
            Play_Intro_FallOff();
        }
        else if (MatchHandler.LongRounds > 3 && !m_playingIntro)    // Too many long/short rounds
        {
            MatchHandler.ResetLongRounds(); // Reset long/short round counter
            m_playingIntro = true;
            Play_Intro_LongShortRounds();
        }
        else    // If no conditions are met, play Generic/KotH/LMS Intro
        {
            m_playingIntro = true;
            System.Random rnd = new System.Random();// Generate seed (System.Random is more random than UnityEngine.Random)
            int r = rnd.Next(100);                  // Get random non-negative int less than 100
            if (r < 50)                             // Play audio if random number < 50
                Play_Intro_Generic();
            else if (m_gamemodeController.GamemodeType == Gamemode.Type.LMS)
                Play_Intro_LMS();
            else if (m_gamemodeController.GamemodeType == Gamemode.Type.KotH)
                Play_Intro_KotH();
        }
    }

    public void Play_Outro()
    {
        // Check if it is final round
        foreach (PlayerInfo info in LobbyMenu.Players)
        {
            if (MatchHandler.WinTracker[info.Name] == MatchHandler.WinsNeeded)
            {
                m_playingOutro = true;
                Play_Outro_FinalRound();
                break;
            }
        }

        if (!m_playingOutro)
        {
            m_playingOutro = true;

            // Everyone died
            if (MatchHandler.DeathCount == LobbyMenu.Players.Count && m_gamemodeController.gamemodeType == Gamemode.Type.LMS)
                Play_Outro_AllDead();
            else if (MatchHandler.FallCheck())       // Everyone fell off map
                Play_Outro_FallOff();
            else if (MatchHandler.ShortCheck()) // If current round was short
                Play_Outro_ShortRound();
            else if (MatchHandler.LongCheck())  // If current round was long
                Play_Outro_LongRound();
            else    // If no conditions are met, play Generic Outro
                Play_Outro_Generic();
        }
    }

    public void Play_Player_Hurt() { SetPlaylist(m_hurtPlaylist); }
    public void Play_Player_Dead() { SetPlaylist(m_deadPlaylist); }
    public void Play_Player_Respawn() { SetPlaylist(m_respawnPlaylist); }

    public void Play_PointCaptured() { SetPlaylist(m_pointCapturedPlaylist); }

    public void Play_WeaponDrop() { SetPlaylist(m_weaponDropPlaylist); }

    public void Play_Pickup(Weapon.Type _Type)
    {
        // Choose pickup playlist based on criteria
        System.Random rnd = new System.Random();// Generate seed (System.Random is more random than UnityEngine.Random)
        int r = rnd.Next(100);                  // Get random non-negative int less than 100
        if (r < 50)                             // Play audio if random number < 50
            SetPlaylist(m_pickupPlaylist);
        else
        {
            switch (_Type)
            {
                case Weapon.Type.GumballLauncher:
                    Play_Pickup_GumballLauncher();
                    break;
                case Weapon.Type.WallGun:
                    Play_Pickup_WallInACan();
                    break;
                case Weapon.Type.SharkZooka:
                    Play_Pickup_SharkZooka();
                    break;
                default:
                    SetPlaylist(m_pickupPlaylist);
                    break;
            }
        }
    }

    public void Play_Time_Low() { SetPlaylist(m_timeLowPlaylist, true); }

    public void Play_Random()
    {
        SetPlaylist(m_randomPlaylist);
        if (m_silence)    // If it decided not to speak, count silence again
            StartCoroutine(SilenceTracker());
    }

    #endregion  // Public

    #region Private

    #region Intros

    private void Play_Intro_Generic()
    {
        SetPlaylist(m_introPlaylist, true);
    }
    private void Play_Intro_LongShortRounds()
    {
        SetPlaylist(m_introLongShortRoundsPlaylist, true);
    }
    private void Play_Intro_FallOff()
    {
        SetPlaylist(m_introFallOffPlaylist, true);
    }
    private void Play_Intro_LMS()
    {
        SetPlaylist(m_introLMSPlaylist, true);
    }
    private void Play_Intro_KotH()
    {
        SetPlaylist(m_introKOTHPlaylist, true);
    }

    #endregion  // Intros

    #region Outros

    private void Play_Outro_Generic()
    {
        SetPlaylist(m_outroPlaylist, true);
    }
    private void Play_Outro_LongRound()
    {
        SetPlaylist(m_outroLongRoundPlaylist, true);
    }
    private void Play_Outro_ShortRound()
    {
        SetPlaylist(m_outroShortRoundPlaylist, true);
    }
    private void Play_Outro_FallOff()
    {
        SetPlaylist(m_outroFallOffPlaylist, true);
    }
    private void Play_Outro_AllDead()
    {
        SetPlaylist(m_outroAllDeadPlaylist, true);
    }
    private void Play_Outro_FinalRound()
    {
        SetPlaylist(m_outroFinalRoundPlaylist, true);
    }

    #endregion  // Outros

    #region Pickups

    private void Play_Pickup_GumballLauncher() { SetPlaylist(m_pickupGumballPlaylist); }
    private void Play_Pickup_SharkZooka() { SetPlaylist(m_pickupSharkPlaylist); }
    private void Play_Pickup_WallInACan() { SetPlaylist(m_pickupWallPlaylist); }

    #endregion

    /// <param name="_ForcePlay"> Ignore playlist manager's inturrupt and chatter's speaking chance settings </param>
    private void SetPlaylist(AudioPlaylist _Playlist, bool _ForcePlay = false)
    {
        bool play = _ForcePlay;

        if (!m_pausing && m_silence && !play)
        {
            System.Random rnd = new System.Random();// Generate seed (System.Random is more random than UnityEngine.Random)
            int r = rnd.Next(100);                  // Get random non-negative int less than 100
            if (r < m_speakingChance)                 // Play audio if random number < speaking chance
                play = true;
        }

        if (play)
        {
            m_silence = false;
            m_playlistManager.SetCurrentPlaylist(_Playlist, _ForcePlay);
        }
    }

    /// <summary> Pause between clips to avoid noise overload. </summary>
    /// <returns></returns>
    private IEnumerator Pause()
    {
        m_pausing = true;
        float pauseTime = UnityEngine.Random.Range(m_minPauseTimer, m_maxPauseTimer);   // Generate random pause time (UnityEngine.Random is needed for float)
        float pauseTimer = 0.0f;

        while (m_pausing)
        {
            pauseTimer += Time.fixedDeltaTime;
            if (m_playingOutro || pauseTimer >= pauseTime)
                m_pausing = false;
            yield return new WaitForFixedUpdate();
        }

        if (!m_playingOutro)
            StartCoroutine(SilenceTracker());
    }

    /// <summary> Tracks how long game goes without chatter. Plays clip from random playlist if silence is too long. </summary>
    /// <returns></returns>
    private IEnumerator SilenceTracker()
    {
        float silenceTime = UnityEngine.Random.Range(m_minSilenceTimer, m_maxSilenceTimer); // Generate random max silence time (UnityEngine.Random is needed for float)
        float silenceTimer = 0;

        while (m_silence)
        {
            silenceTimer += Time.fixedDeltaTime;
            if (silenceTimer >= silenceTime)
                break;
            yield return new WaitForFixedUpdate();
        }

        if (m_silence)
            Play_Random();
    }

    #endregion  // Private

    #endregion  // Functions

    //---------------------------------------------------------------------------
    #region EventsAndHandlers

    #region EventHandlers

    public EventHandler IntroFinished;
    public EventHandler OutroFinished;
    public EventHandler ClipFinished;

    #endregion  // Event Handlers

    #region Events

    private void Intro_Finished()
    {
        m_playingIntro = false;
        IntroFinished?.Invoke(this, EventArgs.Empty);
    }

    private void Outro_Finished()
    {
        m_playingOutro = false;
        OutroFinished?.Invoke(this, EventArgs.Empty);
    }

    private void Clip_Finished(object _Sender, EventArgs _Args)
    {
        m_silence = true; // No chatter is playing

        if (m_playOnce)
            m_playlistManager.Btn_StopCurrentPlaylist();

        StartCoroutine(Pause());    // Go without playing new clip until pause is over

        if (m_playingIntro)
            Intro_Finished();
        else if (m_playingOutro)
            Outro_Finished();

        ClipFinished?.Invoke(this, EventArgs.Empty);
    }

    #endregion  // Events

    #endregion  // Events & Handlers
}