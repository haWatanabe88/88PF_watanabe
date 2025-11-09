using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMissile : EnemyBase
{
    float interval;
    GameObject player;
    Vector3 currentPlayerPos;
    bool isStay;
    float count;
    private void Start()
    {
        GeneralAudioSrcObj = GameObject.Find("GeneralAudioSource");
        GeneralAudioSrc = GeneralAudioSrcObj.GetComponent<GeneralAudioSource>();
        player = GameObject.Find("Player");
        isStay = true;
        interval = 0;
        moveTime = 2.0f;
        this.hp = 100;
    }

    private void Update()
    {
        BossMissilePlayerAimAttack();
    }

    // Playerを狙う
    // ・Playerの位置取得
    void BossMissilePlayerAimAttack()
    {
        interval += Time.deltaTime;
        if (interval >= 2.0f)
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
                Destroy(this.gameObject);
            }
        }
        else if (interval >= 1.0f)//レベル調整次第、、、interval <= 2.0f　という場合にするとより簡単になる　もうちょっと2.5とかでもいいかも。。。
        {
            currentPlayerPos = player.transform.position;
        }
    }

    public override void TakeDamage(int damage, Collider2D collision)
    {
        /* ダメージを受ける処理 */
        this.hp -= damage;
        Destroy(collision.gameObject);
        if (this.hp <= 0)
        {
            Defeated();
        }
    }

    public override void Defeated()
    {
        Destroy(this.gameObject);
    }
}
