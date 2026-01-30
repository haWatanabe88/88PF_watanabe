using Unity.VisualScripting;
using UnityEngine;

public class PlayerThroughWalls : MonoBehaviour
{
    [SerializeField] private Transform player;
    GameObject prevHitWall;

    void Update()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("normalwall"))
            {
                GameObject currentWall = hit.collider.gameObject;

                // ï ÇÃï«Ç…êÿÇËë÷ÇÌÇ¡ÇΩèuä‘
                if (prevHitWall != currentWall)
                {
                    // ëOÇÃï«ÇñﬂÇ∑
                    if (prevHitWall != null)
                    {
                        prevHitWall.GetComponent<MeshRenderer>().enabled = true;
                    }

                    // ç°ÇÃï«ÇìßñæÇ…
                    currentWall.GetComponent<MeshRenderer>().enabled = false;
                    prevHitWall = currentWall;
                }

                return;
            }
        }

        // RayÇ™ìñÇΩÇÁÇ»Ç¢ or normalwallÇ∂Ç·Ç»Ç¢
        if (prevHitWall != null)
        {
            prevHitWall.GetComponent<MeshRenderer>().enabled = true;
            prevHitWall = null;
        }
    }
}
