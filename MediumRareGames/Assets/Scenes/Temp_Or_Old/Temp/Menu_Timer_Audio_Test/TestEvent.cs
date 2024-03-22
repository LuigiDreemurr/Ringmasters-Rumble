// For testing timer null check
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestEvent : MonoBehaviour
{
    public Text txt;
    public RoundTimer timer;

    // Use this for initialization
    void Start()
    {
        if (timer != null)
        {
            timer.TimeIsUp += Timer_End;
        }
    }

    void Timer_End(object sender, EventArgs e)
    {
        txt.text = "End";
    }
}
