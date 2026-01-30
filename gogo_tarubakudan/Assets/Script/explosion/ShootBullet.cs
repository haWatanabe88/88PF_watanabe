using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    /// <summary>
    /// 弾のPrefab
    /// </summary>
    [SerializeField, Tooltip("弾のPrefab")]
    private GameObject bulletPrefab;

    /// <summary>
    /// 砲身のオブジェクト
    /// </summary>
    [SerializeField, Tooltip("砲身のオブジェクト")]
    private GameObject barrelObject;

    /// <summary>
    /// 弾を生成する位置情報
    /// </summary>
    private Vector3 instantiatePosition;
    /// <summary>
    /// 弾の生成座標(読み取り専用)
    /// </summary>
    public Vector3 InstantiatePosition
    {
        get { return instantiatePosition; }
    }

    /// <summary>
    /// 弾の速さ
    /// </summary>
    [SerializeField, Range(20F, 30.0F), Tooltip("弾の射出する速さ")]
    private float speed = 25F;

    /// <summary>
    /// 弾の初速度
    /// </summary>
    private Vector3 shootVelocity;
    /// <summary>
    /// 弾の初速度(読み取り専用)
    /// </summary>
    public Vector3 ShootVelocity
    {
        get { return shootVelocity; }
    }

    void Update()
    {
        // 弾の初速度を更新
        shootVelocity = barrelObject.transform.forward * speed + barrelObject.transform.up;

        // 弾の生成座標を更新
        instantiatePosition = barrelObject.transform.position;
    }

    public void fireBullet()
    {
        // 弾を生成して飛ばす
        SoundManager.Instance.PlaySE("shot_ex");
        GameObject obj = Instantiate(bulletPrefab, instantiatePosition, Quaternion.identity);
        Rigidbody rid = obj.GetComponent<Rigidbody>();
        rid.AddForce(shootVelocity * rid.mass, ForceMode.Impulse);
    }
}

