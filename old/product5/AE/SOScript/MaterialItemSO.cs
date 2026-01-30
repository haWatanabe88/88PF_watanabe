// ScriptableObject: 素材アイテム
using UnityEngine;

[CreateAssetMenu(menuName = "Item/MaterialItem")]
public class MaterialItemSO : BaseItemSO
{
    public int amountPerPickup = 1; // 1度のインタラクトで取得できる個数
}
