using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerText : MonoBehaviour
{
    float LIMIT_TIME = 35f;
    float current_time;
    bool is_running = false;

    //[SerializeField] GameObject timer_text_obj;
    [SerializeField] GameObject max_text_Manager;
    TextMeshProUGUI timer_text;
    GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        timer_text = GetComponent<TextMeshProUGUI>();
        current_time = LIMIT_TIME;
        updateText(); // 初期表示だけやる
    }

    void Update()
    {
        if (!is_running) return;

        current_time -= Time.deltaTime;

        if (current_time <= 0)
        {
            current_time = 0;
            is_running = false;
            timerFinished();
        }

        updateText();
    }

    public void startTimer()
    {
        is_running = true;
    }

    public void stopTimer()
    {
        is_running = false;
    }

    private void updateText()
    {
        timer_text.text = current_time.ToString("F1"); // 小数1桁
    }

    private void timerFinished()
    {
        Debug.Log("Finish!");
        // 他のイベント呼ぶならここ
        player.GetComponent<Rigidbody>().linearVelocity = new Vector3(0f, 0f, 0f);
        max_text_Manager.GetComponent<MaxTextManager>().maxTexttweenKill();
        player.GetComponent<PlayerRestrict>().setEnable(player.GetComponent<PlayerMovement>(), false);
        StartCoroutine(SceneFlowManager.Instance.questLoador());
    }

    public float getCurrentTime()
    {
        return current_time;
    }

    public float getLimitTime()
    {
        return LIMIT_TIME;
    }
}
