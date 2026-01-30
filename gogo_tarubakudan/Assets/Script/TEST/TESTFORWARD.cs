using UnityEngine;

public class TESTFORWARD : MonoBehaviour
{
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(transform.forward * 2f, ForceMode.Acceleration);
    }
}
