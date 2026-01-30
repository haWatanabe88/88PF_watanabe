using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class text1PI : MonoBehaviour
{
    public float text1_pi_score { get; private set; }
    TextMeshProUGUI contentText;
    [SerializeField] GameObject timerText;
    float ESTIMATED_TIME = 90f;
    float current_time;



    private void Start()
    {
        contentText = GetComponent<TextMeshProUGUI>();
        text1_pi_score = 0;
    }

    void Update()
    {
        calcScore();
        contentText.text = $"クリアタイム：{text1_pi_score}";
    }

    void calcScore()
    {
        current_time = timerText.GetComponent<Timer_PI>().displayTime;
        if (ESTIMATED_TIME <= current_time)
        {
            current_time = ESTIMATED_TIME;
        }
        text1_pi_score = (ESTIMATED_TIME - current_time) * 200f;
    }
}

