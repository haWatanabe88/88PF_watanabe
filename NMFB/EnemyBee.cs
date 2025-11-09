using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBee : EnemyBase
{
    GameObject player;
    public BulletEnemy enemyBulletPrefab;
    private float bulletInterval;
    float fireTime = 0f;
    bool isAttack;
    bool isMove;
    float beemovet;
    float appearanceTime;
    Vector3 currentPos;
    Vector3 nextPos;
    Vector3 firstPos;
    Vector3 secondPos;
    Vector3 thirdPos;
    int moveNum = 0;
    private void Start()
    {
        player = GameObject.Find("Player");
        Setting();
        beemovet = 0;
        moveTime = 1.0f;
        moveNum = 0;
        bulletInterval = 1.0f;
        appearanceTime = 15.0f;
        isAttack = false;
        isMove = true;
        currentPos = endPos;
        firstPos = currentPos;
        secondPos = firstPos + new Vector3(0, -5.0f, 0);
        thirdPos = firstPos + new Vector3(3.0f, -5.0f / 2, 0);
    }

    private void Update()
    {
        if(isMove)
        {
            Move();
        }
        else if(!isMove)
        {
            PlayerAimShot(5, 3f);
        }
    }

    public override void Move()
    {
        if (isMoving)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / moveTime); // 0から1の範囲に正規化
            transform.position = Vector3.Lerp(startPos, endPos, t);
            if (t >= 1.0f)
            {
                isMoving = false;
                timer = 0;
            }
        }
        else
        {
            appearanceTime -= Time.deltaTime;
            if(appearanceTime >= 0)
            {
                timer += Time.deltaTime;
                isAttack = false;
                if (moveNum != 4)
                {
                    if (currentPos == firstPos)
                    {
                        nextPos = secondPos;
                        beemovet = Mathf.Clamp01(timer / moveTime);
                        transform.position = Vector3.Lerp(currentPos, nextPos, beemovet);
                        if (beemovet >= 1.0f)
                        {
                            beemovet = 0;
                            timer = 0;
                            currentPos = secondPos;
                            moveNum++;
                        }
                    }
                    else if (currentPos == secondPos)
                    {
                        nextPos = thirdPos;
                        beemovet = Mathf.Clamp01(timer / moveTime);
                        transform.position = Vector3.Lerp(currentPos, nextPos, beemovet);
                        if (beemovet >= 1.0f)
                        {
                            beemovet = 0;
                            timer = 0;
                            currentPos = thirdPos;
                            moveNum++;

                        }
                    }
                    else if (currentPos == thirdPos)
                    {
                        nextPos = firstPos;
                        beemovet = Mathf.Clamp01(timer / moveTime);
                        transform.position = Vector3.Lerp(currentPos, nextPos, beemovet);
                        if (beemovet >= 1.0f)
                        {
                            beemovet = 0;
                            timer = 0;
                            currentPos = firstPos;
                            moveNum++;
                        }
                    }
                }
                else
                {
                    fireTime = 0;
                    isMove = false;
                    isAttack = true;
                }
            }
            else
            {
                GoStraight();
            }
            
        }
    }

    void PlayerAimShot(int count, float speed)
    {
        fireTime += Time.deltaTime;
        bulletInterval += Time.deltaTime;
        if (bulletInterval >= 1.0f)
        {
            bulletInterval = 0;
            Vector3 diffPosition = player.transform.position - transform.position;
            float angleP = Mathf.Atan2(diffPosition.y, diffPosition.x);
            int bulletCount = count;

            for (int i = 0; i < bulletCount; i++)
            {
                float angle = (i - Mathf.Round(bulletCount / 2f)) * (Mathf.PI / bulletCount); // PI/2f：90
                Shot(angleP + angle, speed);
            }
        }
        if (fireTime >= 3.0f)
        {
            bulletInterval = 1.0f;
            moveNum = 0;
            isMove = true;
        }
    }

    void Shot(float angle, float speed)
    {
        BulletEnemy bullet = Instantiate(enemyBulletPrefab, transform.position, transform.rotation);
        bullet.Prepare(angle, speed); // Mathf.PI/4fは45°
    }

    public override void TakeDamage(int damage, Collider2D collision)
    {
        //Beeが攻撃している時だけ、ダメージ判定が出るようにするためにboolを設定
        if(isAttack)
        {
            this.hp -= damage;
            Destroy(collision.gameObject);
            if (this.hp <= 0)
            {
                Defeated();
            }
        }
        else
        {
            GeneralAudioSrc.ProtectedSEFunc();
            Destroy(collision.gameObject);
        }
    }

    public override void GoStraight()
    {
        float speed = 10.0f;
        transform.position -= new Vector3(1f, 0, 0) * Time.deltaTime * speed;
        if (transform.position.x <= -15f)
        {
            Destroy(this.gameObject);
        }
    }
}
