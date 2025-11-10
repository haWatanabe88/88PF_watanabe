// ScriptableObject: 調合アイテム
using UnityEngine;

public enum ItemEffectType
{
    PaintCone,//ペイントコーン
    SeeThroughWall, //透視メガネ
    EscapeKey,//エスケープキー
    MiniKey,//小さな鍵
    ruggedironball,//トゲ鉄球
    heavyobject,//ヘビーオブジェ
    beetledrone,//ビートル型ドローン
    mappingdrone,//マッピングドローン
    warphole,//ワープホール
    BonnoScanner,//煩悩スキャナー
}

[CreateAssetMenu(menuName = "Item/CraftedItem")]
public class CraftedItemSO : BaseItemSO
{
    //public string description;      // アイテムの説明
    public float duration;          // 例: 効果時間
    public bool isUsable = true;    // 使用可能なアイテムか
    public ItemEffectType effectType;

}
