using UnityEngine;

public class CCHighSpeedRun : MonoBehaviour
{
    public GameObject main_camera;
    [SerializeField] GameObject camera_center_pos;
    GameObject bullet;
    public float y_adjust { get; private set; } // = 1.5f;
    public float z_adjust { get; private set; }// = 8.0f;
    public bool is_track_disable { get; private set; }

    private void Start()
    {
        y_adjust = 0f;
        z_adjust = 15.0f;
        bullet = GameObject.FindWithTag("cannon_bullet");
    }

    void Update()
    {
        trackbullet_highSpeedRun();
    }

    void trackbullet_highSpeedRun()
    {
        // オフセットベクトルをカメラの回転分だけ回しながら、追従させる
        main_camera.transform.position = camera_center_pos.transform.position + main_camera.transform.rotation * new Vector3(0, y_adjust, -z_adjust);
    }
}
