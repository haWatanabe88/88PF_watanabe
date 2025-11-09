using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISAVE : MonoBehaviour
{
    public static UISAVE instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
