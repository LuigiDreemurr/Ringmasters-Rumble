using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodexMenu : Menu<CodexMenu> 
{

    public void ShowPanel(GameObject _Panel)
    {
        _Panel.SetActive(true);
    }

    public void HidePanel(GameObject _Panel)
    {
        _Panel.SetActive(false);
    }
}
