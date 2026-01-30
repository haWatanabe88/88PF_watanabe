using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder;

public class TESTMAGNET : MonoBehaviour
{
    [SerializeField] float ray_length;
    [SerializeField] float magnet_power;
    [SerializeField] float slide_move_power;
    //[SerializeField] GameObject bullet;
    //[SerializeField] GameObject camera_centerPos;
    Rigidbody rb;

    float input;
    private void Start()
    {
        //bullet = GameObject.FindWithTag("cannon_bullet");
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, -transform.up * ray_length, Color.red);

        //transform.position = new Vector3(transform.position.x, transform.position.y, bullet.transform.position.z);

    }

    void FixedUpdate()
    {
        Vector3 inputVector = Vector3.zero;
        input = Input.GetAxis("Horizontal");
        if(input > 0)
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
            if(input != 0)
            {
                moveSlide(wallNormal, inputVector);//ê⁄ê¸ï˚å¸Ç÷à⁄ìÆÇ≥ÇπÇÈ
            }
            adsorbWall(attractDir);//ï«Ç…ãzíÖÇ∑ÇÈóÕ
            
            
            //Vector3 onPlane = Vector3.ProjectOnPlane(inputVector, wallNormal);
            //Debug.DrawRay(hit.point, wallNormal * 5.0f, Color.green);
            //    Vector3 forward = transform.forward;
            //    //Vector3 forward = transform.forward.normalized;
            //    Vector3 tangent = Vector3.ProjectOnPlane(forward, wallNormal).normalized;
            //    Debug.DrawRay(hit.point, tangent * 5.0f, Color.blue);
        }
    }

    void adsorbWall(Vector3 attractDir)
    {
        rb.AddForce(attractDir * magnet_power, ForceMode.Force);
    }



    void moveSlide(Vector3 wallNormal, Vector3 inputVector)
    {
        rb.AddForce(inputVector * slide_move_power, ForceMode.Acceleration);

        //Vector3 forward = transform.forward.normalized;
        //Vector3 tangent = Vector3.Cross(wallNormal, forward).normalized;


        //rb.AddForce(tangent * input * slide_move_power, ForceMode.Acceleration);
        //rb.linearVelocity = Vector3.Project(rb.linearVelocity, tangent);




        //Vector3 v = rb.linearVelocity.normalized;
        //Vector3 forward_component = Vector3.Project(v, forward);
        //rb.linearVelocity = v - forward_component;

        //Rigidbody bullet_rb = bullet.GetComponent<Rigidbody>();
        //bullet_rb.linearVelocity.normalized;
        //Vector3 tangent = Vector3.ProjectOnPlane(forward, wallNormal).normalized;

        //Vector3 vel = bullet_rb.linearVelocity;
        // ê⁄ê¸ï˚å¸ê¨ï™ÇæÇØÇéÊÇËèoÇ∑
        //Vector3 tangentVel = Vector3.Project(vel, tangent);
        //rb.AddForce(Vector3.left * input * slide_move_power, ForceMode.Acceleration);
    }
}
