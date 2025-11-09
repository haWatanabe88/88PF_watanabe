using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissile : EnemyBase
{
    float interval;
    GameObject player;
    Vector3 currentPlayerPos;
    bool isStay;
    float count;
    private void Start()
    {
        interval = 0;
        moveTime = 2.0f;
        count = 0.0f;
        isStay = false;
        player = GameObject.Find("Player");
        Setting();
    }

    private void Update()
    {
        Move();
        PlayerAimAttack();
    }

    public override void Move()
    {
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
                isStay = true;
            }
        }
    }

    // Playerを狙う
    // ・Playerの位置取得
    void PlayerAimAttack()
    {
        interval += Time.deltaTime;
        if (interval >= 3.0f)
        {
            if (isStay)
            {
                count += Time.deltaTime;
                float t = Mathf.Clamp01(count / moveTime); // 0から1の範囲に正規化
                transform.position = Vector3.Lerp(transform.position, currentPlayerPos, t);
                if (t >= 1)
                {
                    isStay = false;
                }
            }
            else
            {
                GoStraight();
            }
        }
        else if(interval >= 2.0f)//レベル調整次第、、、interval <= 2.0f　という場合にするとより簡単になる　もうちょっと2.5とかでもいいかも。。。
        {
            currentPlayerPos = player.transform.position;
        }
    }
}
