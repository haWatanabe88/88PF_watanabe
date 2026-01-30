using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float move_speed = 0.1f;
    public float rotation_speed = 100f;
    [SerializeField] PlayerPhysicsDataSO physicsData;
    [SerializeField] GameObject explosionPrefab;
    Transform player;
    PirateShip pirate_ship;
    Vector3 rotation_axis;
    Vector3 adjust_height = new Vector3(0f, 2f, 0f);

    void Start()
    {
        player = GameObject.FindWithTag("cannon").transform;
        pirate_ship = GameObject.FindWithTag("pirate_ship").GetComponent<PirateShip>();
        rotation_axis = Random.onUnitSphere; // 回転軸をランダムで決める
    }

    void Update()
    {
        move();
        inActive();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("cannon_bullet"))
        {
            SoundManager.Instance.PlaySE("obstacleAttack_pi");
            Vector3 effect_scale = explosionPrefab.transform.localScale;
            effect_scale.x = 1f;
            effect_scale.y = 1f;
            effect_scale.z = 1f;
            explosionPrefab.transform.localScale = effect_scale;
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            if (physicsData.currentState != PlayerStatus.PlayerState.Big)
                Destroy(collider.gameObject);
            Destroy(this.gameObject);
        }
    }

    void move()
    {
        // プレイヤーへ向かって進む
        Vector3 moveDir = (player.position + adjust_height - transform.position).normalized;
        transform.position += moveDir * move_speed * Time.deltaTime;

        // 自分軸で回転
        transform.Rotate(rotation_axis * rotation_speed * Time.deltaTime);
    }

    void inActive()
    {
        if (pirate_ship.getIsDefeat())
            this.gameObject.SetActive(false);
    }
}
