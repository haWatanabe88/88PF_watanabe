using TMPro;
using UnityEngine;

public class text2HS : MonoBehaviour
{
    public float text2_hs_score { get; private set; }
    TextMeshProUGUI contentText;
    [SerializeField] HighSpeedManager hs_manager;
    int bonus_item_count;

    private void Start()
    {
        bonus_item_count = 0;
        contentText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        calcScore();
        contentText.text = $"ボーナスアイテム（{bonus_item_count}個）：{text2_hs_score}";
    }

    void calcScore()
    {
        text2_hs_score = hs_manager.sum_score;
        bonus_item_count = hs_manager.bonum_item_count;
    }
}
