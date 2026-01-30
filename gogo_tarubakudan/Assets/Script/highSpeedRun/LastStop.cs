using UnityEngine;

public class LastStop : MonoBehaviour
{
    [SerializeField] HighSpeedManager manager;
    [SerializeField] GameObject manualManager;
    [SerializeField] GameObject timer_text;
    [SerializeField] GameObject bonusscore_text;

    private void Start()
    {
        timer_text.SetActive(true);
        bonusscore_text.SetActive(true);
        manager = GameObject.FindWithTag("highSpeedManager").GetComponent<HighSpeedManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("cannon_bullet"))
        {
            manager.activeGameClearInfo();
            timer_text.SetActive(false);
            bonusscore_text.SetActive(false);
            manualManager.GetComponent<ManualManager>().hideManualCanvas();
        }
    }
}
