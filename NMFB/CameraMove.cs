using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public static CameraMove instance;

    private void Awake()
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


    // Update is called once per frame
    public float cameraSpeed = 1.0f;
    void Update()
    {
        //transform.position += new Vector3(cameraSpeed, 0, 0) * Time.deltaTime;
    }
}
