using UnityEngine;
using UnityEngine.LowLevelPhysics2D;

public class BulletPirate : MonoBehaviour
{
    GameObject fire_point;
    float move_force;
    [SerializeField] float move_force_up_coef;
    [SerializeField] PlayerPhysicsDataSO physicsData;
    [SerializeField] GameObject explosionPrefab;
    Rigidbody rb;


    int SMALL_ATTACK_POWER = 2;
    int MIDDLE_ATTACK_POWER = 10;
    int BIG_ATTACK_POWER = 30;
    int attack_power;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = physicsData.mass;
        rb.linearDamping = physicsData.linier_damping;
        parameterInitialization();
    }

    void Start()
    {
        Vector3 effect_scale = explosionPrefab.transform.localScale;
        effect_scale.x = 2f;
        effect_scale.y = 2f;
        effect_scale.z = 2f;
        explosionPrefab.transform.localScale = effect_scale;
        fire_point = GameObject.FindWithTag("fire_point");
        rb = GetComponent<Rigidbody>();
        Vector3 move_dir = fire_point.transform.forward + fire_point.transform.up * move_force_up_coef;
        Vector3 force = move_dir * move_force;
        rb.AddForce(force, ForceMode.Impulse);
    }

    void Update()
    {
        if(this.transform.position.y <= -10f)//‚Æ‚è‚ ‚¦‚¸Œˆ‚ß‘Å‚¿
        {
            Destroy(this.gameObject);
        }
    }

    void parameterInitialization()
    {
        transform.localScale = new Vector3(physicsData.scale_x, physicsData.scale_y, physicsData.scale_z);
        if (physicsData.currentState == PlayerStatus.PlayerState.Small)
        {
            move_force = 45f;
            move_force_up_coef = 0.1f;
            attack_power = SMALL_ATTACK_POWER;
        }
        else if (physicsData.currentState == PlayerStatus.PlayerState.Middle)
        {
            move_force = 60f;
            move_force_up_coef = 0.1f;
            attack_power = MIDDLE_ATTACK_POWER;
        }
        else if (physicsData.currentState == PlayerStatus.PlayerState.Big)
        {
            move_force = 80f;
            move_force_up_coef = 0.1f;
            attack_power = BIG_ATTACK_POWER;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        attack(collider);
        hitGround(collider);
    }

    void attack(Collider collider)
    {
        if (collider.gameObject.CompareTag("pirate_ship"))
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            collider.gameObject.GetComponent<PirateShip>().damageByBullet(attack_power);
            Destroy(gameObject);
        }
    }

    void hitGround(Collider collider)
    {
        if (collider.gameObject.CompareTag("ground"))
        {
            Destroy(gameObject);
        }
    }


}
