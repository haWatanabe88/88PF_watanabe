using UnityEngine;
using UnityEngine.UI;

public class EnergyUI : MonoBehaviour
{
    Image image;
    PlayerItemPickup player_item_pickup;
    int MAX_ITEM_NUM;

    void Start()
    {
        player_item_pickup = GameObject.FindWithTag("Player").GetComponent<PlayerItemPickup>();
        image = GetComponent<Image>();
        MAX_ITEM_NUM = player_item_pickup.MAX_OBTAIN_ITEM_NUM;
        image.fillAmount = 0;
        //Debug.Log(((int)((1f / (float)MAX_ITEM_NUM) * 1000))/1000f;
    }

    public void addFillAmount()
    {
        image.fillAmount += 1f / (float)MAX_ITEM_NUM;
    }

    public void decreseFillAmount()
    {
        image.fillAmount -= 1f / (float)MAX_ITEM_NUM;
    }
}
