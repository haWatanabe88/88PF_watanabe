using UnityEngine;

public class CameraCenterPosition : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject pipe_in;
    bool is_active_bullet;
    private void Start()
    {
        is_active_bullet = false;
        transform.position = new Vector3(pipe_in.transform.position.x, pipe_in.transform.position.y, transform.position.z);
    }

    private void Update()
    {
        if (!is_active_bullet)
        {
            bullet = GameObject.FindWithTag("cannon_bullet");
        }
        if (bullet)
        {
            is_active_bullet = true;
            transform.position = new Vector3(transform.position.x, transform.position.y, bullet.transform.position.z);
        }
    }
}
