using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerStatusTextManager : MonoBehaviour
{
    TextMeshProUGUI player_status_text;
    [SerializeField] GameObject energy_ui;
    [SerializeField] GameObject max_text;
    void Start()
    {
        max_text.SetActive(false);
        player_status_text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        float fill_amount = energy_ui.GetComponent<Image>().fillAmount;
        if (fill_amount < 0.45f)
        {
            player_status_text.text = "Light";
            max_text.SetActive(false);
        }
        else if (fill_amount >= 1f)
        {
            player_status_text.text = "MEGA";
            max_text.SetActive(true);
        }
        else
        {
            player_status_text.text = "Normal";
            max_text.SetActive(false);
        }
    }
}
