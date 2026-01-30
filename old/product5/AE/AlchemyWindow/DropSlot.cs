//→調合対象となるスロットにアタッチされているスクリプト
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public Image iconImage; // スロットに表示するアイコン画像
    public BaseItemSO itemData;
    [SerializeField] private bool allowDrop = true; // デフォルトではドロップ許可

    public void OnDrop(PointerEventData eventData)
    {
        if (!allowDrop) return; // ← Drop不可なら無視！
        DraggableItem draggedItem = eventData.pointerDrag?.GetComponent<DraggableItem>();
        if (draggedItem != null)
        {
            BaseItemSO droppedItem = draggedItem.itemData;
            if(droppedItem is MaterialItemSO)
            {
                // すでにスロットに別のアイテムが入っていれば、イベントリに戻す
                if (itemData != null)
                {
                    InventoryManager.Instance.AddCraftedItem(itemData); // 元のアイテムを戻す
                }
                // アイコンをセット
                iconImage.sprite = draggedItem.GetComponent<Image>().sprite;
                iconImage.color = Color.white; // 非表示だった場合に再表示
                itemData = draggedItem.itemData;//データを受け取る

                // 所持数を減らす（0になればUIスロットも消える）
                InventoryManager.Instance.DecreaseMaterial(itemData, 1);
            }
        }
    }

    public void Clear()
    {
        itemData = null;
        iconImage.sprite = null;
        iconImage.color = new Color(1, 1, 1, 255); //白にする
    }

    public void DisplayResult(CraftedItemSO item)
    {
        itemData = item;
        iconImage.sprite = item.icon;
        iconImage.color = Color.white;
    }

    public void ReturnToInventory()
    {
        if (itemData != null)
        {
            InventoryManager.Instance.AddCraftedItem(itemData);
            Clear();
        }
    }

}
