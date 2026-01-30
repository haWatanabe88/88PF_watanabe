//ドラッグ操作をした時に「DragIcon」というゲームオブジェクトを生成し、
//ドラッグ操作可能にする

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image iconImage; // ドラッグ中に表示する画像
    private Canvas parentCanvas;
    private Transform originalParent;

    private GameObject dragImage;
    public BaseItemSO itemData;

    void Awake()
    {
        // 自動取得（未設定なら）
        if (iconImage == null)
        {
            iconImage = GetComponent<Image>();
        }
    }

    void Start()
    {
        parentCanvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;

        dragImage = new GameObject("DragIcon");
        dragImage.transform.SetParent(parentCanvas.transform, false);
        dragImage.transform.localScale = new Vector3(1.0f, 3.3f, 1.0f);
        Image img = dragImage.AddComponent<Image>();
        img.sprite = iconImage.sprite;
        img.raycastTarget = false;
        img.rectTransform.sizeDelta = iconImage.rectTransform.sizeDelta;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragImage != null)
        {
            dragImage.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragImage != null)
        {
            Destroy(dragImage);
        }
    }

    public void OnDestroy()
    {
        if (dragImage != null)
        {
            Destroy(dragImage);
            dragImage = null;
        }
    }
}
