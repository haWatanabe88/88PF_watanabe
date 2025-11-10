using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;

public class SwitchPuzzleManager : MonoBehaviour
{
    [SerializeField] private List<SwitchButton> switches; // 3つのスイッチ
    [SerializeField] private GameObject door; // 扉
    [SerializeField] private List<int> correctOrder; // 正しい押す順番（例: 0, 1, 2）

    private List<int> pressedOrder = new List<int>(); // プレイヤーが押した順番

    private void Update()
    {
        //Debug.Log("Pressed:" + pressedOrder.Count);
        //Debug.Log("Correct:" + correctOrder.Count);
    }


    // スイッチが押されたら呼び出される
    public void PressSwitch(SwitchButton switchButton)
    {
        int index = switches.IndexOf(switchButton);
        if (index == -1)
        {
            Debug.LogWarning("押したスイッチがセットに存在しない！");
            return;
        }

        pressedOrder.Add(index);

        // すべて押し終わったら判定
        if (pressedOrder.Count == correctOrder.Count)
        {
            if (IsCorrectOrder())
            {
                //Debug.Log("正解！扉を開きます！");
                OpenDoor();
            }
            else
            {
                //Debug.Log("間違い！リセットします！");
                ResetPuzzle();
            }
        }
    }

    private bool IsCorrectOrder()
    {
        for (int i = 0; i < correctOrder.Count; i++)
        {
            if (pressedOrder[i] != correctOrder[i])
                return false;
        }
        return true;
    }

    private void OpenDoor()
    {
        Destroy(door); // ここは好きに演出変えてOK（SetActive(false)でもアニメでも）
    }

    private void ResetPuzzle()
    {
        pressedOrder.Clear();

        // 🎯 押されたスイッチをリセットする
        foreach (var sw in switches)
        {
            sw.SetPressedState(false);
            sw.isReset = true;
        }
    }
}
