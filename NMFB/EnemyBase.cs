using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    //共通の変数
    public int hp;
    public int score;
    public float speed;
    public int scoreValue;
    public float moveTime;
    public float timer = 0f;
    public bool isMoving = true;
    public Vector3 startPos;
    public Vector3 endPos;
    public Score scoreScript;
    public GameObject GeneralAudioSrcObj;
    public GeneralAudioSource GeneralAudioSrc;

    // 共通の動作や機能を定義
    protected void Setting()
    {
        GeneralAudioSrcObj = GameObject.Find("GeneralAudioSource");
        GeneralAudioSrc = GeneralAudioSrcObj.GetComponent<GeneralAudioSource>();
        scoreScript = GameObject.Find("Score").GetComponent<Score>();
        this.hp = JsonLoader.instance.hp;
        this.score = JsonLoader.instance.score;
        startPos = new Vector3(JsonLoader.instance.startPosX, JsonLoader.instance.startPosY, 0);
        endPos = new Vector3(JsonLoader.instance.endPosX, JsonLoader.instance.endPosY, 0);
        transform.position = startPos;
    }

    public virtual void Move(){ /* 移動のロジック */ }
    public virtual void Attack() { /* 攻撃のロジック */ }
    public virtual void TakeDamage(int damage, Collider2D collision)
    {
        /* ダメージを受ける処理 */
        this.hp -= damage;
        Destroy(collision.gameObject);
        if(this.hp <= 0)
        {
            Defeated();
        }
    }
    public virtual void Defeated()
    {
        /* 撃破時の処理 */
        Destroy(this.gameObject);
        Player.instance.DestroyEffect(this.transform.position);
        scoreScript.GetScoreConversion(this.score);
    }
    public virtual void ForHitSE()
    {
        GeneralAudioSrc.HitSEFunc();
    }
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            TakeDamage(Player.instance.bulletDamage, collision);
            ForHitSE();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            if(!Player.instance.isHit)
            {
                if (Player.instance.currentMode == Player.Mode.FAT)//FATでヒットしているので、１機失う
                {
                    Player.instance.StartCoroutine("DamageBlink");
                }
                else if (Player.instance.currentMode == Player.Mode.MUSCLE)//MUSCLEでヒットしているので、ファットモードになる
                {
                    Player.instance.currentMode = Player.Mode.FAT;
                }
            }
        }
    }
    public virtual void GoStraight()
    {
        float speed = 3.0f;
        transform.position -= new Vector3(1f, 0, 0) * Time.deltaTime * speed;
        if (transform.position.x <= -10f)
        {
            Destroy(this.gameObject);
        }
    }
}
