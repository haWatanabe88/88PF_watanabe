using UnityEngine;

public class CCTracker : MonoBehaviour
{
    public GameObject main_camera;
    public GameObject player;

    public float y_adjust { get; private set; } // = 1.5f;
    public float z_adjust { get; private set; }// = 8.0f;

    private void Start()
    {
        y_adjust = 1.5f;
        z_adjust = 8.0f;
    }

    void Update()
    {
        trackPlayer();
    }

    void trackPlayer()
    {
        // オフセットベクトルをカメラの回転分だけ回しながら、追従させる
        main_camera.transform.position = player.transform.position + main_camera.transform.rotation * new Vector3(0, y_adjust, -z_adjust);
    }

    public void setYadjust(float val)
    {
        y_adjust = val;
    }

    public void setZadjust(float val)
    {
        z_adjust = val;
    }

    public void setYandZinisial()
    {
        y_adjust = 1.5f;
        z_adjust = 8f;
    }
}
