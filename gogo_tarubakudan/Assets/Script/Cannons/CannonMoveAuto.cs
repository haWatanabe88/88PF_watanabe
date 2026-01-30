using System.Data;
using UnityEngine;

public class CannonMoveAuto : MonoBehaviour
{
    GameObject main_cannon;
    GameObject full_cannon;
    CannonClampEuller limit_euller;
    ShootBullet shoot_bullet;
    DrawArc draw_arc;
    Vector3 body_euller;
    Vector3 main_cannon_euller;
    bool is_max;
    bool is_stop_cannonbody;
    bool is_stop_maincannon;
    bool is_fire;

    float ROTATION_X_MAX;
    float ROTATION_X_MIN;
    float ROTATION_Y_MAX;
    float ROTATION_Y_MIN;
    [SerializeField] float rotation_coef = 100f;
    [SerializeField] float rotation_coef2 = 0.3f;
    [SerializeField] GameObject manualManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        full_cannon = this.gameObject;
        main_cannon = GameObject.FindWithTag("main_cannon");
        draw_arc = this.gameObject.GetComponent<DrawArc>();
        shoot_bullet = this.gameObject.GetComponent<ShootBullet>();
        limit_euller = GameObject.FindWithTag("cannon").GetComponent<CannonClampEuller>();
        ROTATION_X_MAX = limit_euller.getRotationXMAX();
        ROTATION_X_MIN = limit_euller.getRotationXMIN();
        ROTATION_Y_MAX = limit_euller.getRotationYMAX();
        ROTATION_Y_MIN = limit_euller.getRotationYMIN();
    }

    // Update is called once per frame
    void Update()
    {
        if (!AnimationManager.Instance.getIsCompHpBarAnim())
            return;
        stopMove();
        if (!is_stop_cannonbody)
        {
            cannonBodyRotationAuto();
        }
        else if(is_stop_cannonbody && !is_stop_maincannon)
        {
            mainCannonRotationAuto();
        }
        if(is_stop_cannonbody && is_stop_maincannon && !is_fire)
        {
            //’e‚ð”­ŽË‚·‚é
            is_fire = true;
            manualManager.GetComponent<ManualManager>().hideManualCanvas();
            draw_arc.setDrawArc(false);
            shoot_bullet.fireBullet();
        }
    }
    void stopMove()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!is_stop_cannonbody)
            {
                SoundManager.Instance.PlaySE("cannonstop_ex");
                is_stop_cannonbody = true;
            }
            else if (is_stop_cannonbody && !is_stop_maincannon)
            {
                SoundManager.Instance.PlaySE("cannonstop_ex");
                is_stop_maincannon = true;
            }
        }
    }

    public void initialize()
    {
        draw_arc.setDrawArc(true);
        is_stop_cannonbody = false;
        is_stop_maincannon = false;
        is_fire = false;
    }

    void cannonBodyRotationAuto()
    {
        body_euller = full_cannon.transform.localEulerAngles;//‘å–C‚»‚Ì‚à‚Ì
        if (body_euller.y > 180f) body_euller.y -= 360f;

        if (!is_max && ROTATION_Y_MAX > body_euller.y)
        {
            body_euller.y += Time.deltaTime * rotation_coef;
        }
        else if (is_max && ROTATION_Y_MIN < body_euller.y)
        {
            body_euller.y -= Time.deltaTime * rotation_coef;
        }
        full_cannon.transform.localEulerAngles = body_euller;

        if (ROTATION_Y_MAX <= body_euller.y)
        {
            is_max = true;
        }
        else if (ROTATION_Y_MIN >= body_euller.y)
        {
            is_max = false;
        }
    }

    void mainCannonRotationAuto()
    {
        main_cannon_euller = main_cannon.transform.localEulerAngles;//‘å–C‚»‚Ì‚à‚Ì
        if (main_cannon_euller.x > 180f) main_cannon_euller.x -= 360f;

        if (!is_max && ROTATION_X_MAX > main_cannon_euller.x)
        {
            main_cannon_euller.x += Time.deltaTime * rotation_coef * rotation_coef2;
        }
        else if (is_max && ROTATION_X_MIN < main_cannon_euller.x)
        {
            main_cannon_euller.x -= Time.deltaTime * rotation_coef * rotation_coef2;
        }
        main_cannon.transform.localEulerAngles = main_cannon_euller;

        if (ROTATION_X_MAX <= main_cannon_euller.x)
        {
            is_max = true;
        }
        else if (ROTATION_X_MIN >= main_cannon_euller.x)
        {
            is_max = false;
        }
    }
}
