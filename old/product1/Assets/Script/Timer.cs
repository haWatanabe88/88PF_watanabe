/*
 時間を管理するためのもの
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //TextMeshProを扱う際に必要

public class Timer : MonoBehaviour
{
    public float maxTime = 15.0f;//ゲームプレイ時間
    [SerializeField] TextMeshProUGUI timerText;
    GameObject gd;
    // Start is called before the first frame update
    void Start()
    {
        gd = GameObject.Find("GameDirector");
    }

    // Update is called once per frame
    void Update()
    {
        GameDirector gd_class = gd.GetComponent<GameDirector>();
        if (gd_class._isStart == true)
        {
            timerText.text = "Time:" + maxTime.ToString("F1");
            maxTime -= Time.deltaTime;
            if (maxTime <= 0.0f)//ゲーム終了した合図
            {
                maxTime = 0.0f;
                gd_class._isStart = false;
                gd_class._isEnd = true;
            }
        }
    }
}
