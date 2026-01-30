using UnityEngine;

public class Fan : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("cannon_bullet"))
        {
            SoundManager.Instance.PlaySE("gekitotu_hs");
        }
    }
}
