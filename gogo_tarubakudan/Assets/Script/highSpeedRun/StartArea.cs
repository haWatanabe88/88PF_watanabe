using UnityEngine;

public class StartArea : MonoBehaviour
{
    HighSpeedManager highSpeedManager;
    private void Start()
    {
        highSpeedManager = GameObject.FindWithTag("highSpeedManager").GetComponent<HighSpeedManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("cannon_bullet"))
        {
            BulletHighSpeedRun bullet_obj = other.GetComponent<BulletHighSpeedRun>();
            Rigidbody rb = other.GetComponent<Rigidbody>();
            bullet_obj.setIsStart(true);
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero;
            highSpeedManager.setIsGameStart(true);
            this.gameObject.SetActive(false);
        }
    }
}
