using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWindowManager : MonoBehaviour
{
    public static CloseWindowManager Instance { get; private set; }
    /// <summary>
    /// ƒVƒ“ƒOƒ‹ƒgƒ“ˆ—
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
