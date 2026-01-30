using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.BoolParameter;

public class HighSpeedManager : MonoBehaviour
{
    public int sum_score {  get; private set; }
    int add_score = 500;//テキトー
    float timer;
    public float displayTime { get; private set; }
    bool is_game_clear;
    bool is_anim;
    public int bonum_item_count {  get; private set; }
    [SerializeField] RelatedPirateInformText inform_text;
    [SerializeField] TextMeshProUGUI bonus_score_ui;
    [SerializeField] TextMeshProUGUI timerText;
    public bool is_game_start { get; private set; }
    public void setIsGameStart(bool val)
    {
        is_game_start = val;
    }

    private void Start()
    {
        is_game_clear = false;
        is_game_start = false;
        is_anim = false;
        timer = 0;
        displayTime = 0;
        setIsGameStart(false);
        sum_score = 0;
        bonum_item_count = 0;
        timerText.text = displayTime.ToString("0.0");
    }

    void Update()
    {
        if (SceneFlowManager.Instance.IsHighSpeedScene && !is_anim)
        {
            is_anim = true;
            AnimationManager.Instance.bootStartTextAnimation();
        }
        if (is_game_start)
        {
            updateTimerUI();
            updateBonusScoreUI();
        }
    }

    public void addScore()
    {
        sum_score += add_score;
        bonum_item_count++;
    }

    void updateTimerUI()
    {
        if (!is_game_clear)
        {
            timer += Time.deltaTime;
            displayTime = Mathf.Floor(timer * 10f) / 10f;
            timerText.text = displayTime.ToString("0.0");
        }
    }
    void updateBonusScoreUI()
    {
        if (!is_game_clear)
        {
            bonus_score_ui.text = "Bonus: " + sum_score.ToString();
        }
    }

    public void activeGameClearInfo()
    {
        Debug.Log("ゲームクリア");
        is_game_clear = true;
        inform_text.ActiveText();
    }
}
