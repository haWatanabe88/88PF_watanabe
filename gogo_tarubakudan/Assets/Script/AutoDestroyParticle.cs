using UnityEngine;

public class AutoDestroyParticle : MonoBehaviour
{
    void Update()
    {
        Destroy(gameObject, 2f);
    }
}
