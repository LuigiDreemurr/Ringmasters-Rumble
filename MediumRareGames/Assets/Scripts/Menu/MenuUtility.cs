/*
 -----------------------------------------------------------------------------
        Created By Wesley Ducharme
 -----------------------------------------------------------------------------
    MenuUtility
        - A class mean't to hold simple utility functions to be used by menus

    Details:
        - Can possibly extend this class using extension methods
          https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods
        - Singleton
 -----------------------------------------------------------------------------
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MenuUtility : MonoBehaviour
{
    #region Singleton
    //Instance of MenuUtility
    static private MenuUtility s_instance;

    /// <summary>Get the only instance of MenuUtility</summary>
    static public MenuUtility Instance { get { return s_instance; } }

    /// <summary>Singleton initialization</summary>
    protected void Awake()
    {
        if (s_instance == null)
            s_instance = this;
        else
        {
            Debug.LogError("Two MenuUtility exist (Destroying): " + gameObject.name);
            Destroy(gameObject);
        }
    }
    #endregion

    #region Utility Methods
    /// <summary>Load a scene through its name</summary>
    /// <param name="_SceneName">The name of the scene</param>
    public void LoadScene(string _SceneName)
    {
        MatchHandler.ResetLongRounds();
        MatchHandler.ResetFall();
        SceneManager.LoadScene(_SceneName);
    }

    /// <summary>Load a scene through its build index</summary>
    /// <param name="_SceneName">The build index of the scene</param>
    public void LoadScene(int _SceneBuildIndex)
    {
        MatchHandler.ResetLongRounds();
        MatchHandler.ResetFall();
        SceneManager.LoadScene(_SceneBuildIndex);
    }

    /// <summary>Quit the application. Will exit play mode if in the editor</summary>
    public void QuitApplication()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    #endregion
}
