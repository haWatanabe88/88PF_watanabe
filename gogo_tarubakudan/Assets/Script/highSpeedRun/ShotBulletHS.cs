using UnityEngine;

public class ShotBulletHS : MonoBehaviour
{
    [SerializeField] GameObject bullet_hs;
    [SerializeField] GameObject fire_point;
    bool is_fire;

    private void Update()
    {
        if (!AnimationManager.Instance.getIsCompHpBarAnim())
            return;
        fireBullet();
    }

    void fireBullet()
    {
        if (!is_fire  && Input.GetKeyDown(KeyCode.Space))
        {
            is_fire = true;
            SoundManager.Instance.PlaySE("shot_pi");
            Instantiate(bullet_hs, fire_point.transform.position, Quaternion.identity);
        }
    }
}
