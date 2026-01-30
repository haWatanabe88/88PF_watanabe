using UnityEngine;

public class Target : MonoBehaviour
{
    HighSpeedManager high_speed_manager;
    private void Start()
    {
        high_speed_manager = GameObject.FindWithTag("highSpeedManager").GetComponent<HighSpeedManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("cannon_bullet"))
        {
            SoundManager.Instance.PlaySE("posi_hs");
            high_speed_manager.addScore();
            Destroy(gameObject);
        }
    }
}
