// InventorySlotUI.cs
//→inventoryのUIの値を更新するための関数
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI countText;
    public BaseItemSO currentItem;
    public Button useButton;

    public void OnPointerEnter(PointerEventData eventData)
    {   
        if (currentItem != null)
        {
            TooltipUIManager.Instance.Show(currentItem.itemName, currentItem.description, GetComponent<RectTransform>());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUIManager.Instance.Hide();
    }

    public void Set(BaseItemSO item, int count)
    {
        iconImage.sprite = item.icon;
        nameText.text = item.itemName;
        countText.text = $"x{count}";
        iconImage.GetComponent<DraggableItem>().itemData = item;
        currentItem = item;
        useButton.gameObject.SetActive(false); // 初期状態では非表示

        // 使用ボタンにクリックイベントを追加（必要なら1度だけ）
        useButton.onClick.RemoveAllListeners();
        useButton.onClick.AddListener(() =>
        {
            ItemEffectHandler.Instance.UseItem(item); // 効果発動
            useButton.gameObject.SetActive(false); // 使用後ボタンは閉じる
        });
    }

    public void UpdateCount(int newCount)
    {
        countText.text = $"x{newCount}";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        InventoryUIManager.Instance.OnSlotClicked(this);
        if (currentItem is CraftedItemSO)
        {
            // InventoryUIManager に通知して、他を非表示にしてもらう
            //InventoryUIManager.Instance.OnSlotClicked(this);
            useButton.gameObject.SetActive(true);
        }
        else
        {
            useButton.gameObject.SetActive(false);
        }
    }

    public void HideUseButton()
    {
        useButton.gameObject.SetActive(false);
    }
}
