/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   SimpleTimer
       - A simple script for a simple timer providing two events, OnStart and OnEnd

   Details:
       - Can be started (Begin()) and stopped (Stop())
       - Utilizes WaitForSeconds in coroutine so it does not keep track of 
         elapsed time
       - OnEnd will not be called when the timer is stopped manually using Stop()
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTimer : MonoBehaviour
{
    #region Data Members
    [SerializeField] private float m_time; //How long the timer lasts
    private Coroutine m_timerRoutine;

    /// <summary>How long does the timer last</summary>
    public float Time { get { return m_time; } }

    /// <summary>Is the timer currently running</summary>
    public bool Running { get { return m_timerRoutine != null; } }
    #endregion

    #region Events
    public delegate void SimpleTimerEvent(SimpleTimer _Timer);

    /// <summary>Called when the timer begins/starts</summary>
    public event SimpleTimerEvent OnStart;

    /// <summary>Called only when the timer ends naturally (will not be called if using Stop())</summary>
    public event SimpleTimerEvent OnEnd;
    #endregion

    /// <summary>Start the timer</summary>
    public void Begin()
    {
        if (!Running)
            m_timerRoutine = StartCoroutine(TimerRoutine());
    }

    /// <summary>Stop the timer</summary>
    public void Stop()
    {
        if(Running)
        {
            StopCoroutine(m_timerRoutine);
            m_timerRoutine = null;
        }
    }

    /// <summary>Basic coroutine used for the timer</summary>
    private IEnumerator TimerRoutine()
    {
        OnStart?.Invoke(this);
        yield return new WaitForSeconds(m_time);
        m_timerRoutine = null;
        OnEnd?.Invoke(this);
    }
}
