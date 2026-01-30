using UnityEngine;

public class BulletMoveAB : MonoBehaviour
{
    Vector3 startPos;
    Vector3 targetPos;
    float speed;

    bool isMoving = false;

    // Instantiate’¼Œã‚ÉŒÄ‚Ô‰Šú‰»ŠÖ”
    public void Init(Vector3 A, Vector3 B, float moveSpeed)
    {
        startPos = A;
        targetPos = B;
        speed = moveSpeed;

        transform.position = startPos;
        isMoving = true;
    }

    void Update()
    {
        if (!isMoving) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            speed * Time.deltaTime
        );

        // “’B”»’è
        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            GetComponent<BulletMoveAB>().enabled = false;
        }
    }
}
