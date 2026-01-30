using UnityEngine;

public class DeltaAngle : MonoBehaviour
{
    [SerializeField] int a;
    [SerializeField] int b;

        void Update ()
        {
            // Prints 90
            //Debug.Log(Mathf.DeltaAngle(a, b));
        }
}
