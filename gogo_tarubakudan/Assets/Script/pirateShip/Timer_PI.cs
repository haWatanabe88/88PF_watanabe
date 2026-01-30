using TMPro;
using UnityEngine;

public class Timer_PI : MonoBehaviour
{
    bool is_anim;
    bool is_game_clear;
    float timer = 0f;
    public float displayTime { get; private set; }
    TextMeshProUGUI timerText;
    PirateShip ship;
    public bool is_game_start { get; private set; }
    public void setIsGameStart(bool val)
    {
        is_game_start = val;
    }

    private void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        ship = GameObject.FindWithTag("pirate_ship").GetComponent<PirateShip>();
    }

    void Update()
    {
        if (!AnimationManager.Instance.getIsCompHpBarAnim())
            return;
        updateTimerUI();
    }

    void updateTimerUI()
    {
        if (!ship.getIsDefeat())
        {
            timer += Time.deltaTime;
            displayTime = Mathf.Floor(timer * 10f) / 10f;
            timerText.text = displayTime.ToString("0.0");
        }
    }
}
