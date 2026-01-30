using UnityEngine;
using UnityEngine.LowLevelPhysics2D;

public class CannonBullet : MonoBehaviour
{

    [SerializeField] PlayerPhysicsDataSO physicsData;
    [SerializeField] GameObject fire_point;
    [SerializeField] GameObject test_bullet;

    //’e‚Ì”­ŽËŠÔŠu
    float FIRE_INTERVAL_SMALL = 0.5f;
    float FIRE_INTERVAL_MIDDLE = 1f;
    float FIRE_INTERVAL_BIG = 2f;
    float fire_interval;

    void Start()
    {
        fire_interval = 0;
    }

    private void Update()
    {
        if (fire_interval > 0)
        {
            fire_interval -= Time.deltaTime;
        }
    }

    public void instantiateBulletManual()
    {
        if (fire_interval <= 0 && Input.GetKeyDown(KeyCode.Space))
        {
            if (physicsData.currentState == PlayerStatus.PlayerState.Small)
            {
                fire_interval = FIRE_INTERVAL_SMALL;
            }
            else if (physicsData.currentState == PlayerStatus.PlayerState.Middle)
            {
                fire_interval = FIRE_INTERVAL_MIDDLE;
            }
            else if (physicsData.currentState == PlayerStatus.PlayerState.Big)
            {
                fire_interval = FIRE_INTERVAL_BIG;
            }
            SoundManager.Instance.PlaySE("shot_pi");
            Instantiate(test_bullet, fire_point.transform.position, transform.rotation);
        }
    }
}
