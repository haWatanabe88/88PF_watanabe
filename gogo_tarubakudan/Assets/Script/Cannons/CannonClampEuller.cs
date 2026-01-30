using UnityEngine;

public class CannonClampEuller : MonoBehaviour
{
    GameObject main_cannon;

    float ROTATION_X_MAX = 25f;
    float ROTATION_X_MIN = -16.5f;
    float ROTATION_Y_MAX = 30f;
    float ROTATION_Y_MIN = -30f;

    public float getRotationXMAX() { return ROTATION_X_MAX; }
    public float getRotationXMIN() { return ROTATION_X_MIN; }
    public float getRotationYMAX() { return ROTATION_Y_MAX; }
    public float getRotationYMIN() { return ROTATION_Y_MIN; }


    void Start()
    {
        main_cannon = GameObject.FindWithTag("main_cannon");
    }

    void Update()
    {
        clampEullerAngles();
    }

    void clampEullerAngles()
    {
        Vector3 euler = main_cannon.transform.localEulerAngles;

        // 0〜360 を -180〜180 に直す（重要）
        if (euler.x > 180f) euler.x -= 360f;
        euler.x = Mathf.Clamp(euler.x, ROTATION_X_MIN, ROTATION_X_MAX);
        main_cannon.transform.localEulerAngles = euler;//砲台

        euler = transform.localEulerAngles;
        if (euler.y > 180f) euler.y -= 360f;
        euler.y = Mathf.Clamp(euler.y, ROTATION_Y_MIN, ROTATION_Y_MAX);
        transform.localEulerAngles = euler;//大砲そのもの
    }
}
