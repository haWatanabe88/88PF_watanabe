using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : EnemyBase
{
    GameObject player;
    public BulletEnemy enemyBulletPrefab;
    GameObject enemyManagerObj;
    public BossMissile bossMissile;
    //EnemyManager enemyManager;
    BoxCollider2D boxCol2d;
    Vector3 bossMissile1;
    Vector3 bossMissile2;
    Vector3 bossMissile3;
    Vector3 bossMissile4;
    Vector3 bossMissile5;
    float xLimitMax;
    float xLimitMin;
    float yLimitMax;
    float yLimitMin;
    Vector3 targetPos;
    float t;
    bool isDicision = false;

    private void Start()
    {
        player = GameObject.Find("Player");
        enemyManagerObj = GameObject.Find("EnemyManager");
        boxCol2d = GetComponent<BoxCollider2D>();
        Setting();
        bossMissile1 = new Vector3(transform.position.x - 3.5f, transform.position.y + 3.0f, transform.position.z);
        bossMissile2 = new Vector3(transform.position.x - 3.5f, transform.position.y + 1.5f, transform.position.z);
        bossMissile3 = new Vector3(transform.position.x - 3.5f, transform.position.y, transform.position.z);
        bossMissile4 = new Vector3(transform.position.x - 3.5f, transform.position.y - 1.5f, transform.position.z);
        bossMissile5 = new Vector3(transform.position.x - 3.5f, transform.position.y - 3.0f, transform.position.z);
        xLimitMax = 8.4f;
        xLimitMin = -8.5f;
        yLimitMax = 3.7f;
        yLimitMin = -4.3f;
        moveTime = 5.0f;
    }

    void Update()
    {
        if (isMoving)
        {
            timer += Time.deltaTime;
            t = Mathf.Clamp01(timer / moveTime); // 0から1の範囲に正規化
            transform.position = Vector3.Lerp(startPos, endPos, t);
            if (t >= 1.0f)
            {
                isMoving = false;
                timer = 0;
                t = 0;
                boxCol2d.enabled = true;
                StartCoroutine("AttackLoop");
            }
        }
        //ボスが移動してから、攻撃を開始するようにする
        if(!isMoving)
        {
            if (!isDicision)
            {
                float posx = Random.Range(xLimitMin, xLimitMax);
                float posy = Random.Range(yLimitMin, yLimitMax);
                targetPos = new Vector3(posx, posy, 0);
                isDicision = true;
            }
            timer += Time.deltaTime;
            t = Mathf.Clamp01(timer / moveTime); //0から1の範囲に正規化
            transform.position = Vector3.Lerp(endPos, targetPos, t);
            if (t >= 1.0f)
            {
                endPos = targetPos;
                t = 0;
                timer = 0;
                isDicision = false;
            }
            if (this.hp <= 1000)
            {
                moveTime = 3.5f;
            }
            else if (this.hp <= 2500)
            {
                moveTime = 4.0f;
            }
        }
    }

    public override void Defeated()
    {
        /* 撃破時の処理 */
        Destroy(this.gameObject);
        Player.instance.DestroyEffect(this.transform.position);
        scoreScript.GetScoreConversion(this.score);
        Player.instance.isClear = true;
    }

    IEnumerator AttackLoop()
    {
        while (true)
        {
            yield return SurroundShot(3,8);
            yield return new WaitForSeconds(1f);
            yield return WaveNPlayerAimShot(4, 6);
            yield return new WaitForSeconds(1f);
            yield return WaveNPlayerAimShotMissile();
            yield return new WaitForSeconds(1f);
            yield return WaveNShotMSpiral(4, 200);
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator WaveNShotMSpiral(int n, int m)
    {
        yield return ShotSpiral(m, 3);
    }

    IEnumerator ShotSpiral(int count, float speed)
    {
        int bulletCount = count;
        float angle = 0f;
        for (int i = 0; i < bulletCount; i++)
        {
            angle += 0.3f;
            Shot(angle, speed);
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator SurroundShot(int n, int bulletNum)
    {
        for(int i = 0; i < n; i++)
        {
            yield return new WaitForSeconds(1f);
            ShotN(bulletNum, 2);
        }
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

    IEnumerator WaveNPlayerAimShot(int n, int m)
    {
        for (int i = 0; i < n; i++)
        {
            yield return new WaitForSeconds(1f);
            PlayerAimShot(m, 3);
        }
    }

    void PlayerAimShot(int count, float speed)
    {
        Vector3 diffPosition = player.transform.position - transform.position;
        float angleP = Mathf.Atan2(diffPosition.y, diffPosition.x);
        int bulletCount = count;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = (i - Mathf.Round(bulletCount / 2f)) * (Mathf.PI / bulletCount); // PI/2f：90
            Shot(angleP + angle, speed);
        }
    }

    IEnumerator WaveNPlayerAimShotMissile()
    {
        yield return null;
        SpawnMissile(bossMissile1);
        yield return new WaitForSeconds(0.5f);
        SpawnMissile(bossMissile2);
        yield return new WaitForSeconds(0.5f);
        SpawnMissile(bossMissile3);
        yield return new WaitForSeconds(0.5f);
        SpawnMissile(bossMissile4);
        yield return new WaitForSeconds(0.5f);
        SpawnMissile(bossMissile5);
    }

    void SpawnMissile(Vector3 position)
    {
        Instantiate(bossMissile, position, Quaternion.identity);
    }

    void Shot(float angle, float speed)
    {
        BulletEnemy bullet = Instantiate(enemyBulletPrefab, transform.position, transform.rotation);
        bullet.Prepare(angle, speed); // Mathf.PI/4fは45°
    }
}
