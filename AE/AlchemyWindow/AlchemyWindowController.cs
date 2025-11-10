using UnityEngine;

public class AlchemyWindowController : MonoBehaviour
{
    [SerializeField] private GameObject alchemyWindowPanel;
    [SerializeField] private GameObject alchemyWindowToggleIcon;
    [SerializeField] private GameObject toolTipPanel;

    public static AlchemyWindowController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Open()
    {
        alchemyWindowPanel.SetActive(true);
        alchemyWindowToggleIcon.SetActive(false);
    }

    public void Close()
    {
        alchemyWindowPanel.SetActive(false);
        alchemyWindowToggleIcon.SetActive(true);
        toolTipPanel.SetActive(false);
    }

    public void Toggle()
    {
        bool isOpen = alchemyWindowPanel.activeSelf;
        if (isOpen) Close();
        else Open();
    }
}
