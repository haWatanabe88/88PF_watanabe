using UnityEngine;
using UnityEngine.UI;

public class TestButtonTrigger : MonoBehaviour
{
    public ItemEffectHandler handler;
    public CraftedItemSO seeThroughItem;

    public void TriggerTest()
    {
        handler.UseItem(seeThroughItem);
    }
}
