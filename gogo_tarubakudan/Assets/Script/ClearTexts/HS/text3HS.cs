using TMPro;
using UnityEngine;

public class text3HS : MonoBehaviour
{
    [SerializeField] GameObject text1;
    [SerializeField] GameObject text2;
    TextMeshProUGUI contentText;
    float text1_hs_score;
    float text2_hs_score;
    int total_score;
    public int getTotalScore() { return total_score; }
    private void Start()
    {
        contentText = GetComponent<TextMeshProUGUI>();

    }

    void Update()
    {
        calcScore();
        contentText.text = $"総スコア：{text1_hs_score} + {text2_hs_score} = {total_score}";
    }

    void calcScore()
    {
        text1_hs_score = text1.GetComponent<text1HS>().text1_hs_score;
        text2_hs_score = text2.GetComponent<text2HS>().text2_hs_score;
        total_score = Mathf.RoundToInt(text1_hs_score + text2_hs_score);
    }

}
