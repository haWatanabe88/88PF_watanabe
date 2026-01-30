using System.Collections;
using UnityEngine;
using UnityEngine.LowLevelPhysics2D;

public class BulletHighSpeedRun : MonoBehaviour
{
    GameObject magnet;
    float attract_force;
    float forward_power;
    [SerializeField] PlayerPhysicsDataSO physicsData;
    [SerializeField] GameObject target_point;
    [SerializeField] GameObject fire_point;
    float speed = 5f;

    Vector3 move_dir;
    Rigidbody rb;

    public bool is_start {get; private set;}

    bool is_magnet_se;

    public void setIsStart(bool val)
    {
        is_start = val;
    }
    void Start()
    {
        attract_force = 30f;
        if (physicsData.currentState == PlayerStatus.PlayerState.Small)
        {
            forward_power = 20f;
        }else if(physicsData.currentState == PlayerStatus.PlayerState.Middle)
        {
            forward_power = 30f;
        }
        else if(physicsData.currentState == PlayerStatus.PlayerState.Big)
        {
            forward_power = 50f;
        }
        setIsStart(false);
        magnet = GameObject.FindWithTag("magnet");
        transform.localScale = new Vector3(physicsData.scale_x, physicsData.scale_y, physicsData.scale_z);
        rb = GetComponent<Rigidbody>();
        rb.mass = physicsData.mass;
        fire_point = GameObject.FindWithTag("fire_point");
        target_point = GameObject.FindWithTag("target_point");
        is_magnet_se = false;
    }

    private void FixedUpdate()
    {
        if(is_start) 
        {
            moveForward();//前進させる　＊弾を前進させることで、マグネットを追従させる
            attractForce();//マグネットのある方に近づく
        }
    }

    private void Update()
    {
        if (!is_start)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target_point.transform.position, step);
        }
        else
        {
            destroyObject();
        }
    }

    void moveForward()
    {
        rb.AddForce(transform.forward * forward_power, ForceMode.Force);
    }

    void attractForce()
    {
        if (magnet)
        {
            Vector3 force_dir = (magnet.transform.position - transform.position).normalized;

            if (Input.GetKey(KeyCode.Space))
            {
                if (!is_magnet_se)
                {
                    is_magnet_se = true;
                    SoundManager.Instance.PlaySE("attract_hs");
                }
                rb.AddForce(force_dir * attract_force, ForceMode.Acceleration);
            }else if (Input.GetKeyUp(KeyCode.Space))
            {
                is_magnet_se = false;
            }
        }
    }
    public void aceleration()
    {
        Debug.Log("加速しました");
        float current_forward_power = forward_power;
        forward_power += 6f;
        StartCoroutine(returnOrgForwardPower(current_forward_power));
    }


    IEnumerator returnOrgForwardPower(float current_forward_power)
    {
        Debug.Log("cur:" + current_forward_power);
        while (forward_power > current_forward_power)
        {
            Debug.Log("for:" + forward_power);
            yield return new WaitForSeconds(0.5f);
            forward_power -= 2f;
        }
        Debug.Log("元に戻りました");
    }

    void destroyObject()
    {
        if(transform.position.y <= -100f)
        {
            Destroy(gameObject);
        }
    }

}