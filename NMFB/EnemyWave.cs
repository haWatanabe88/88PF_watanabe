using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave : EnemyBase
{
    float offset;

    void Start()
    {
        Setting();
        offset = 2f * Mathf.PI;
    }

    void Update()
    {
        Move();
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
            }
        }
        transform.position -= new Vector3(
                Time.deltaTime,
                Mathf.Cos(Time.frameCount * 0.05f + offset) * 0.1f,
                0);
        if (transform.position.x <= -10f)
        {
            Destroy(this.gameObject);
        }
    }
}
