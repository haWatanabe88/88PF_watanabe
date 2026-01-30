using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ItemEffectHandler : MonoBehaviour
{
    [SerializeField] private GameObject player;
    /// <summary>
    /// 透視メガネ
    /// </summary>
    [SerializeField] private float seeThroughRadius = 5f;
    [SerializeField] private float seeThroughDuration = 5f;
    [SerializeField] private Material seeThroughMaterial;
    private bool isSeeThroughActive = false;
    private float remainingDuration = 0f;
    private Dictionary<Renderer, Material> originalMaterials = new Dictionary<Renderer, Material>();
    //////////////////////
    /// <summary>
    /// ペイントコーン
    /// </summary>
    [SerializeField] private GameObject paintConePrefab; // ← プレハブをInspectorでアサイン
    /// <summary>
    /// エスケープキー、小さな鍵、トゲ鉄球、ヘビーオブジェクト
    /// </summary>
    [SerializeField] private float interactionRadius = 2f;
    public static ItemEffectHandler Instance { get; private set; }
    /// <summary>
    ///ビートル型・マッピングドローン 
    /// </summary>
    [SerializeField] private DroneCameraController droneCameraController;
    /// <summary>
    /// ワープホール
    /// </summary>
    [SerializeField] private GameObject warpHolePrefab;
    [SerializeField] private float warpHoleYOffset = -0.07f; // 地面との距離の調整
    /// <summary>
    /// 煩悩スキャナー
    /// </summary>
    [SerializeField] private List<CraftedItemSO> allCraftedItems; // 全登録リスト
    [SerializeField] private CraftedItemSO escapeKeyItem;          // 除外対象
    //[SerializeField] private int minItems = 2;
    [SerializeField] private int maxItems = 3;

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

    void Update()
    {
        if (isSeeThroughActive)
        {
            UpdateSeeThroughEffect();
        }
    }

    public void UseItem(BaseItemSO item)
    {
        if (!(item is CraftedItemSO crafted)) return;

        switch (crafted.effectType)
        {
            case ItemEffectType.SeeThroughWall:
                AlchemyWindowController.Instance.Close(); // 開いてたら閉じる
                StartCoroutine(SeeThroughCoroutine());
                InventoryManager.Instance.DecreaseMaterial(item, 1); // 成功したら減らす
                break;

            case ItemEffectType.PaintCone:
                AlchemyWindowController.Instance.Close(); // 開いてたら閉じる
                PlacePaintCone();
                InventoryManager.Instance.DecreaseMaterial(item, 1); // 成功したら減らす
                break;
            case ItemEffectType.beetledrone:
                AlchemyWindowController.Instance.Close(); // 開いてたら閉じる
                droneCameraController.StartDroneViewTimed();
                InventoryManager.Instance.DecreaseMaterial(item, 1); // 成功したら減らす
                break;

            case ItemEffectType.mappingdrone:
                AlchemyWindowController.Instance.Close(); // 開いてたら閉じる
                droneCameraController.StartDroneViewTimed(); // この後カーソル処理追加予定！
                droneCameraController.EnableMappingMode(); // マッピングモードON
                InventoryManager.Instance.DecreaseMaterial(item, 1); // 成功したら減らす
                break;
            case ItemEffectType.warphole:
                AlchemyWindowController.Instance.Close(); // 開いてたら閉じる
                InventoryManager.Instance.DecreaseMaterial(item, 1); // 成功したら減らす
                UseWarpHole();
                break;
            case ItemEffectType.BonnoScanner:
                InventoryManager.Instance.DecreaseMaterial(item, 1); // 成功したら減らす
                UseBonnoScanner();
                break;

            case ItemEffectType.EscapeKey:
                AlchemyWindowController.Instance.Close(); // 開いてたら閉じる
                if (HandleTaggedInteraction("EscapeDoor", () =>
                {
                    GameClearManager.Instance.OnGameClear(); // ← ここで演出スタート！
                }))
                {
                    InventoryManager.Instance.DecreaseMaterial(item, 1);
                }
                break;

            case ItemEffectType.MiniKey:
                AlchemyWindowController.Instance.Close(); // 開いてたら閉じる
                if (HandleTaggedInteraction("minidoor", () => DestroyFirstHitWithTag("minidoor")))
                {
                    InventoryManager.Instance.DecreaseMaterial(item, 1);
                }
                break;

            case ItemEffectType.ruggedironball:
                AlchemyWindowController.Instance.Close(); // 開いてたら閉じる
                if (HandleTaggedInteraction("statue", () => DestroyFirstHitWithTag("statue")))
                {
                    InventoryManager.Instance.DecreaseMaterial(item, 1);
                }
                break;

            case ItemEffectType.heavyobject:
                AlchemyWindowController.Instance.Close(); // 開いてたら閉じる
                if(UseHeavyObject())// 🎯 heavySwitch だけ探して発動！
                {
                    InventoryManager.Instance.DecreaseMaterial(item, 1);
                }
                break;
            default:
                Debug.Log("未定義の効果です");
                break;
        }
    }


    /// <summary>
    ///透視メガネに関するもの
    /// </summary>
    private IEnumerator SeeThroughCoroutine()
    {
        isSeeThroughActive = true;
        remainingDuration = seeThroughDuration;
        originalMaterials.Clear();

        while (remainingDuration > 0f)
        {
            remainingDuration -= Time.deltaTime;
            yield return null;
        }

        // 効果終了後、すべて元に戻す
        foreach (var pair in originalMaterials)
        {
            if (pair.Key != null)
                pair.Key.material = pair.Value;
        }

        originalMaterials.Clear();
        isSeeThroughActive = false;
        Debug.Log("透視メガネ効果終了！");
    }
    private void UpdateSeeThroughEffect()
    {
        Collider[] hits = Physics.OverlapSphere(player.transform.position, seeThroughRadius);

        // 一時的なセットで現在接触しているwallだけ保持
        HashSet<Renderer> currentHits = new HashSet<Renderer>();

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Wall"))
            {
                Renderer rend = hit.GetComponent<Renderer>();
                if (rend != null)
                {
                    currentHits.Add(rend);

                    // 初めての接触なら記録＆マテリアル変更
                    if (!originalMaterials.ContainsKey(rend))
                    {
                        originalMaterials[rend] = rend.material;
                        rend.material = seeThroughMaterial;
                    }
                }
            }
        }

        // 接触が切れた wall を元に戻す
        List<Renderer> toRemove = new List<Renderer>();
        foreach (var pair in originalMaterials)
        {
            if (!currentHits.Contains(pair.Key) && pair.Key != null)
            {
                pair.Key.material = pair.Value;
                toRemove.Add(pair.Key);
            }
        }

        foreach (var rend in toRemove)
        {
            originalMaterials.Remove(rend);
        }
    }
    /// <summary>
    /// ペイントコーンに関するもの
    /// </summary>
    private void PlacePaintCone()
    {
        if (paintConePrefab != null && player != null)
        {
            //Vector3 spawnPos = player.transform.position;
            Vector3 spawnPos =  new Vector3(player.transform.position.x, player.transform.position.y + 0.55f, player.transform.position.z);
            Instantiate(paintConePrefab, spawnPos, Quaternion.identity);
            Debug.Log("ペイントコーンを設置しました！");
        }
        else
        {
            Debug.LogWarning("PaintCone prefab か player が未設定です！");
        }
    }

    /// <summary>
    /// エスケープキー、小さな鍵、トゲ鉄球、ヘビーオブジェクト
    /// 特定のタグと接触していれば効果を発動、true を返す
    /// </summary>
    private bool HandleTaggedInteraction(string targetTag, System.Action onSuccess)
    {
        Collider[] hits = Physics.OverlapSphere(player.transform.position, interactionRadius);

        foreach (var hit in hits)
        {
            if (hit.CompareTag(targetTag))
            {
                onSuccess?.Invoke(); // 効果発動
                return true;
            }
        }

        Debug.Log("使用できません！（対象が近くにない）");
        return false;
    }
    private void DestroyFirstHitWithTag(string targetTag)
    {
        Collider[] hits = Physics.OverlapSphere(player.transform.position, interactionRadius);

        foreach (var hit in hits)
        {
            if (hit.CompareTag(targetTag))
            {
                Destroy(hit.gameObject);
                Debug.Log($"{targetTag} を破壊しました！");
                return;
            }
        }

        Debug.Log($"使用できません！{targetTag} が近くにありません！");
    }
    /// <summary>
    /// ワープホール関連
    /// </summary>
    private void UseWarpHole()
    {
        Vector3 dropPosition = player.transform.position;
        dropPosition.y += warpHoleYOffset;

        Instantiate(warpHolePrefab, dropPosition, Quaternion.identity);
        Debug.Log("ワープホール設置完了！");
    }
    /// <summary>
    /// 煩悩スキャナー関連
    /// </summary>
    private void UseBonnoScanner()
    {
        // 除外対象以外を抽出
        var filteredItems = allCraftedItems
            .Where(item => item != escapeKeyItem)
            .ToList();

        int numItems = maxItems - 1;

        for (int i = 0; i < numItems; i++)
        {
            if (filteredItems.Count == 0) break;

            var randomIndex = Random.Range(0, filteredItems.Count);
            var selectedItem = filteredItems[randomIndex];

            InventoryManager.Instance.AddCraftedItem(selectedItem);

            // 重複防止したい場合は↓
            filteredItems.RemoveAt(randomIndex);
        }

        Debug.Log("煩悩スキャナーを使用し、ランダムにアイテムを獲得！");
    }
    /// <summary>
    /// ヘビースイッチ関連
    /// </summary>
    private bool UseHeavyObject()
    {
        Collider[] hits = Physics.OverlapSphere(player.transform.position, interactionRadius);

        foreach (var hit in hits)
        {
            HeavySwitch heavySwitch = hit.GetComponent<HeavySwitch>();
            if (heavySwitch != null)
            {
                heavySwitch.ActivateSwitch(); // 🎯 ここでスイッチ発動！
                Debug.Log("Heavy Switch activated!");
                WarpMessageUI.Instance?.ShowMessage("どこかで扉が開いたようだ・・・");//ものすごく良くないけど、代理で
                return true; // 1個だけ反応させたら抜ける
            }
        }

        Debug.Log("近くに HeavySwitch が見つかりませんでした。");
        return false;
    }

}
