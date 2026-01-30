using UnityEngine;

public class SpeedUpItem : MonoBehaviour
{
    BulletHighSpeedRun bullet;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("cannon_bullet"))
        {
            SoundManager.Instance.PlaySE("speedUp_hs");
            Debug.Log("スピードアップ");
            bullet = other.gameObject.GetComponent<BulletHighSpeedRun>();
            bullet.aceleration();
            Destroy(gameObject);
        }
    }
}
