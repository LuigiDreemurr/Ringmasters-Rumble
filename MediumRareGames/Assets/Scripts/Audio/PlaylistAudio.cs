/*
 -----------------------------------------------------------------------------
        Created By Brandon Vout
 -----------------------------------------------------------------------------
    Playlist Audio
        - Holds audio-related data for AudioPlaylist

    Details:
        - Stores audio clip
        - Stores volume float
        - If volume is given as between 1 & 100, convert to between 0 & 1
        - If volume is below 0, set to 0
        - If volume is greater than 100, set volume to 1
        - Stores playlist buffer value
        - Stores bool to show whether it has been played for the first time
 -----------------------------------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaylistAudio
{

    #region Variables

    #region Constants

    const float MIN_VOLUME = 0f;
    const float MAX_VOLUME = 1f;

    #endregion  // Constants

    #region Public

    public AudioClip Clip { get { return m_clip; } }
    public float Volume
    {
        get { return m_volume; }
        set
        {
            if (value < m_volume)
                m_volume = Mathf.Max(value, MIN_VOLUME);
            else
                m_volume = Mathf.Min(value, MAX_VOLUME);
        }
    }
    public int BufferCount
    {
        get { return m_bufferCount; }
        set { m_bufferCount = value; }
    }
    public bool IsFirstPlay { get { return m_isFirstPlay; } }

    #endregion  // Public

    #region private

    private AudioClip m_clip;
    private float m_volume;
    private int m_bufferCount;  // Clips played since this was last played
    private bool m_isFirstPlay; // Is this the first time it's been played?

    #endregion  // Private

    #endregion  // Variables

    //---------------------------------------------------------------------------

    #region ConstructorsAndDestructors

    #region Constructors

    /// <summary>
    /// Create Playlist Audio using AudioClip and custom Volume (float between 0 and 1)
    /// </summary>
    /// <param name="_Clip"> AudioClip </param>
    /// <param name="_Volume"> Volume between 0 and 1 </param>
    public PlaylistAudio(AudioClip _Clip, float _Volume = MAX_VOLUME)
    {
        m_clip = _Clip;
        Volume = _Volume;
        ResetBufferCount();
        m_isFirstPlay = true;
    }

    #endregion  // Constructors

    #region Destructors

    ~PlaylistAudio()
    {
        m_clip = null;
        m_volume = 0.0f;
        m_bufferCount = 0;
        m_isFirstPlay = false;
    }

    #endregion  // Destructors

    #endregion  // ConstructorsAndDestructors

    //---------------------------------------------------------------------------

    #region Functions

    #region Public
        
    public void SetClip(AudioClip _Clip) { m_clip = _Clip; }
    public void IncrementBufferCount() { m_bufferCount++; }
    public void ResetBufferCount() { m_bufferCount = 0; }
    public void FirstPlay() { m_isFirstPlay = false; }

    #endregion  // Public

    #region Private



    #endregion  // Private

    #endregion  // Functions
}
