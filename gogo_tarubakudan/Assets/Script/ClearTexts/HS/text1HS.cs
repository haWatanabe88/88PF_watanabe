using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class text1HS : MonoBehaviour
{
    public float text1_hs_score { get; private set; }
    TextMeshProUGUI contentText;
    [SerializeField] HighSpeedManager hs_manager;
    float ESTIMATED_TIME = 60f;
    float current_time;



    private void Start()
    {
        contentText = GetComponent<TextMeshProUGUI>();
        text1_hs_score = 0;
    }

    void Update()
    {
        calcScore();
        contentText.text = $"クリアタイム：{text1_hs_score}";
    }

    void calcScore()
    {
        current_time = hs_manager.GetComponent<HighSpeedManager>().displayTime;
        if (ESTIMATED_TIME <= current_time)
        {
            current_time = ESTIMATED_TIME;
        }
        text1_hs_score = (ESTIMATED_TIME - current_time) * 250f;
    }
}

