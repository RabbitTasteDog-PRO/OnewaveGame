using UnityEngine;
using System.Collections;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{

    private static T _instance = null;

    /// <summary>
    /// gets the instance of this Singleton
    /// use this for all instance calls:
    /// MyClass.Instance.MyMethod();
    /// or make your public methods static
    /// and have them use Instance
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (T)FindObjectOfType(typeof(T));
                if (_instance == null)
                {
                    string goName = typeof(T).ToString();
                    GameObject go = GameObject.Find(goName);
                    if (go == null)
                    {
                        go = new GameObject();
                        go.name = goName;
                    }
                    _instance = go.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// for garbage collection
    /// </summary>
    public virtual void OnApplicationQuit()
    {
        // release reference on exit
        _instance = null;
    }



}
