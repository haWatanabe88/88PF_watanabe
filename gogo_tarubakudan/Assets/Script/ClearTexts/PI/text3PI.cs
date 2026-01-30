using TMPro;
using UnityEngine;

public class text3PI : MonoBehaviour
{
    [SerializeField] GameObject text1;
    [SerializeField] GameObject text2;
    TextMeshProUGUI contentText;
    float text1_pi_score;
    float text2_pi_score;
    int total_score;
    public int getTotalScore() { return total_score; }
    private void Start()
    {
        contentText = GetComponent<TextMeshProUGUI>();

    }

    void Update()
    {
        calcScore();
        contentText.text = $"総スコア：{text1_pi_score} - {-text2_pi_score} = {total_score}";
    }

    void calcScore()
    {
        text1_pi_score = text1.GetComponent<text1PI>().text1_pi_score;
        text2_pi_score = text2.GetComponent<text2PI>().text2_pi_score;
        total_score = Mathf.RoundToInt(text1_pi_score + text2_pi_score);
    }

}
