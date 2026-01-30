using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG : MonoBehaviour
{
    float speed = 0.5f;

    private void Update()
    {
        if(transform.position.x >= -21.0)
        {
            transform.position -= new Vector3(1f, 0, 0) * Time.deltaTime * speed;
        }
    }
}
