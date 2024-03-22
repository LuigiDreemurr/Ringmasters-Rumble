using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScreen : MonoBehaviour {

    public MenuJoystickInfo[] UIJoysticks;

    [System.Serializable]
    public struct MenuJoystickInfo
    {
        public Text joystick, slackOffset;
    }
    
	void Update () {
		for(int i = 0; i < UIJoysticks.Length; i++)
        {
            if(JoystickInfo.GetJoystickInfo(i) == null) { return; }
            UIJoysticks[i].joystick.text = JoystickInfo.GetJoystickInfo(i).name;
            UIJoysticks[i].slackOffset.text = JoystickInfo.GetJoystickInfo(i).slack.ToString("0.000") + " [" + JoystickInfo.GetJoystickInfo(i).deadZone.ToString("0.000") + "]";
        }
	}
}

public static class JoystickInfo
{
    public static MenuJoystickInfo[] UIJoysticks = new MenuJoystickInfo[4];

    public class MenuJoystickInfo
    {
        public string name;
        public float slack;
        public float deadZone;
    }

    public static MenuJoystickInfo GetJoystickInfo(int joystick)
    {
        if(UIJoysticks[joystick] != null)
        {
            return UIJoysticks[joystick];
        }
        else { return null; }
    }

    public static void SetJoystickInfo(int joystick, MenuJoystickInfo info)
    {
            UIJoysticks[joystick] = info;
    }
}
