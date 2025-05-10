using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentCanvas : MonoBehaviour
{
    private static PersistentCanvas _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Ya existe un Canvas persistente, destruye este duplicado
            Destroy(gameObject);
        }
    }
}
