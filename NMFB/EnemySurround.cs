using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySurround : EnemyBase
{
    public BulletEnemy enemyBulletPrefab;

    private void Start()
    {
        Setting();
        StartCoroutine(AttackLoop());
    }

    private void Update()
    {
        Move();
    }

    public override void Move()
    {
        if (isMoving)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / moveTime);

            transform.position = Vector3.Lerp(startPos, endPos, t);

            if (t >= 1.0f)
            {
                isMoving = false;
            }
        }
        GoStraight();
    }

    IEnumerator AttackLoop()
    {
        while (true)
        {
            yield return SurroundShot(8);
        }
    }

    IEnumerator SurroundShot(int bulletNum)
    {
        yield return new WaitForSeconds(2.5f);
        ShotN(bulletNum, 2);
    }

    void ShotN(int count, float speed)
    {
        int bulletCount = count;
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * (2 * Mathf.PI / bulletCount); // 2PI：360
            Shot(angle, speed);
        }
    }

    void Shot(float angle, float speed)
    {
        BulletEnemy bullet = Instantiate(enemyBulletPrefab, transform.position, transform.rotation);
        bullet.Prepare(angle, speed); // Mathf.PI/4fは45°
    }

    public override void GoStraight()
    {
        float speed = 0.5f;
        transform.position -= new Vector3(1f, 0, 0) * Time.deltaTime * speed;
        if (transform.position.x <= -10f)
        {
            Destroy(this.gameObject);
        }
    }


}
