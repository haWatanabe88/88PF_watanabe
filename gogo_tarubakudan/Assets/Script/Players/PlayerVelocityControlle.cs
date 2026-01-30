using UnityEngine;

public class PlayerVelocityControlle : MonoBehaviour
{

    Rigidbody rb;
    bool is_zero;
    void Start()
    {
        is_zero = false;
        rb = GetComponent<Rigidbody>();
    }

    public void velosityZero()
    {
        if (!is_zero)
        {
            is_zero = true;
            rb.linearVelocity = Vector3.zero;
        }
    }
}
