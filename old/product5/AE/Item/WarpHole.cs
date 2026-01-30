using UnityEngine;

public class WarpHole : MonoBehaviour
{
    [HideInInspector]
    public WarpHole pairedHole;

    [SerializeField] private float warpDelay = 2f; // 滞在秒数でワープ
    [SerializeField] private string playerTag = "Player";

    private float stayTimer = 0f;
    private bool playerInside = false;
    private Transform player;

    private void Start()
    {
        WarpHoleManager.Instance.RegisterHole(this);
    }

    private void Update()
    {
        if (playerInside && pairedHole != null)
        {
            stayTimer += Time.deltaTime;
            if (stayTimer >= warpDelay)
            {
                WarpPlayer();
                stayTimer = 0f; // リセット
            }
        }
    }

    private void WarpPlayer()
    {
        Debug.Log("ワープ実行！");
        player.position = pairedHole.transform.position + Vector3.up * 1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInside = true;
            player = other.transform;
            stayTimer = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInside = false;
            stayTimer = 0f;
        }
    }
}
