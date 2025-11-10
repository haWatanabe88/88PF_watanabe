using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MaterialItemDisplay : MonoBehaviour
{
    public MaterialItemSO itemData;

    public TextMeshProUGUI itemNameText;
    public Image itemIconImage;
    public TextMeshProUGUI amountText;

    void Start()
    {
        if (itemData != null)
        {
            itemNameText.text = itemData.itemName;
            itemIconImage.sprite = itemData.icon;
            amountText.text = $"x{itemData.amountPerPickup}";
        }
    }
}
