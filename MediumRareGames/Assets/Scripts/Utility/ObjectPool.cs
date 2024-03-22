/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   ObjectPool<T, PooledObject>
       - An abstract class that allows for the easy implementation of object pooling

   Details:
       - When inheriting, T is the derived class (for setting up singleton)
       - PooledObject is a class that inherits monobehaviour and implements IPooled
       - Easy to use look to GumballPool, GumballLauncher, and Gumball for reference
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T, PooledObject> : MonoBehaviour 
    where T : ObjectPool<T, PooledObject>
    where PooledObject : MonoBehaviour, IPooled
{
    #region Static Members
    static private ObjectPool<T, PooledObject> s_instance;
    static public ObjectPool<T, PooledObject> Instance { get { return s_instance; } }

    /// <summary>Spawns (enables) an object from the pool</summary>
    /// <returns>The object spawned</returns>
    static public PooledObject Spawn()
    {
        return Spawn(Vector3.zero, Quaternion.identity);
    }

    /// <summary>Spawns (enables) an object from the pool</summary>
    /// <param name="_Position">The object position</param>
    /// <param name="_Rotation">The object rotation</param>
    /// <returns>The object spawned</returns>
    static public PooledObject Spawn(Vector3 _Position, Quaternion _Rotation)
    {
        if (Instance.m_pool.Count == 0)
            ConsoleLogging.Log.Error(Instance, "ObjectPool has count of 0");

        //Get a pooled object
        PooledObject spawned = Instance.m_pool.Dequeue();

        //Kill the object if it is alive
        if (spawned.gameObject.activeSelf)
            Kill(spawned);

        //Transform
        spawned.transform.position = _Position;
        spawned.transform.rotation = _Rotation;

        //Enable and spawn
        spawned.gameObject.SetActive(true);
        spawned.OnSpawn();

        //Add it back to the queue
        Instance.m_pool.Enqueue(spawned);

        //Return the pooled object that was spawned
        return spawned;
    }

    /// <summary>Kills (disables) an object from the pool</summary>
    /// <param name="_Killed">The object to kill</param>
    static public void Kill(PooledObject _Killed)
    {
        //Disable and kill
        _Killed.gameObject.SetActive(false);
        _Killed.OnKill();
    }

    #endregion

    #region Data Members
    [Tooltip("The prefab that is pooled (uses the script on the prefab)")]
    [SerializeField] protected PooledObject m_prefab;

    private Queue<PooledObject> m_pool;
    #endregion

    #region Unity Messages
    /// <summary>Awake used to set up singleton</summary>
    protected void Awake()
    {
        if (s_instance == null)
            s_instance = this;
        else
            ConsoleLogging.Log.Warning(this, "Two ObjectPool of the same type exist");
    }

    /// <summary>Initialization</summary>
    protected void Start()
    {
        //Get the pool size and initialize the queue
        int poolSize = Size();
        m_pool = new Queue<PooledObject>(poolSize);

        //Populate the queue based on the pool size
        for(int i=0; i<poolSize; i++)
        {
            GameObject gameObject = Instantiate(m_prefab.gameObject, transform, true);
            gameObject.SetActive(false);

            gameObject.GetComponent<PooledObject>().OnInit();

            m_pool.Enqueue(gameObject.GetComponent<PooledObject>());
        }
    }
    #endregion

    /// <summary>Determines how big the object pool is</summary>
    /// <returns>Returns the size of the object pool</returns>
    abstract protected int Size();
}

/// <summary>
/// Simple interface that any object in an ObjectPool needs to implement
/// </summary>
public interface IPooled
{
    /// <summary>What to do when this pooled object is pooled (instansiated)</summary>
    void OnInit();

    /// <summary>What to do when this pooled object is spawned</summary>
    void OnSpawn();

    /// <summary>What to do when this pooled object is killed</summary>
    void OnKill();
}