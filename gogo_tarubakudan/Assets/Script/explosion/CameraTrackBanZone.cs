using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraTrackBanZone : MonoBehaviour
{
    CCExplosion cc_explosion;


    private void Start()
    {
        cc_explosion = GameObject.FindWithTag("CC").GetComponent<CCExplosion>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("cannon_bullet"))
        {
            cc_explosion.setIsTrackDisnable(true);
        }
    }
}
