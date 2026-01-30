using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySquad : EnemyBase
{
    GameObject player;
    public BulletEnemy enemyBulletPrefab;
    [SerializeField] public int childCount;
    public GameObject squadChildObject;

    Vector3 squadChildPos1;
    Vector3 squadChildPos2;
    Vector3 squadChildPos3;
    Vector3 squadChildPos4;

    private void Start()
    {
        Setting();
        squadChildPos1 = new Vector3(transform.position.x - 1.0f, transform.position.y, transform.position.z);
        squadChildPos2 = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
        squadChildPos3 = new Vector3(transform.position.x + 1.0f, transform.position.y, transform.position.z);
        squadChildPos4 = new Vector3(transform.position.x, transform.position.y - 1.0f, transform.position.z);

        Instantiate(squadChildObject, squadChildPos1, Quaternion.identity);
        Instantiate(squadChildObject, squadChildPos2, Quaternion.identity);
        Instantiate(squadChildObject, squadChildPos3, Quaternion.identity);
        Instantiate(squadChildObject, squadChildPos4, Quaternion.identity);
        player = GameObject.Find("Player");

        moveTime = 1.0f;
        isMoving = true;

        childCount = 0;
        this.gameObject.tag = "Squad" + JsonLoader.instance.squadCount;
        StartCoroutine(AttackLoop());
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
        GoStraight();
    }

    IEnumerator AttackLoop()
    {
        while (true)
        {
            yield return SquadShot(3);
        }
    }

    IEnumerator SquadShot(int bulletNum)
    {
        yield return new WaitForSeconds(2.0f);
        PlayerAimSquadShot(bulletNum, 2);
    }

    void PlayerAimSquadShot(int count, float speed)
    {
        Vector3 diffPosition = player.transform.position - transform.position;
        float angleP = Mathf.Atan2(diffPosition.y, diffPosition.x);
        int bulletCount = count;

        for (int i = 1; i <= bulletCount; i++)
        {
            float angle = (i - Mathf.Round(bulletCount / 2f)) * (Mathf.PI / bulletCount); // PI/2f：90
            Shot(angleP + angle, speed);
        }
    }

    void Shot(float angle, float speed)
    {
        BulletEnemy bullet = Instantiate(enemyBulletPrefab, transform.position, transform.rotation);
        bullet.Prepare(angle, speed); // Mathf.PI/4fは45°
    }

    public override void GoStraight()
    {
        float speed = 1.0f;
        transform.position -= new Vector3(1f, 0, 0) * Time.deltaTime * speed;
        if (transform.position.x <= -15f)
        {
            Destroy(this.gameObject);
        }
    }

    public override void TakeDamage(int damage, Collider2D collision)
    {
        /* ダメージを受ける処理 */
        if(childCount >= 4)
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

    public void ChildCountUp()
    {
        childCount++;
    }
}
