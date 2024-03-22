/*
 -----------------------------------------------------------------------------
        Created By Brandon Vout
 -----------------------------------------------------------------------------
    SCRIPT_NAME
        - Send arena away from its starting location, move arena back to start.

    Details:
        - 
 -----------------------------------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaRise : MonoBehaviour
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

    [SerializeField] private float startingDistance = 80;
    [SerializeField] private float speed = 10;

    private Vector3 startPos;
    private Vector3 endPos;

    #endregion  // Private

    #endregion  // Variables

    //---------------------------------------------------------------------------
    #region MonoBehaviour 

    /// <summary> Use this for initialization </summary>
    void Start()
    {
        StartCoroutine(RaiseArena());
    }

    #endregion  // MonoBehaviour 

    //---------------------------------------------------------------------------
    #region Functions

    #region Public



    #endregion  // Public

    #region Private

    private IEnumerator RaiseArena()
    {
        float step;

        endPos = gameObject.transform.position;
        startPos = new Vector3(gameObject.transform.position.x,
            gameObject.transform.position.y - startingDistance,
            gameObject.transform.position.z);
        gameObject.transform.position = startPos;

        while (gameObject.transform.position != endPos)
        {
            step = speed * Time.deltaTime;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, endPos, step);
            yield return new WaitForEndOfFrame();
        }
        Arena_Risen();
    }

    #endregion  // Private

    #endregion  // Functions

    //---------------------------------------------------------------------------
    #region EventsAndHandlers

    #region EventHandlers

    public EventHandler ArenaRisen;

    #endregion  // Event Handlers

    #region Events

    private void Arena_Risen()
    {
        ArenaRisen?.Invoke(this, EventArgs.Empty);
    }

    #endregion  // Events

    #endregion  // Events & Handlers
}