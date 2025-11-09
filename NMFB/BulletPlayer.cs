using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlayer : MonoBehaviour
{
    void Update()
    {
        transform.position += new Vector3(1f, 0, 0) * Time.deltaTime * Player.instance.bulletSpeed;
        if(transform.position.x >= Player.instance.xLimitMax + 5.0f)
        {
            Destroy(this.gameObject);
        }
    }
}
