using UnityEngine;

public class CannonRotationSE : MonoBehaviour
{
    public float stepAngle_y = 2f;
    public float stepAngle_x = 5f;

    float lastAngle_y;
    float accumulatedAngle_y;
    float lastAngle_x;
    float accumulatedAngle_x;
    GameObject main_cannon;

    void Start()
    {
        main_cannon = GameObject.FindWithTag("main_cannon");
        lastAngle_y = transform.eulerAngles.y;
        lastAngle_x = main_cannon.transform.eulerAngles.x;
    }

    void Update()
    {
        withBody();
        withMainCannon();
    }

    void withBody()
    {
        float currentAngle_y = transform.eulerAngles.y;
        float delta = Mathf.DeltaAngle(lastAngle_y, currentAngle_y);
        accumulatedAngle_y += Mathf.Abs(delta);

        if (accumulatedAngle_y >= stepAngle_y)
        {
            SoundManager.Instance.PlaySE("cannonMove_pi");
            accumulatedAngle_y = 0f;
        }

        lastAngle_y = currentAngle_y;
    }

    void withMainCannon()
    {
        float currentAngle_x = main_cannon.transform.eulerAngles.x;
        float delta = Mathf.DeltaAngle(lastAngle_x, currentAngle_x);
        accumulatedAngle_x += Mathf.Abs(delta);

        if (accumulatedAngle_x >= stepAngle_x)
        {
            SoundManager.Instance.PlaySE("cannonMove_pi");
            accumulatedAngle_x = 0f;
        }

        lastAngle_x = currentAngle_x;
    }

}
