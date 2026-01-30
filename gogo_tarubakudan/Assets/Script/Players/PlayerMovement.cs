using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject camera_obj;
    Rigidbody rb;
    public float moveForce { get; private set; }
    float rollTorque = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveForce = 10f;
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        //SoundManager.Instance.PlaySE("spinBall_grow");
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 camera_foward = camera_obj.transform.forward;
        Vector3 camera_right = camera_obj.transform.right;

        //è„ï˚å¸ÇÃâeãøÇÇ»Ç≠Ç∑ÇΩÇﬂ
        camera_foward.y = 0;
        camera_right.y = 0;
        camera_foward.Normalize();
        camera_right.Normalize();

        Vector3 moveDir = camera_foward * v + camera_right * h;

        // ì¸óÕÇ»ÇµÇÃéûÅAí‚é~Ç≥ÇπÇÈ
        if (Mathf.Approximately(h, 0f) && Mathf.Approximately(v, 0f))
            return;

        //moveDirÇ≈åàÇﬂÇΩï˚å¸Ç÷óÕÇâ¡Ç¶ÇÈ
        Vector3 force = moveDir * moveForce;
        rb.AddForce(force, ForceMode.Force);
        Vector3 torqueAxis = Vector3.Cross(Vector3.up, moveDir);
        rb.AddTorque(torqueAxis * rollTorque, ForceMode.Force);
    }

    public void setMoveForce(float val)
    {
        moveForce = val;
    }
}
