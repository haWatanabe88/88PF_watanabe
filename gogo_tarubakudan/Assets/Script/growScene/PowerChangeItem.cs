using UnityEngine;
using DG.Tweening;

/*
 カメラをインスペクターではめる
 */
public class PowerChangeItem : MonoBehaviour
{
    GameObject camera_obj;
    PlayerItemPickup player_item_pick_up;
    BoxCollider box_col;

    void Start()
    {
        GameObject player_obj = GameObject.FindWithTag("Player");
        player_item_pick_up = player_obj.GetComponent<PlayerItemPickup>();
        camera_obj = GameObject.FindWithTag("MainCamera");
    }

    void Update()
    {
        faceCamera();
    }

    void OnTriggerEnter(Collider other_collision)
    {
        if (other_collision.gameObject.CompareTag("Player"))
        {
            obtainItemAnimation(other_collision);
        }
    }


    void faceCamera()
    {
        Vector3 target = camera_obj.transform.position;
        target.y = transform.position.y;  //yの座標値は、そのアイテム自身と同じものにして、回転させないため
        transform.LookAt(target);
    }

    void obtainItemAnimation(Collider other_collision)
    {
        if (other_collision.gameObject.CompareTag("Player"))
        {
            //アニメーションが何度も発生しないように当たり判定を削除している
            box_col = this.gameObject.GetComponent<BoxCollider>();
            box_col.enabled = false;
            if (this.gameObject.CompareTag("mini_bomb_positive"))//もし自分がポジティブタグなら→取ったものが嬉しい：アニメーション
            {
                // カメラの回転方向を取得
                Quaternion cam_rotation = camera_obj.transform.rotation;
                Vector3[] local_path;

                if (player_item_pick_up.getObtainPositiveItemnum() % 2 == 1)// 奇数：右回り
                {
                    local_path = new[]
                    {
                        new Vector3(2.5f, 2f, 4f),
                        new Vector3(2f, 1f, 0f)
                    };
                }
                else// 偶数：左回り
                {
                    local_path = new[]
                    {
                        new Vector3(-2.5f, 2f, 4f),
                        new Vector3(-2f, 1f, 0f)
                    };
                }

                Vector3[] current_path = new Vector3[local_path.Length];
                for (int i = 0; i < local_path.Length; i++)
                {
                    current_path[i] = cam_rotation * local_path[i];// ワールド座標方向へ向けて回転
                }

                //OnWaypointChangeは、Tweenに直接作用させる必要があるので、ここでTweenPathを作る
                Tweener path_tween = transform.DOLocalPath(current_path, 0.3f, PathType.CatmullRom)
                    .SetOptions(true)
                    .SetEase(Ease.InOutQuart)
                    .SetRelative(true);

                path_tween.OnWaypointChange(index =>
                {
                    if (index == 0)
                    {
                        transform.DOScale(1.5f, 0.1f);
                    }
                    else if (index == 1)
                    {
                        transform.DOScale(0.1f, 0.1f);
                    }
                });

                Sequence seq = DOTween.Sequence();
                seq.Append(path_tween);//アニメーションの動きを登録しておくことで、それが完全に終わったら、デストロイするようにする
                seq.OnComplete(() => Destroy(gameObject));
            }
            else if (this.gameObject.CompareTag("mini_bomb_negative"))//ネガティブタグなら
            {
                Destroy(gameObject);
            }
        }
    }
}
