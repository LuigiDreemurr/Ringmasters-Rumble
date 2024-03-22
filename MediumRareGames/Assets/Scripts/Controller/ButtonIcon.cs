/*
-----------------------------------------------------------------------------
       Created By Chris Knight
-----------------------------------------------------------------------------
   ButtonIcon
       - Sets up the image this script is attached to with a Xbox icon and optional text.

   Details:
       - Drag XBox Button from the Prefabs folder
       - Optionally set the text
       - Optionally set the icon intially
       - Using another script, call ChangeButtonIcon and use the relevant overload
-----------------------------------------------------------------------------
*/

using Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonIcon : MonoBehaviour {

    public bool visible = true;
    public string buttonText; //optional
    public XboxIcons buttonIcon;

    private Image img;
    private Text txt;

    private void Start()
    {
        img = GetComponent<Image>();
        txt = transform.GetChild(0).GetComponent<Text>();
        SetButtonIcon();
    }

    private void SetButtonIcon()
    {
        img.enabled = visible;
        txt.text = buttonText;
        img.sprite = ControllerUI.GetIcon(buttonIcon);
    }

    /// <summary>
    /// Changes the icon of this image, to the passed in Xbox Icon button.
    /// </summary>
    /// <param name="newButtonIcon">The new Xbox Icon to change this image to.</param>
    public void ChangeButtonIcon(XboxIcons newButtonIcon)
    {
        buttonIcon = newButtonIcon;
        SetButtonIcon();
    }

    /// <summary>
    /// Changes the icon of this image, to the passed in Xbox Icon button.
    /// </summary>
    /// <param name="newButtonIcon">The new Xbox Icon to change this image to.</param>
    /// <param name="isVisible">Should the icon be visible?</param>
    public void ChangeButtonIcon(XboxIcons newButtonIcon, bool isVisible, string displayText)
    {
        visible = isVisible;
        buttonText = displayText;
        buttonIcon = newButtonIcon;
        SetButtonIcon();
    }

    /// <summary>
    /// Convert a Button to a Xbox Icon.
    /// </summary>
    /// <param name="button">Button to convert.</param>
    /// <returns></returns>
    public static XboxIcons ConvertInput(Controller.Button button)
    {
        switch (button)
        {
            case Controller.Button.A:
                return XboxIcons.A;
            case Controller.Button.B:
                return XboxIcons.B;
            case Controller.Button.X:
                return XboxIcons.X;
            case Controller.Button.Y:
                return XboxIcons.Y;
            case Controller.Button.Start:
                return XboxIcons.Start;
            case Controller.Button.Back:
                return XboxIcons.Menu;
            case Controller.Button.LeftBumper:
                return XboxIcons.LB;
            case Controller.Button.RightBumper:
                return XboxIcons.RB;
            case Controller.Button.LeftStick:
                return XboxIcons.LS_Press;
            case Controller.Button.RightStick:
                return XboxIcons.RS_Press;

            default: return XboxIcons.A;
        }
    }
}
