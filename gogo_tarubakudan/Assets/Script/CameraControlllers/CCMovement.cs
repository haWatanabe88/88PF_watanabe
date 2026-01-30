using UnityEngine;
using UnityEngine.SocialPlatforms;


public class CCMovement : MonoBehaviour
{
    public GameObject main_camera;
    public GameObject player;
    [SerializeField] Vector2 rotation_speed;//xをseria~で0.06にする
    Vector2 start_mouse_position;

    void Update()
    {
        rotateArroundPlayer();
    }


    void rotateArroundPlayer()
    {
        if (Input.GetMouseButtonDown(0))
        {
            start_mouse_position = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            //var x_change_amount = (start_mouse_position.x - Input.mousePosition.x);
            var x_change_amount = (Input.mousePosition.x - start_mouse_position.x);
            var newAngle = Vector3.zero;
            newAngle.x = x_change_amount * rotation_speed.x;
            //RotateAround（回転の中心、回転軸、量）;
            main_camera.transform.RotateAround(player.transform.position, Vector3.up, newAngle.x);
            start_mouse_position = Input.mousePosition;//start_positionを更新して変化量分の回転をさせる         
        }
    }
}
