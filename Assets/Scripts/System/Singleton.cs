using System;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject(typeof(T).Name);
                instance = go.AddComponent<T>();
                DontDestroyOnLoad(go);
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = (T)this;
        }
        else
        {
            DestroyImmediate(this.gameObject);
        }
    }
}


public class EventManager<T, C> : Singleton<C> where C : EventManager<T, C>
{
    public Dictionary<T, Action<object>> Events = new Dictionary<T, Action<object>>();

    public void AddEvent(T type, Action<object> add)
    {
        if (!Events.ContainsKey(type))
        {
            Events.Add(type, add);
        }
        else
        {
            Events[type] += add;
        }
    }

    public void RemoveEvent(T type, Action<object> remove)
    {
        if (Events.ContainsKey(type))
        {
            Events[type] -= remove;
        }
    }

    public void SendEvent(T type, object value)
    {
        Events[type](value);
    }
}
