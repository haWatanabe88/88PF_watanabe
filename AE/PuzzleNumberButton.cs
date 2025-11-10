//numberパズルにて、クリック対象となる数字が書かれているボタンに関するスクリプト
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleNumberButton : MonoBehaviour
{
    public int number; // このボタンの数字
    private NumberPuzzleManager manager; // 管理者（親）

    private Button button;
    private Image buttonImage; // ボタンの背景色を変えるため

    private Color defaultColor = Color.white; // 元の色
    private Color selectedColor = Color.yellow; // 選択された色

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();

        if (button != null)
        {
            button.onClick.AddListener(OnButtonClicked);
        }

        if (buttonImage != null)
        {
            defaultColor = buttonImage.color; // 初期色を保存
        }
    }

    public void Initialize(NumberPuzzleManager puzzleManager, int assignedNumber)
    {
        manager = puzzleManager;
        number = assignedNumber;

        TMP_Text text = GetComponentInChildren<TMP_Text>();
        if (text != null)
        {
            text.text = number.ToString();
        }
    }

    private void OnButtonClicked()
    {
        if (manager != null)
        {
            manager.OnNumberClicked(this);
            SetSelectedColor();
        }
    }

    public void SetSelectedColor()
    {
        if (buttonImage != null)
        {
            buttonImage.color = selectedColor;
        }
    }

    public void ResetColor()
    {
        if (buttonImage != null)
        {
            buttonImage.color = defaultColor;
        }
    }
}
