using UnityEngine;

public class enemy : MonoBehaviour
{
    void FixedUpdate()
    {
        float dot = Vector3.Dot(transform.up, Vector3.up);
        if (dot < 0.8f)
        {
            this.tag = "defeatEnemy";
        }
    }
}
