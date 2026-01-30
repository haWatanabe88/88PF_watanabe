using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHidden : EnemyBase
{
    GameObject player;
    public BulletEnemy enemyBulletPrefab;
    private float bulletInterval;
    private float appearanceTime;


    private void Start()
    {
        bulletInterval = 3.0f;
        appearanceTime = 5.0f;
        //transform.position = startPos;
        player = GameObject.Find("Player");
        Setting();
        moveTime = 2.0f;
    }

    private void Update()
    {
        Move();
    }

    public override void Move()
    {
        // isMovingがtrueの間に移動を行う
        if (isMoving)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / moveTime); // 0から1の範囲に正規化

            // startPosからendPosに向かって補間する
            transform.position = Vector3.Lerp(startPos, endPos, t);

            // もし移動が完了したら、isMovingをfalseにする
            if (t >= 1.0f)
            {
                isMoving = false;
            }
        }
        else
        {
            //移動後、数秒間、プレイヤーに向かって攻撃して、一定時間ご、Gostraightでさる
            appearanceTime -= Time.deltaTime;
            if(appearanceTime >= 0)
            {
                PlayerAimShot(3, 7.0f);
            }
            else
            {
                GoStraight();
            }
        }
    }

    void PlayerAimShot(int count, float speed)
    {
        bulletInterval += Time.deltaTime;
        if(bulletInterval >= 3.0f)
        {
            bulletInterval = 0;
            // 自分からみたPlayerの位置を計算する
            Vector3 diffPosition = player.transform.position - transform.position;
            // 自分から見たPlayerの角度を出す：傾きから角度を出す：アークタンジェントを使う
            float angleP = Mathf.Atan2(diffPosition.y, diffPosition.x);

            int bulletCount = count;
            for (int i = 0; i < bulletCount; i++)
            {
                Shot(angleP, speed);
            }
        }
    }

    void Shot(float angle, float speed)
    {
        BulletEnemy bullet = Instantiate(enemyBulletPrefab, transform.position, transform.rotation);
        bullet.Prepare(angle, speed); // Mathf.PI/4fは45°
    }
}
