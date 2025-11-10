// InventoryUIManager.cs
//→inventoryのUIの更新をinventorySlotUIに対して「指示している」関数
using System.Collections.Generic;
using UnityEngine;


public class InventoryUIManager : MonoBehaviour
{
    [Header("表示するスロットの親")]
    public Transform slotParent; // InventoryPanelのTransform
    [Header("使うスロットのプレハブ")]
    public GameObject inventorySlotPrefab; // プレハブ化されたInventorySlot

    private Dictionary<BaseItemSO, GameObject> slotLookup = new Dictionary<BaseItemSO, GameObject>();
    public static InventoryUIManager Instance { get; private set; }
    private InventorySlotUI activeSlotUI; // 現在表示中のスロット
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

    public void OnSlotClicked(InventorySlotUI clickedSlot)
    {
        // 前のスロットの「使用」ボタンを非表示
        if (activeSlotUI != null && activeSlotUI != clickedSlot)
        {
            activeSlotUI.HideUseButton();
        }

        // 今回クリックしたスロットを記録
        activeSlotUI = clickedSlot;
    }

    public void ClearActiveSlot()
    {
        activeSlotUI = null;
    }

    void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        // すでにあるスロットを一旦すべて破棄（後で差分更新方式にもできる）
        foreach (Transform child in slotParent)
        {
            Destroy(child.gameObject);
        }
        slotLookup.Clear();

        Dictionary<BaseItemSO, int> inventory = InventoryManager.Instance.GetAllMaterials();
        foreach (var pair in inventory)
        {
            BaseItemSO item = pair.Key;
            int count = pair.Value;

            GameObject slot = Instantiate(inventorySlotPrefab, slotParent);
            InventorySlotUI slotUI = slot.GetComponent<InventorySlotUI>();
            slotUI.Set(item, count);

            slotLookup[item] = slot;
        }
    }

    // 素材が追加されたときに手軽に更新できる関数
    public void OnItemAdded(BaseItemSO item)
    {
        if (slotLookup.ContainsKey(item))
        {
            // 所持数を更新
            int newCount = InventoryManager.Instance.GetMaterialCount(item);
            slotLookup[item].GetComponent<InventorySlotUI>().UpdateCount(newCount);
        }
        else
        {
            // 新規スロットを追加
            GameObject slot = Instantiate(inventorySlotPrefab, slotParent);
            InventorySlotUI slotUI = slot.GetComponent<InventorySlotUI>();
            int count = InventoryManager.Instance.GetMaterialCount(item);
            slotUI.Set(item, count);
            slotLookup[item] = slot;
        }
    }

    public void RemoveSlot(BaseItemSO item)
    {
        if (slotLookup.TryGetValue(item, out GameObject slotGO))
        {
            DraggableItem draggable = slotGO.GetComponentInChildren<DraggableItem>();
            if(draggable != null)
            {
                draggable.OnDestroy();
            }
            Destroy(slotGO);
            slotLookup.Remove(item);
        }
    }

    public void UpdateSlot(BaseItemSO item, int newCount)
    {
        if (slotLookup.TryGetValue(item, out GameObject slotGO))
        {
            InventorySlotUI slotUI = slotGO.GetComponent<InventorySlotUI>();
            slotUI.Set(item, newCount);
        }
    }

}
