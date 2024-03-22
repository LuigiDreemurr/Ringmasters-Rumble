/*
 -----------------------------------------------------------------------------
        Created By Brandon Vout
 -----------------------------------------------------------------------------
    KingOfTheHillPoint
        - Starts player's timer when they enter the trigger, stop it when they leave.

    Details:
        - More detailed summary
        - List of inputs
        - List of uses/outputs
 -----------------------------------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingOfTheHillPoint : MonoBehaviour
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

    private bool active = false;

    #endregion  // Private

    #endregion  // Variables

    //---------------------------------------------------------------------------
    #region MonoBehaviour

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && active)
        {
            other.transform.parent.GetComponent<RoundTimer>().Btn_StartTimer();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && active)
        {
            other.transform.parent.GetComponent<RoundTimer>().Btn_StopTimer();
        }
    }

    #endregion  // MonoBehaviour 

    //---------------------------------------------------------------------------
    #region Functions

    #region Public

    public void Activate()
    {
        SetActivate(true);
    }

    public void Deactivate()
    {
        SetActivate(false);
    }

    #endregion  // Public

    #region Private

    private void SetActivate(bool _Activate)
    {
        active = _Activate;
        gameObject.SetActive(_Activate);
    }

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