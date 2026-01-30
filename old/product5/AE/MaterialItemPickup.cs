using UnityEngine;

public class MaterialItemPickup : MonoBehaviour
{
    public MaterialItemSO itemData;

    private bool isPlayerInRange = false;
    private MeshRenderer cubeRenderer;
    private Color originalColor;
    public Color highlightColor = Color.yellow;
    [Header("復活設定")]
    public bool respawnable = true;

    void Start()
    {
        // Cubeの見た目を変えるためにRendererを取得
        cubeRenderer = GetComponent<MeshRenderer>();
        if (cubeRenderer != null)
        {
            originalColor = cubeRenderer.material.color;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            PickupPromptUI.Instance.Show("Eキー：取得"); // Singleton or UIへの参照を持つ
            if (cubeRenderer != null)
            {
                cubeRenderer.material.color = highlightColor;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            PickupPromptUI.Instance.Hide();
            if (cubeRenderer != null)
            {
                cubeRenderer.material.color = originalColor;
            }
        }
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (itemData != null)
            {
                InventoryManager.Instance.AddMaterial(itemData, itemData.amountPerPickup);
                // UI更新（オプション）
                InventoryUIManager.Instance.OnItemAdded(itemData);
                //Debug.Log($"{itemData.itemName} を {itemData.amountPerPickup} 個拾いました");
                PickupPromptUI.Instance.Hide();
                gameObject.SetActive(false);
                isPlayerInRange = false;
                cubeRenderer.material.color = originalColor;
            }
        }
    }
}
