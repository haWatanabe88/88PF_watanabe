using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] public TMPro.TMP_Text scoreText;

    public float Sumscore;

    void Start()
    {
        scoreText.SetText("SCORE:0");
        Sumscore = 0;
    }

    public void GetScoreConversion(float score)
    {
        if(Player.instance.currentMode == Player.Mode.MUSCLE)
        {
            score *= 1.5f;
        }
        Sumscore += score;
        scoreText.SetText("SCORE:{0}", Mathf.Round(Sumscore * 100.0f) / 100);
    }

    public void AddScore(float score)
    {
        Sumscore += score;
        if (Sumscore <= 0)
        {
            Sumscore = 0;
        }
        scoreText.SetText("SCORE:{0}", Mathf.Round(Sumscore * 100.0f) / 100);
    }
}
