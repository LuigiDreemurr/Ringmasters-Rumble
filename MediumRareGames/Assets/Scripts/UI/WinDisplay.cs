/*
-----------------------------------------------------------------------------
       Created By Zachary Pilon/Wesley Ducharme
----------------------------------------------------------------------------- 
   WinDisplay
       - A simple script to hold reference to child UI components that allows
         for controlled changes

   Details:
       - Contains the win counter (text), win bar (image), and player info (temp, text)
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinDisplay : MonoBehaviour
{
    public enum MedalType { NONE, Gold, Silver, Bronze }

    [SerializeField] private Text m_winCounter;
    [SerializeField] private Image m_winBar;
    [SerializeField] private Text m_player; //TODO: Use character image instead
    [SerializeField] private Image m_goldMedal;
    [SerializeField] private Image m_silverMedal;
    [SerializeField] private Image m_bronzeMedal;
    [Space]
    [SerializeField] private float m_winAnimSpeed = 0.5f;

    #region Properties
    /// <summary>Updates the win counter and bar fill amount</summary>
    public int Wins
    {
        set
        {
            m_winCounter.text = value.ToString();

            if (MatchHandler.Winner != null && m_player.text == MatchHandler.Winner.name)
                StartCoroutine(WinAnimRoutine(value));
            else
                m_winBar.fillAmount = (float)value / (float)MatchHandler.WinsNeeded;
        }
    }

    /// <summary>Updates the bar color</summary>
    public Color Color
    {
        set
        {
            m_winBar.color = value;
        }
    }

    /// <summary>Updates the player name</summary>
    public string Name
    {
        set
        {
            m_player.text = value;
        }
    }
    #endregion

    /// <summary>Simple routine that allows the fill amount to be animated</summary>
    /// <param name="_Wins">The wins the player has</param>
    private IEnumerator WinAnimRoutine(int _Wins)
    {
        AwardMedal();

        //Need to start the fill amount at the previous win count fill amount
        if (_Wins > 0)
            m_winBar.fillAmount = (float)(_Wins - 1) / (float)MatchHandler.WinsNeeded;

        //Calculate the goal amount
        float goalFillAmount = (float)_Wins / (float)MatchHandler.WinsNeeded;

        //Continue to animate while the goal amount has not been reached
        while(m_winBar.fillAmount != goalFillAmount)
        {
            //TODO: Allow controllinput to skip animation (can just set fillamount to goal at that time)

            m_winBar.fillAmount = Mathf.MoveTowards(m_winBar.fillAmount, goalFillAmount, m_winAnimSpeed * Time.unscaledDeltaTime);

            yield return null;
        }

        m_winBar.fillAmount = goalFillAmount;
    }

    public void AwardMedal(MedalType _Medal = MedalType.NONE)
    {
        switch (_Medal)
        {
            case MedalType.Gold:
                m_goldMedal.enabled = true;
                m_silverMedal.enabled = false;
                m_bronzeMedal.enabled = false;
                break;
            case MedalType.Silver:
                m_goldMedal.enabled = false;
                m_silverMedal.enabled = true;
                m_bronzeMedal.enabled = false;
                break;
            case MedalType.Bronze:
                m_goldMedal.enabled = false;
                m_silverMedal.enabled = false;
                m_bronzeMedal.enabled = true;
                break;
            default:    // case MedalType.NONE:
                m_goldMedal.enabled = false;
                m_silverMedal.enabled = false;
                m_bronzeMedal.enabled = false;
                break;
        }
    }
}
