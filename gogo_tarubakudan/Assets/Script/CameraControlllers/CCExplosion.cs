using DG.Tweening;
using UnityEngine;

public class CCExplosion : MonoBehaviour
{
    public GameObject main_camera;
    [SerializeField] GameObject bullet_explosion;
    [SerializeField] Vector3 start_camera_transform_position;
    [SerializeField] Quaternion start_camera_transform_rotation;
    [SerializeField] GameObject manualManager;
    [SerializeField] GameObject explosionManager;
    public float y_adjust { get; private set; } // = 1.5f;
    public float z_adjust { get; private set; }// = 8.0f;
    public bool is_track_disable { get; private set; }

    private void Start()
    {
        y_adjust = 1.5f;
        z_adjust = 10.0f;
        start_camera_transform_position = main_camera.transform.position;
        start_camera_transform_rotation = main_camera.transform.rotation;
    }

    void Update()
    {
        if (!GameObject.FindWithTag("cannon_bullet"))
        {
            initializeCameraPos();
            return;
        }
        if (!is_track_disable)
        {
            trackbullet_explosion();
        }
    }

    void trackbullet_explosion()
    {
        bullet_explosion = GameObject.FindWithTag("cannon_bullet");
        // オフセットベクトルをカメラの回転分だけ回しながら、追従させる
        main_camera.transform.position = bullet_explosion.transform.position + main_camera.transform.rotation * new Vector3(0, y_adjust, -z_adjust);
    }

    void initializeCameraPos()
    {
        if(!explosionManager.GetComponent<ExplosionManager>().is_gameover)
            manualManager.GetComponent<ManualManager>().showManualCanvas();
        main_camera.transform.position = start_camera_transform_position;
        main_camera.transform.rotation = start_camera_transform_rotation;
    }

    public void setYadjust(float val)
    {
        y_adjust = val;
    }

    public void setZadjust(float val)
    {
        z_adjust = val;
    }

    public void setIsTrackDisnable(bool val)
    {
        is_track_disable = val;
    }

    public void setYandZinisial()
    {
        y_adjust = 1.5f;
        z_adjust = 8f;
    }
}
