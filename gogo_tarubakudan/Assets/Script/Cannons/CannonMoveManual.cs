using System.Collections;
using UnityEngine;

public class CannonMoveManual : MonoBehaviour
{
    GameObject main_cannon;
    float rotation_speed_coef = 120f;
    CannonBullet cannon_bullet;
    PirateShip ship;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        main_cannon = GameObject.FindWithTag("main_cannon");
        ship = GameObject.FindWithTag("pirate_ship").GetComponent<PirateShip>();
        cannon_bullet = GetComponent<CannonBullet>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!AnimationManager.Instance.getIsCompHpBarAnim())
            return;
        if (!ship.getIsDefeat())
        {
            cannonRotation();
            fireBullet();
        }
    }

    void cannonRotation()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Vector3 rotation_x = new Vector3(v * rotation_speed_coef * Time.deltaTime, 0, 0);
        Vector3 rotation_y = new Vector3(0, h * rotation_speed_coef * Time.deltaTime, 0);
        main_cannon.transform.Rotate(rotation_x);
        transform.Rotate(rotation_y);
    }

    void fireBullet()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cannon_bullet.instantiateBulletManual();
        }
    }
}
