using UnityEngine;
using TMPro;
using NUnit.Framework.Constraints;
public class RelatedGrowInformText : MonoBehaviour
{
    bool is_running = false;
    bool is_start;
    bool is_timeup;
    float COUNTDOWN_TIME = 4f;
    float current_time;
    TextMeshProUGUI inform_text;
    [SerializeField] GameObject timer_text_obj;
    TimerText timer_text_component;
    GameObject player;
    void Start()
    {
        is_start = false;
        is_timeup = false;
        player = GameObject.FindWithTag("Player");
        player.GetComponent<PlayerRestrict>().setEnable(player.GetComponent<PlayerMovement>(), false);
        timer_text_component = timer_text_obj.GetComponent<TimerText>();
        inform_text = GetComponent<TextMeshProUGUI>();
        current_time = COUNTDOWN_TIME;
        countDownText(); // 初期表示だけやる
    }

    void Update()
    {
        if (!is_running)
        {
            countDownText();
            return;
        }
        informTextUpdate();
    }

    public void startTimer()
    {
        is_running = true;
    }

    public void stopTimer()
    {
        is_running = false;
    }

    void countDownText()
    {
        current_time -= Time.deltaTime * 2;
        inform_text.text = Mathf.FloorToInt(current_time).ToString();
        if (current_time <= 1)
        {
            current_time = 1;
            is_running = true;
            inform_text.text = "";
            timer_text_component.startTimer();
            player.GetComponent<PlayerRestrict>().setEnable(player.GetComponent<PlayerMovement>(),true);
        }
    }

    void informTextUpdate()
    {
        if(timer_text_component.getCurrentTime() >= timer_text_component.getLimitTime() - 0.5f)
        {
            if (!is_start)
            {
                is_start = true;
                SoundManager.Instance.PlaySE("start_grow");
            }
            inform_text.text = "START!!";
        }
        else if(timer_text_component.getCurrentTime() <= 0f)
        {
            if (!is_timeup)
            {
                is_timeup = true;
                SoundManager.Instance.PlaySE("timeup_grow");
            }
            inform_text.text = "TIME UP";
        }
        else
        {
            inform_text.text = "";
        }
    }

    private void timerFinished()
    {
        Debug.Log("Finish!");
        // 他のイベント呼ぶならここ
    }
}
