// HeavySwitch.cs
using UnityEngine;
using System.Collections.Generic;

public class HeavySwitch : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> linkedObjects; // ← ここに開けるドアとか壊すオブジェクトを登録する

    public void ActivateSwitch()
    {
        foreach (var obj in linkedObjects)
        {
            Destroy(obj); // または obj.SetActive(false) でもOK
        }
    }
}
