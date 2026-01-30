using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class PickupPromptUI : MonoBehaviour
{
    public TextMeshProUGUI promptText;
    public static PickupPromptUI Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        promptText.gameObject.SetActive(false); // 最初は非表示
    }

    public void Show(string message)
    {
        promptText.text = message;
        promptText.gameObject.SetActive(true);
    }

    public void Hide()
    {
        promptText.gameObject.SetActive(false);
    }
}