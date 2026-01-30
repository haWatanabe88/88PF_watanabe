using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberPuzzleManager : MonoBehaviour
{
    [Header("数字ボタンのプレハブ")]
    public GameObject numberButtonPrefab;
    [Header("配置する親オブジェクト（枠）")]
    public Transform buttonParent;
    [Header("クリアすると開くドア")]
    public GameObject door;
    [Header("並べる数字たち（例：1〜10）")]
    public List<int> numbers = new List<int>();
    [Header("昇順ならtrue / 降順ならfalse")]
    public bool ascending = true;
    private List<int> clickedNumbers = new List<int>();
    private List<PuzzleNumberButton> clickedButtons = new List<PuzzleNumberButton>();
    [Header("指示を表示するテキスト")]
    public TMP_Text instructionText;
    [Header("パズルを開始させるスイッチ")]
    public NumberPuzzleSwitch puzzleSwitch;

    private void Start()
    {
        SpawnNumberButtons();
        UpdateInstruction();
    }

    private void UpdateInstruction()
    {
        if (instructionText != null)
        {
            instructionText.text = ascending ? "小さい順に押せ！" : "大きい順に押せ！";
        }
    }

    private void SpawnNumberButtons()
    {
        float spacingX = 350f; // 横間隔（広めに）
        float spacingY = 200f; // 縦間隔（広めに）
        int columns = 3; // 列数
        Vector2 startPos = new Vector2(-200f, 200f); // スタート位置

        for (int i = 0; i < numbers.Count; i++)
        {
            GameObject obj = Instantiate(numberButtonPrefab, buttonParent);
            PuzzleNumberButton button = obj.GetComponent<PuzzleNumberButton>();
            button.Initialize(this, numbers[i]);

            int row = i / columns;
            int col = i % columns;

            // 基本グリッド位置
            Vector2 pos = new Vector2(startPos.x + col * spacingX, startPos.y - row * spacingY);

            // 大きめにランダムでズラす
            float offsetX = Random.Range(-80f, 80f); // 横に±60ぐらい自由に
            float offsetY = Random.Range(-80f, 80f); // 縦も同様

            pos += new Vector2(offsetX, offsetY);

            obj.GetComponent<RectTransform>().anchoredPosition = pos;
        }
    }


    //public void RegisterClickedNumber(int number)
    //{
    //    clickedNumbers.Add(number);

    //    if (clickedNumbers.Count == numbers.Count)
    //    {
    //        CheckResult();
    //    }
    //}

    //private void CheckResult()
    //{
    //    bool isCorrect = true;

    //    for (int i = 0; i < clickedNumbers.Count - 1; i++)
    //    {
    //        if (ascending)
    //        {
    //            if (clickedNumbers[i] > clickedNumbers[i + 1])
    //            {
    //                isCorrect = false;
    //                break;
    //            }
    //        }
    //        else // 降順の場合
    //        {
    //            if (clickedNumbers[i] < clickedNumbers[i + 1])
    //            {
    //                isCorrect = false;
    //                break;
    //            }
    //        }
    //    }

    //    if (isCorrect)
    //    {
    //        Debug.Log("クリア！！ 扉を開きます！");
    //        OpenDoor();
    //    }
    //    else
    //    {
    //        Debug.Log("間違い！やり直し！");
    //        ResetPuzzle();
    //    }
    //}


    private void OpenDoor()
    {
        if (door != null)
        {
            Destroy(door);
        }
        else
        {
            Debug.LogWarning("ドアが設定されていません！");
        }
        if (puzzleSwitch != null)
        {
            puzzleSwitch.SetCleared();
        }
    }


    public void ResetPuzzle()
    {
        clickedNumbers.Clear();

        // 押したボタンの色をリセット
        foreach (var button in clickedButtons)
        {
            button.ResetColor();
        }
        clickedButtons.Clear();

        // ボタンを全削除
        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }

        SpawnNumberButtons();
    }


    public void OnNumberClicked(PuzzleNumberButton button)
    {
        clickedNumbers.Add(button.number);
        clickedButtons.Add(button);

        if (!CheckProgress()) // ここで押すたびに判定！
        {
            Debug.Log("間違えた！リセットします！");
            ResetPuzzle();
            return;
        }

        if (clickedNumbers.Count == numbers.Count)
        {
            Debug.Log("クリア！扉を開きます！");
            OpenDoor();
        }
    }

    private bool CheckProgress()
    {
        int currentIndex = clickedNumbers.Count - 1;

        if (currentIndex < 0 || currentIndex >= numbers.Count)
            return false; // 範囲外ならアウト

        int expectedNumber;

        if (ascending)
        {
            // 昇順：numbersを小さい順に並び替えたリスト
            List<int> sortedNumbers = new List<int>(numbers);
            sortedNumbers.Sort();

            expectedNumber = sortedNumbers[currentIndex];
        }
        else
        {
            // 降順：numbersを大きい順に並び替えたリスト
            List<int> sortedNumbers = new List<int>(numbers);
            sortedNumbers.Sort();
            sortedNumbers.Reverse();

            expectedNumber = sortedNumbers[currentIndex];
        }

        return clickedNumbers[currentIndex] == expectedNumber;
    }
}
