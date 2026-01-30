using UnityEngine;

public class BreakWall : MonoBehaviour
{
    [SerializeField] TitleManager title_Manager;
    [SerializeField] GameObject explos_pos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.gameObject.gameObject.SetActive(false);
            title_Manager.DestroyBreakWall();
        }
    }
}

