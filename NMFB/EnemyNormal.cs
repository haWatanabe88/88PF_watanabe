using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNormal : EnemyBase
{
    private void Start()
    {
        Setting();
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
            GoStraight();
        }
    }
}
