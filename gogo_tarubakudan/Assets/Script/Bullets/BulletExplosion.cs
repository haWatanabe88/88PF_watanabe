using System.Collections;
using UnityEngine;

//explosionシーンの弾

public class BulletExplosion : MonoBehaviour
{
    CCExplosion cc_explosion;
    CannonMoveAuto canon_move_auto;
    ExplosionManager explosion_manager;
    [SerializeField] PlayerPhysicsDataSO playerPhysicsDataSO;
    [SerializeField] GameObject explosionPrefab;
    float explosionForce;
    float explosionRadius;
    float upwardsModifier;
    bool is_touch;


    private void Start()
    {
        cc_explosion = GameObject.FindWithTag("CC").GetComponent<CCExplosion>();
        canon_move_auto = GameObject.FindWithTag("cannon").GetComponent <CannonMoveAuto>();
        explosion_manager = GameObject.FindWithTag("explosionManager").GetComponent<ExplosionManager>();
        switch (playerPhysicsDataSO.currentState)
        {
            case PlayerStatus.PlayerState.Small:
                adjustBulletScale(1f);
                explosionForce = 10f;
                explosionRadius = 10f;
                upwardsModifier = 2f;
                break;
            case PlayerStatus.PlayerState.Middle:
                adjustBulletScale(5f);
                explosionForce = 30f;
                explosionRadius = 20f;
                upwardsModifier = 5f;
                break;
            case PlayerStatus.PlayerState.Big:
                adjustBulletScale(10f);
                explosionForce = 100f;
                explosionRadius = 30f;
                upwardsModifier = 10f;
                break;
        }
    }

    private void Update()
    {
        if(transform.position.y <= -50f)//決め打ち
        {
            StartCoroutine(retryFlow());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!is_touch && (collision.gameObject.CompareTag("ground") || collision.gameObject.CompareTag("enemy")))
        {
            SoundManager.Instance.PlaySE("bakuhatu_ex");
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            is_touch = true;
            Debug.Log("touch");
            MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
            foreach (var r in renderers)
            {
                r.enabled = false; // 非表示
            }
            StartCoroutine(retryFlow());
            explode();
        }
    }

    IEnumerator retryFlow()
    {
        yield return new WaitForSeconds(3.5f);
        GameObject[] defeatEnemies = GameObject.FindGameObjectsWithTag("defeatEnemy"); //タグのついたオブジェクトを全て検索して配列にいれる
        if (defeatEnemies.Length != 0)
        {
            foreach (GameObject enemy in defeatEnemies)
            {
                Destroy(enemy);
            }
        }
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy"); //タグのついたオブジェクトを全て検索して配列にいれる
        if (enemies.Length == 0)
        {
            explosion_manager.informGameOver();
        }
        cc_explosion.setIsTrackDisnable(false);
        canon_move_auto.initialize();
        explosion_manager.hideBulletIcon();
        Destroy(gameObject);
    }

    void explode()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy"); //タグのついたオブジェクトを全て検索して配列にいれる

        if (enemies.Length == 0) return; // タグがついたオブジェクトがなければ何もしない。

        foreach (GameObject enemy in enemies) // 配列に入れた一つひとつのオブジェクト
        {
            if (enemy.GetComponent<Rigidbody>()) // Rigidbodyがあれば、グレネードを中心とした爆発の力を加える
            {
                enemy.GetComponent<Rigidbody>().AddExplosionForce(explosionForce//爆発力
                                                                , transform.position//爆発の中心点
                                                                , explosionRadius//爆発半径
                                                                , upwardsModifier//オブジェクトを持ち上げる力（演出）
                                                                , ForceMode.Impulse);//適用する力
            }
        }
    }

    void adjustBulletScale(float value)
    {
        Vector3 effect_scale = explosionPrefab.transform.localScale;
        effect_scale.x = value;
        effect_scale.y = value;
        effect_scale.z = value;
        explosionPrefab.transform.localScale = effect_scale;
    }
}
