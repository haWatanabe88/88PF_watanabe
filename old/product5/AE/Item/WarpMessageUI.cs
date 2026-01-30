using UnityEngine;
using TMPro;
using System.Collections;

public class WarpMessageUI : MonoBehaviour
{
    public static WarpMessageUI Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float displayDuration = 2f;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;

        Hide(); // 最初は非表示
    }

    public void ShowMessage(string message)
    {
        StopAllCoroutines();
        messageText.text = message;
        canvasGroup.alpha = 1f;
        gameObject.SetActive(true);

        StartCoroutine(AutoHide());
    }

    private IEnumerator AutoHide()
    {
        yield return new WaitForSeconds(displayDuration);
        Hide();
    }

    private void Hide()
    {
        canvasGroup.alpha = 0f;
        messageText.text = "";
        gameObject.SetActive(false);
    }
}
