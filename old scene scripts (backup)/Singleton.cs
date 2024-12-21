using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance { get { return instance; } }

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this as T;
        DontDestroyOnLoad(gameObject);
    }



    /*
    private static T instance;
    public static T Instance { get { return instance; }}

    protected virtual void Awake() 
    {
        if (instance != null && this.gameObject != null) 
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = (T)this;
        }

        DontDestroyOnLoad(gameObject);
    } 

    /*
    private static T instance;
    public static T Instance { get { return instance; }}


    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject); // Opcional: Preserve a instância ao carregar novas cenas
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Destrói o novo GameObject se já existir uma instância
        }
    }



*/



}
