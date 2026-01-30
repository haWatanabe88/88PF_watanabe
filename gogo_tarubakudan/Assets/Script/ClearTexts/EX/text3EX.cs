using TMPro;
using UnityEngine;

public class text3EX : MonoBehaviour
{
    [SerializeField] GameObject text1;
    [SerializeField] GameObject text2;
    TextMeshProUGUI contentText;
    float text1_ex_score;
    float text2_ex_score;
    int total_score;
    public int getTotalScore() { return total_score; }

    private void Start()
    {
        contentText = GetComponent<TextMeshProUGUI>();

    }

    void Update()
    {
        calcScore();
        contentText.text = $"総スコア：{text1_ex_score} + {text2_ex_score} = {total_score}";
    }

    void calcScore()
    {
        text1_ex_score = text1.GetComponent<text1EX>().text1_ex_score;
        text2_ex_score = text2.GetComponent<text2EX>().text2_ex_score;
        total_score = Mathf.RoundToInt(text1_ex_score + text2_ex_score);
    }

}
