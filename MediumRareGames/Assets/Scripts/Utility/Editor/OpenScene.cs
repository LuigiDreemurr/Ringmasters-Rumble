using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneItem : Editor
{
    [MenuItem("OpenScene/Menus")]
    static public void OpenMenus()
    {
        OpenScene("Menus");
    }

    [MenuItem("OpenScene/Arena 1")]
    static public void OpenArena1()
    {
        OpenScene("Arena_1");
    }

    [MenuItem("OpenScene/Arena 2")]
    static public void OpenArena2()
    {
        OpenScene("Arena_2");
    }

    [MenuItem("OpenScene/Arena 3")]
    static public void OpenArena3()
    {
        OpenScene("Arena_3");
    }


    static public void OpenScene(string _SceneName)
    {
        if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/Scenes/" + _SceneName + ".unity");
        }
    }
}
