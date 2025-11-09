using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySquadChild : EnemyBase
{
    [SerializeField, Tooltip("ターゲットオブジェクト")]
    private GameObject TargetObject;
    //[SerializeField, Tooltip("回転軸")]
    private Vector3 RotateAxis = Vector3.forward;
    //[SerializeField, Tooltip("速度係数")]
    private float SpeedFactor = 1.5f;//調整する必要あり
    //[SerializeField, Tooltip("半径距離")]//調整する必要あり
    private float RadiusDistance = 1.4f;

    int targetSquadMainNum;

    private void Start()
    {
        targetSquadMainNum = JsonLoader.instance.squadCount;
        TargetObject = GameObject.FindWithTag("Squad" + targetSquadMainNum);
        this.hp = 100;//50~100以下
        GeneralAudioSrcObj = GameObject.Find("GeneralAudioSource");
        GeneralAudioSrc = GeneralAudioSrcObj.GetComponent<GeneralAudioSource>();
    }

    void Update()
    {
        if (TargetObject == null)
        {
            Destroy(this.gameObject);
        }

        // 指定オブジェクトと自身の現在位置を取得する
        Vector3 selfPosition = this.transform.position;
        Vector3 targetPosition = TargetObject.transform.position;

        // 座標が重なっていた場合は回転軸の直交方向に離れた場所を初期位置とする
        if (selfPosition.Equals(targetPosition))
        {
            // 直交ベクトルを求めるため、回転軸に平行でないダミーベクトルを作成する
            Vector3 rotateAxisNormal = RotateAxis.normalized;
            Vector3 dummyDirectVector = Vector3.forward;
            if (Mathf.Abs(rotateAxisNormal.y) < 0.5f) dummyDirectVector = Vector3.forward;

            // 回転軸とダミーベクトルから直交ベクトルを算出し、初期位置を設定する
            Vector3 directVector = Vector3.Cross(RotateAxis, dummyDirectVector).normalized;
            selfPosition = directVector * RadiusDistance;
        }

        // 軸方向の移動量は追従する
        Vector3 diffVector = selfPosition - targetPosition;
        float diffMagnitude = diffVector.magnitude;
        float dot = Vector3.Dot(diffVector, RotateAxis);
        // 回転軸との内積から回転軸方向への移動量を求める
        selfPosition -= RotateAxis.normalized * (diffMagnitude * dot);

        // 現在の距離と半径距離の差分を取得する
        float diffDistance = Vector3.Distance(selfPosition, targetPosition) - RadiusDistance;

        // 指定半径の距離になるよう近づく(or離れる)
        this.transform.position = Vector3.MoveTowards(selfPosition, targetPosition, diffDistance);

        // 指定オブジェクトを中心に回転する
        this.transform.RotateAround(
            targetPosition,
            RotateAxis,
            360.0f / (1.0f / SpeedFactor) * Time.deltaTime
            );
        if (transform.position.x <= -10f)
        {
            Destroy(this.gameObject);
        }
    }

    public override void Defeated()
    {
        /* 撃破時の処理 */
        TargetObject.GetComponent<EnemySquad>().ChildCountUp();//小隊の数をカウントアップし、全て撃破しないと倒せないという仕様のため
        Destroy(this.gameObject);
    }
}