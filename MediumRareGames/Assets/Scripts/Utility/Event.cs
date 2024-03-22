using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event<Sender, Args>
{
    public delegate void Handler(Sender _Sender, Args _Args);
    public event Handler m_event;

    /// <summary>Subscribe to this event</summary>
    /// <param name="_Handler">The subscriber</param>
    public void Subscribe(Handler _Handler)
    {
        m_event += _Handler;
    }

    /// <summary>Unsubscribe from this event</summary>
    /// <param name="_Handler">The subscriber to unsubscribe</param>
    public void UnSubscribe(Handler _Handler)
    {
        m_event -= _Handler;
    }

    /// <summary>Invoke this event</summary>
    /// <param name="_Sender">The event sender</param>
    /// <param name="_Args">The event arguments</param>
    public void Invoke(Sender _Sender, Args _Args)
    {
        m_event?.Invoke(_Sender, _Args);
    }
}
