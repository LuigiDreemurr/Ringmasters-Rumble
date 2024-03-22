/*
 -----------------------------------------------------------------------------
        Created By Brandon Vout
 -----------------------------------------------------------------------------
    Audio Playlist
        - For playing background music from multiple clips (in order of suffled)

    Details:
        - Changes the AudioClip and volume float values of an AudioSource
        - Randomizes order of clips
        - Can use buffer to prevent clips from playing too frequently/infrequently
        - Can add clips to playlist and merge playlists
        - Has Clip Finished Event
 -----------------------------------------------------------------------------
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class AudioPlaylist : MonoBehaviour
{

    #region Classes



    #endregion  // Classes

    //---------------------------------------------------------------------------
    #region Variables

    #region Constants



    #endregion  // Constants

    #region Public

    public PlaylistAudio[] Clips { get { return pAudio; } }

    public bool Shuffle
    {
        get { return shuffle; }
        set { shuffle = value; }
    }

    public bool IsPlaying { get { return isPlaying; } }

    #endregion  // Public

    #region Private

    [SerializeField] [Lockable] private AudioSource source;         // Where the sound is coming from (camera for BG music, object for "radio")
    [SerializeField] [Lockable] private AudioClip[] clips;          // Playlist clip
    [SerializeField] [Lockable] private float[] volumes = { 1f };   // Volume for each clip (Only used when overrideAudioSourceVolume == true)

    [SerializeField] [Lockable] private bool shuffle = false;           // Randomize Playlist
    [SerializeField] [Lockable] private bool playFirstClipFirst = false;// Play first song in playlist first on start even if (shuffle == true)
    [SerializeField] [Lockable] private bool playOnStart = true;

    [SerializeField] [Lockable] private int minPlayBuffer = 1;          // How many different clips have to play before clip can be played again?
    [SerializeField] [Lockable] private int maxPlayBuffer = 4;          // How many other clips can be played before clip has to be played again? (Set to <0 to ignore)

    [SerializeField] [Lockable] private int minPlaylistStartSize = 6;   // Playlist Queue size at start
    [SerializeField] [Lockable] private int minPlaylistSize = 3;        // Keep playlist from going below this number, acts as a buffer

    private PlaylistAudio[] pAudio;         // Private array for script
    private Queue<PlaylistAudio> playlist;  // Playlist queue

    private bool isPlaying;

    private int playlistSize;                                       // Size of playlist

    [SerializeField] [Lockable] private bool overrideAudioSourceVolume = false; // Override source volume with values in array, leave false if using outside volume controller
    private bool stopAudioOverride = false; // Force playlist to stop, do not let script start playing again while this is true

    #endregion  // Private

    #endregion  // Variables

    //---------------------------------------------------------------------------
    #region MonoBehaviour

    /// <summary> Use this for initialization </summary>
    private void Awake()
    {
        isPlaying = false;
        if (clips.Length > 0)
        {
            pAudio = new PlaylistAudio[clips.Length];           // Create private clips array
            for (int i = 0; i < clips.Length; i++)              // Add clips to private array
            {
                if (i >= volumes.Length)
                    pAudio[i] = new PlaylistAudio(clips[i], volumes[volumes.Length - 1]);   // Use final volume value
                else
                    pAudio[i] = new PlaylistAudio(clips[i], volumes[i]);
            }
            playlist = new Queue<PlaylistAudio>();          // Initialize playlist
        }
        playlistSize = 0;
        if (minPlaylistStartSize < 2)
            minPlaylistStartSize = 2;
        if (pAudio != null)
        {
            do                                              // Find size of Playlist
            {
                playlistSize += pAudio.Length;
            } while (playlistSize < minPlaylistStartSize);  // Keep playlist at a minimum size to act as a buffer
            LoadQueue();
        }
        if (minPlaylistSize < 2)    // Checks if count < min size, count should not be < 2
            minPlaylistSize = 2;
    }

    /// <summary> Use this for initialization, after Awake() </summary>
    void Start()
    {
        if (source != null && playOnStart)
        {
            SetSourceClip();
            PlayAudio();
        }
    }

    #endregion  // MonoBehaviour

    //---------------------------------------------------------------------------
    #region Functions

    #region Public

    public int GetPlaylistLength() { return pAudio.Length; }
    public PlaylistAudio GetPlaylistAudio(int _Index) { return pAudio[_Index]; }
    public AudioClip GetAudioClip(int _Index) { return pAudio[_Index].Clip; }

    public void SetMinBuffer(int _MinBuffer) { minPlayBuffer = _MinBuffer; }
    public void SetMaxBuffer(int _MaxBuffer) { maxPlayBuffer = _MaxBuffer; }
    public void SetMinMaxBuffers(int _MinBuffer, int _MaxBuffer)
    {
        SetMinBuffer(_MinBuffer);
        SetMaxBuffer(_MaxBuffer);
    }

    public void ResetPlaylist()
    {
        playlist = new Queue<PlaylistAudio>();
        LoadQueue();
    }

    /// <summary> Add clip to playlist. </summary>
    /// <param name="_Clip"></param>
    public void AddClip(AudioClip _Clip)
    {
        bool wasPlayling = false;
        if (isPlaying)
        {
            wasPlayling = true;
            StopAudio();
        }

        AudioClip[] tempClips = clips;
        clips = new AudioClip[clips.Length + 1];
        pAudio = new PlaylistAudio[clips.Length];
        for (int i = 0; i < clips.Length; i++)
        {
            if (i < tempClips.Length)
                clips[i] = tempClips[i];
            else
                clips[i] = _Clip;
            pAudio[i] = new PlaylistAudio(clips[i]);
        }

        if (wasPlayling)
            PlayAudio();
    }

    /// <summary> Add PlaylistAudio to playlist. </summary>
    /// <param name="_Clip"></param>
    public void AddPlaylistAudio(PlaylistAudio _Audio)
    {
        bool wasPlayling = false;
        if (isPlaying)
        {
            wasPlayling = true;
            StopAudio();
        }

        PlaylistAudio[] tempPAudio = pAudio;
        pAudio = new PlaylistAudio[tempPAudio.Length + 1];
        for (int i = 0; i < pAudio.Length; i++)
        {
            if (i < tempPAudio.Length)
                pAudio[i] = tempPAudio[i];
            else
                pAudio[i] = _Audio;
        }

        if (wasPlayling)
            PlayAudio();
    }

    /// <summary> Concatinates a playlist's audio to this playlists's audio. </summary>
    /// <param name="_Playlist"></param>
    public void MergePlaylist(AudioPlaylist _Playlist)
    {
        if (_Playlist != null)
        {
            if (pAudio == null)
                pAudio = new PlaylistAudio[0];
            pAudio = pAudio.Concat(_Playlist.Clips).ToArray();
        }
    }

    public void SetSource(AudioSource _Source)
    {
        if (_Source != null)
        {
            if (source != null)
                StopAudio();
            source = _Source;
            source.loop = false;
            isPlaying = IsSourcePlayling();
        }
    }

    /// <summary> Set end of playlist to be source clip, plays source </summary>
    public void SetSourceClip()
    {
        if (source != null && playlist != null)
        {
            source.clip = playlist.Peek().Clip;
            if (overrideAudioSourceVolume)
                source.volume = playlist.Peek().Volume;
            if (shuffle)    // If set to shuffle, keep track of bufferCount, does not matter outside of shuffle
            {
                for (int i = 0; i < pAudio.Length; i++)
                    pAudio[i].IncrementBufferCount();
                playlist.Peek().ResetBufferCount();
            }
            isPlaying = IsSourcePlayling();
        }
    }

    public void PlayAudio()
    {
        if (source != null)
        {
            playOnStart = true; // To ensure PlaylistManager's play of start works
            stopAudioOverride = false;
            if (!source.isPlaying)
                source.Play();
            isPlaying = true;
            StartCoroutine(PlayingClip());  // Keep track of whether clip is playing
        }
    }

    public void StopAudio()
    {
        if (source != null)
        {
            stopAudioOverride = true;
            source.Stop();
            isPlaying = false;
        }
    }

    public void PauseAudio()
    {
        if (source != null)
        {
            stopAudioOverride = true;
            source.Pause();
            isPlaying = false;
        }
    }

    public bool IsSourcePlayling()
    {
        if (source != null)
            return source.isPlaying;
        else
            return false;
    }

    /// <summary> Changes volume values on all PlaylistAudios. Overrides all values </summary>
    /// <param name="_Volume"></param>
    public void SetVolume(float _Volume)
    {
        for (int i = 0; i < pAudio.Length; i++)
            pAudio[i].Volume = _Volume;
    }

    /// <summary> Send next clip in queue to source. Refill queue if it runs low. </summary>
    public void NextClip()
    {
        if (playlist.Count < minPlaylistSize)
        {
            LoadQueue();                    // If queue is nearly empty, refill
        }
        playlist.Dequeue();
        SetSourceClip();
        PlayAudio();
    }

    #endregion  // Public

    #region Private

    /// <summary> Checks whether clip is still playing, continue until it stops. Start next clip on stop. </summary>
    /// <returns></returns>
    private IEnumerator PlayingClip()
    {
        while (isPlaying && !stopAudioOverride)
        {
            isPlaying = source.isPlaying;
            yield return new WaitForFixedUpdate();
        }

        Clip_Finished();    // Fire event
        if (!stopAudioOverride)
            NextClip();     // If source has stopped playing, play next clip in playlist if not forced to stop
    }

    /// <summary> Fill playlist </summary>
    void LoadQueue()
    {
        if (!shuffle)                                       // If not set to shuffle, fill playlist with clips in order
            do
                for (int i = 0; i < pAudio.Length; i++)
                    playlist.Enqueue(pAudio[i]);
            while (playlist.Count < playlistSize);
        else                                                // If set to shuffle from start then call shuffler
            ShufflePlaylist();
    }

    /// <summary> Randomize playlist </summary>
    void ShufflePlaylist()
    {
        bool maxBufferHit = false;

        if (playFirstClipFirst && playlist.Count == 0)
            playlist.Enqueue(pAudio[0]);
        for (int i = playlist.Count; i < playlistSize; i++)
        {
            for (int j = 0; j < pAudio.Length && maxPlayBuffer > 0; j++)        // Check if any clips are at the max play buffer (if max < 1, ignore max play buffer)
            {
                if (pAudio[j].BufferCount + i >= maxPlayBuffer)                 // If max play buffer is hit or surpassed, add clip to queue
                {
                    pAudio[j].BufferCount = -i;
                    playlist.Enqueue(pAudio[j]);
                    maxBufferHit = true;
                    break;
                }
            }
            while (!maxBufferHit)
            {
                System.Random rnd = new System.Random();            // Generate seed (System.Random is more random than UnityEngine.Random)
                int r = rnd.Next(0, pAudio.Length);                 // Get random int
                if (pAudio[r].BufferCount + i > minPlayBuffer || pAudio[r].IsFirstPlay || minPlayBuffer < 0)
                {                                                   // If above min play buffer, first time clip is drawn, or if min buffer < 0, add clip to queue
                    pAudio[r].FirstPlay();
                    pAudio[r].BufferCount = -i;
                    playlist.Enqueue(pAudio[r]);
                    break;
                }
            }
            maxBufferHit = false;
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

    private void Clip_Finished() { ClipFinished?.Invoke(this, EventArgs.Empty); }

    #endregion  // Events

    #endregion  // Events & Handlers
}
