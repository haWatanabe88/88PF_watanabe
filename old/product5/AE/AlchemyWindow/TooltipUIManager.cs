using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TooltipUIManager : MonoBehaviour
{
    public GameObject tooltipPanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    //public Vector2 offset = new Vector2(0f, 0f); // インスペクターから調整可能
    public static TooltipUIManager Instance { get; private set; }
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

    private void Start()
    {
        Hide();
    }

    public void Show(string itemName, string description, RectTransform target)
    {
        tooltipPanel.SetActive(true);
        nameText.text = itemName;
        descriptionText.text = description;
        // アイコンのスクリーン位置を取得
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(null, target.position);

        // Y方向にオフセットを加えて少し上に表示（例：+100ピクセル）
        screenPos.y += 220f;
        screenPos.x -= 120f;
        // TooltipPanelをその位置に移動
        tooltipPanel.transform.position = screenPos;
    }

    public void Hide()
    {
        tooltipPanel.SetActive(false);
    }
}
