using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manager<T> : MonoBehaviour where T : Manager<T>
{
    public static T Instance { get; private set; }

    [SerializeField]
    bool isPersistant = false;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this as T;

            if (isPersistant)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
