// InventoryManager.cs
//→materialInventoryで、素材の所持数を管理している
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    // 所持素材の一覧（BaseItemSOをキーにして、所持数を管理）
    private Dictionary<BaseItemSO, int> materialInventory = new Dictionary<BaseItemSO, int>();

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

    // 素材を追加する
    public void AddMaterial(BaseItemSO item, int amount)
    {
        if (materialInventory.ContainsKey(item))
        {
            materialInventory[item] += amount;
        }
        else
        {
            materialInventory[item] = amount;
        }
        //Debug.Log($"[Inventory] {item.itemName} を {amount} 個追加（合計: {materialInventory[item]}）");
    }

    // 所持数を取得する
    public int GetMaterialCount(BaseItemSO item)
    {
        return materialInventory.TryGetValue(item, out int count) ? count : 0;
    }

    // インベントリ全体を取得（必要であれば）
    public Dictionary<BaseItemSO, int> GetAllMaterials()
    {
        return new Dictionary<BaseItemSO, int>(materialInventory);
    }

    //所持数を１つ減らす
    public void DecreaseMaterial(BaseItemSO item, int amount)
    {
        if (materialInventory.ContainsKey(item))
        {
            materialInventory[item] -= amount;

            if (materialInventory[item] <= 0)
            {
                materialInventory.Remove(item);
                InventoryUIManager.Instance.RemoveSlot(item); // スロットも削除
            }
            else
            {
                InventoryUIManager.Instance.UpdateSlot(item, materialInventory[item]);
            }
        }
    }

    //調合アイテムをイベントリ内に追加する関数
    public void AddCraftedItem(BaseItemSO item)
    {
        // 一度 BaseItemSO と共通のUI表示に統一するなら型変換もOK（共通処理化）
        if (materialInventory.ContainsKey(item))
        {
            materialInventory[item] += 1;
            InventoryUIManager.Instance.UpdateSlot(item, materialInventory[item]);
        }
        else
        {
            materialInventory[item] = 1;
            InventoryUIManager.Instance.OnItemAdded(item);
        }
    }



}

// ※ このスクリプトは「素材用インベントリ」に特化しています。
// 他の用途（調合アイテム、装備、消費アイテム等）が出てきたら、
// 将来的にインターフェース分離やカテゴリ別に拡張できます。
