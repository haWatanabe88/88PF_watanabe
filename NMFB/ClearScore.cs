using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearScore : MonoBehaviour
{
    [SerializeField] Score scoreScript;
    [SerializeField] float resultScore;
    public float tmpScore;
    public TMPro.TMP_Text scoreText;
    public TMPro.TMP_Text restartText;
    public GameObject RankMedal;
    [SerializeField] Sprite PlatinumMedal;
    [SerializeField] Sprite GoldMedal;
    [SerializeField] Sprite SilverMedal;
    [SerializeField] Sprite BronzeMedal;
    [SerializeField] GeneralAudioSource generalAudioSrc;
    public SpriteRenderer medalSprite;
    public bool isSE = false;


    private void Start()
    {
        tmpScore = 0;
        scoreText.SetText("0");
        scoreText.enabled = false;
        restartText.enabled = false;
        medalSprite = RankMedal.GetComponent<SpriteRenderer>();
        medalSprite.sprite = default;
    }

    private void Update()
    {
        if(Player.instance.isClear)
        {
            scoreText.enabled = true;
            resultScore = scoreScript.Sumscore;
            StartCoroutine("CountUP");
        }
    }

    IEnumerator CountUP()
    {
        while(tmpScore <= resultScore)
        {
            scoreText.SetText("{0}", Mathf.Round(tmpScore * 100.0f) / 100);
            tmpScore++;
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine("MedalActive");
        restartText.enabled = true;
    }

    IEnumerator MedalActive()
    {
        if(resultScore >= 75000)
        {
            medalSprite.sprite = PlatinumMedal;
            if(!isSE)
            {
                generalAudioSrc.PlatinumMedalSE();
                isSE = true;
            }
        }
        else if(resultScore >= 60000)
        {
            medalSprite.sprite = GoldMedal;
            if (!isSE)
            {
                generalAudioSrc.GoldMedalSE();
                isSE = true;
            }
        }
        else if (resultScore >= 40000)
        {
            medalSprite.sprite = SilverMedal;
            if (!isSE)
            {
                generalAudioSrc.SilverMedalSE();
                isSE = true;
            }
        }
        else if (resultScore < 40000)
        {
            medalSprite.sprite = BronzeMedal;
            if (!isSE)
            {
                generalAudioSrc.BronzeMedalSE();
                isSE = true;
            }
        }
        yield return new WaitForSeconds(0.1f);
        RankMedal.SetActive(true);
    }
}
