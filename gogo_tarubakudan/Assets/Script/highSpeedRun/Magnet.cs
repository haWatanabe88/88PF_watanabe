using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder;

public class MAGNET : MonoBehaviour
{
    [SerializeField] float ray_length;
    [SerializeField] float magnet_power;
    [SerializeField] float slide_move_power;
    Rigidbody rb;
    GameObject bullet;
    HighSpeedManager highSpeedManager;
    bool is_active_bullet;
    float input;
    private void Start()
    {
        is_active_bullet = false; 
        bullet = GameObject.FindWithTag("cannon_bullet");
        highSpeedManager = GameObject.FindWithTag("highSpeedManager").GetComponent<HighSpeedManager>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!is_active_bullet)
        {
            bullet = GameObject.FindWithTag("cannon_bullet");
        }
        //Debug.DrawRay(transform.position, -transform.up * ray_length, Color.red);
        if (bullet && highSpeedManager.is_game_start)
        {
            is_active_bullet = true;
            transform.position = new Vector3(transform.position.x, transform.position.y, bullet.transform.position.z);
        }
        destroyObject();
    }

    void FixedUpdate()
    {
        Vector3 inputVector = Vector3.zero;
        input = Input.GetAxis("Horizontal");
        if (input > 0)
        {
            inputVector = transform.right;
        }
        else
        {
            inputVector = -transform.right;
        }
        Debug.DrawRay(transform.position, inputVector * 5.0f, Color.green);
        Ray ray = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(ray, out RaycastHit hit, ray_length))
        {
            Vector3 wallNormal = hit.normal;
            Vector3 attractDir = -wallNormal;
            if (input != 0)
            {
                moveSlide(wallNormal, inputVector);//ê⁄ê¸ï˚å¸Ç÷à⁄ìÆÇ≥ÇπÇÈ
            }
            else
            {
                rb.linearVelocity = Vector3.zero;
            }
            adsorbWall(attractDir);//ï«Ç…ãzíÖÇ∑ÇÈóÕ
        }
    }

    void adsorbWall(Vector3 attractDir)
    {
        rb.AddForce(attractDir * magnet_power, ForceMode.Force);
    }


    void moveSlide(Vector3 wallNormal, Vector3 inputVector)
    {
        rb.AddForce(inputVector * slide_move_power, ForceMode.Acceleration);
    }

    void destroyObject()
    {
        if (transform.position.y <= -100f)
        {
            Destroy(gameObject);
        }
    }
}
