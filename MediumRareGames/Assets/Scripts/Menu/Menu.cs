/*
 -----------------------------------------------------------------------------
        Created By Wesley Ducharme
 -----------------------------------------------------------------------------
    Menu
        - Simple menu base classes

    Details:
        - Menu provides virtual functions to allow functionality when hiding
          or showing menus
        - Menu<T> provides singleton setup for specific menus
 -----------------------------------------------------------------------------
 */

using UnityEngine;

public abstract class Menu : MonoBehaviour 
{
    //What UI component is initially selected
    [SerializeField] private GameObject m_initialSelect;

    /// <summary>Get the GameObject that should be selected initially</summary>
    public GameObject InitialSelect { get { return m_initialSelect; } }

    /// <summary>Called when a menu is shown</summary>
    public virtual void OnShow() { }

    /// <summary>Called when a menu is hidden</summary>
    public virtual void OnHide() { }
}

public abstract class Menu<T> : Menu where T : Menu<T>
{
    //Instance of menu, T
    static private T s_instance;

    /// <summary>Get the only instance of this menu type</summary>
    static public T Instance { get { return s_instance; } }

    /// <summary>Singleton initialization</summary>
    protected void Awake()
    {
        if (s_instance == null)
            s_instance = (T)this;
        else
        {
            Debug.LogError("Two menus exist of type " + typeof(T).Name + " (Destroying): " + gameObject.name);
            Destroy(gameObject);
        }
    }
}