using UnityEngine;

public class PlayerMoveSE : MonoBehaviour
{
    [SerializeField] float stepDistance;

    Vector3 lastPosition;
    float accumulatedDistance;

    void Start()
    {
        lastPosition = transform. position;
        Debug.Log(lastPosition);
    }

    void Update()
    {
        HandleMoveSE();
    }

    void HandleMoveSE()
    {
        Vector3 currentPosition = transform.position;
        float deltaDistance = Vector3.Distance(currentPosition, lastPosition);
        accumulatedDistance += deltaDistance;

        if (accumulatedDistance >= stepDistance)
        {
            SoundManager.Instance.PlaySE("spinBall_grow");
            accumulatedDistance -= stepDistance;
        }

        lastPosition = currentPosition;
    }
}