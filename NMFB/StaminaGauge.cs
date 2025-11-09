using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaGauge : MonoBehaviour
{
    public int maxStamina;
    public int currentStamina;
    public Slider staminaGaugeSlider;
    private float time;
    private float decreaseFixedTime;//一定時間ごとに自然減少していく
    private int decreaseAmount;
    public bool isStart = true;

    public GameObject fill;
    public Color fillColor;

    void Start()
    {
        maxStamina = 100;
        //Sliderを最大にする。
        decreaseFixedTime = 5.0f;//本来は5.0f;
        decreaseAmount = 5;//本来は5
        staminaGaugeSlider.value = 100;
        //HPを最大HPと同じ値に。
        currentStamina = maxStamina;
        time = decreaseFixedTime;//本来は5
        isStart = true;
        fillColor = fill.GetComponent<Image>().color;
    }

    void Update()
    {
        if(!Player.instance.isClear)
        {
            if (isStart == true && currentStamina < maxStamina * 0.8f)//7割の0.7→（変更）8割,,,85に被らないようにとりあえず調整
            {
                isStart = false;
            }
            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
            if (currentStamina > 0)
            {
                Player.instance.isAllOut = false;
                time -= Time.deltaTime;
                if (time < 0)
                {
                    time = decreaseFixedTime;
                    if (currentStamina > 0)
                    {
                        currentStamina = currentStamina - decreaseAmount;
                    }
                }
            }
            else
            {
                time = decreaseFixedTime;//いらんかもしれないが、一応、念の為きっちり5s確保するため
                currentStamina = 0;
                Player.instance.isAllOut = true;
            }
            staminaGaugeSlider.value = currentStamina;
        }
    }

    public IEnumerator BlinkStaminaGauge()
    {
        //Debug.Log("スタミナゲージ点滅");
        float interval = 0.5f;
        int blinkCount = 10;
        fill.GetComponent<Image>().color = fillColor;
        for (int i = 0; i < blinkCount; i++)
        {
            if(fillColor.a == 0.5f)
            {
                fillColor.a = 1.0f;
            }
            else if(fillColor.a == 1.0f)
            {
                fillColor.a = 0.5f;
            }
            yield return new WaitForSeconds(interval);
            fill.GetComponent<Image>().color = fillColor;
        }
    }

    public void StaminaGaugeColorReset()
    {
        fillColor.a = 1.0f;
        fill.GetComponent<Image>().color = fillColor;
    }
}
