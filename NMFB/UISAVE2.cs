using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISAVE2 : MonoBehaviour
{
    public static UISAVE2 instance;

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
