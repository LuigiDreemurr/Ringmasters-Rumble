/*
 -----------------------------------------------------------------------------
        Created By Brandon Vout
 -----------------------------------------------------------------------------
    Countdown
        - Counts down after intro, starts round timer when finished

    Details:
        - Counts down using a timer
        - Fades text in and out
        - Initializes player controls in menu controller
        - Has countdown finished event
        - Audio not implemented, yet
 -----------------------------------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInput;

public class Countdown : MonoBehaviour
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

    [SerializeField] [Lockable] private AnnouncerChatter announcerChatter;
    [SerializeField] [Lockable] private AudioPlaylist countdownPlaylist;
    //[SerializeField] [Lockable] private MenuController menuController;
    [Space]

    [SerializeField] [Lockable] private int startCount = 5;
    [Tooltip("1.0 == 1 second interval during count down, e.g. change to 2.0 to count down every 2 seconds")]
    [SerializeField]
    [Lockable]
    private float counterInterval = 1.0f;  // E.G. 1 == 1 second intervals
    [SerializeField] [Lockable] private Text counterText;
    [Space]

    [Tooltip("How transparent the text will become.")]
    [SerializeField]
    [Lockable]
    private float minAlpha = 0.0f;  // How transparent the text will become
    [Tooltip("How opaque the text will become.")]
    [SerializeField]
    [Lockable]
    private float maxAlpha = 1.0f;  // How opaque the text will become

    #endregion  // Private

    #endregion  // Variables

    //---------------------------------------------------------------------------
    #region MonoBehaviour 

    /// <summary> Use this for initialization </summary>
    void Start()
    {
        ControllerManager.Instance.SendControllerEvents(false);

        counterText.text = "";
        announcerChatter.IntroFinished += Intro_Finished;

        if (announcerChatter == null && gameObject.GetComponent<AnnouncerChatter>() != null)
            announcerChatter = gameObject.GetComponent<AnnouncerChatter>();
        if (countdownPlaylist == null && gameObject.GetComponent<AudioPlaylist>() != null)
            countdownPlaylist = gameObject.GetComponent<AudioPlaylist>();
        //if (menuController == null && gameObject.GetComponent<MenuController>() != null)
        //    menuController = gameObject.GetComponent<MenuController>();
        if (counterText == null && gameObject.GetComponent<Text>() != null)
            counterText = gameObject.GetComponent<Text>();
    }

    #endregion  // MonoBehaviour 

    //---------------------------------------------------------------------------
    #region Functions

    #region Public

    public void StartCountdown()
    {
        StartCoroutine(Countingdown());
    }

    #endregion  // Public

    #region Private

    private IEnumerator Countingdown()
    {
        int counter = startCount;
        counterText.color = new Color(counterText.color.r, counterText.color.g, counterText.color.b, maxAlpha);
        // TODO: Add Audio Triggers
        while (counter > -1)
        {
            counterText.text = counter.ToString();
            if (counterText.text == "0")
                counterText.text = "Fight!";
            StartCoroutine(FadeTextToMaxAlpha(counterInterval * 0.5f));
            yield return new WaitForSeconds(counterInterval * 0.8f);
            StartCoroutine(FadeTextToMinAlpha(counterInterval * 0.2f));
            yield return new WaitForSeconds(counterInterval * 0.2f);
            counter--;
        }
        counterText.text = "";
        yield return null;

        Countdown_Finished();
    }

    private IEnumerator FadeTextToMaxAlpha(float _Seconds)
    {
        counterText.color = new Color(counterText.color.r, counterText.color.g, counterText.color.b, minAlpha);
        while (counterText.color.a < maxAlpha)
        {
            counterText.color = new Color(counterText.color.r, counterText.color.g, counterText.color.b, counterText.color.a + (Time.deltaTime / _Seconds));
            yield return null;
        }
    }

    private IEnumerator FadeTextToMinAlpha(float _Seconds)
    {
        counterText.color = new Color(counterText.color.r, counterText.color.g, counterText.color.b, maxAlpha);
        while (counterText.color.a > minAlpha)
        {
            counterText.color = new Color(counterText.color.r, counterText.color.g, counterText.color.b, counterText.color.a - (Time.deltaTime / _Seconds));
            yield return null;
        }
    }

    #endregion  // Private

    #endregion  // Functions

    //---------------------------------------------------------------------------
    #region EventsAndHandlers

    #region EventHandlers

    public EventHandler CountdownFinished;

    #endregion  // Event Handlers

    #region Events

    private void Intro_Finished(object _Sender, EventArgs _Args)
    {
        announcerChatter.IntroFinished -= Intro_Finished;
        StartCountdown();
    }

    private void Countdown_Finished()
    {
        ControllerManager.Instance.SendControllerEvents(true);
        //menuController?.InitializeControllers();
        CountdownFinished?.Invoke(this, EventArgs.Empty);   // TODO: Prevent announcer from speaking until after countdown
    }

    #endregion  // Events

    #endregion  // Events & Handlers
}
