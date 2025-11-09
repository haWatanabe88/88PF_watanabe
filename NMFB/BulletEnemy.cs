using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    // 設定された方向に弾を移動させる
    float dx;
    float dy;
    float adjust;
    GameObject scoreObj;
    Score scoreScript;

    private void Start()
    {
        adjust = 2.0f;
    }

    private void Update()
    {
        transform.position += new Vector3(dx, dy, 0) * Time.deltaTime;
        Vanish();
    }
    public void Prepare(float angle, float speed)
    {
        // 敵の右側が0°
        // 反時計回りに角度は増える

        // 2PIが360°
        // PIが180°
        // PI/2が90°

        dx = Mathf.Cos(angle) * speed;
        dy = Mathf.Sin(angle) * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if (!Player.instance.isHit)
            {

                if (Player.instance.currentMode == Player.Mode.FAT)//FATでヒットしているので、１機失う
                {
                    Player.instance.StartCoroutine("DamageBlink");
                    scoreObj = GameObject.Find("Score");
                }
                else if (Player.instance.currentMode == Player.Mode.MUSCLE)//MUSCLEでヒットしているので、ファットモードになる
                {
                    Player.instance.currentMode = Player.Mode.FAT;
                }
            }
        }
    }

    private void Vanish()
    {
        if(transform.position.x <= Player.instance.xLimitMin - adjust
            || transform.position.x >= Player.instance.xLimitMax + adjust
            || transform.position.y <= Player.instance.yLimitMin - adjust
            || transform.position.y >= Player.instance.yLimitMax + adjust)
        {
            Destroy(this.gameObject);
        }
    }
}
