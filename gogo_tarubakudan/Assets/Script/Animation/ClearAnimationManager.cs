using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClearAnimationManager : MonoBehaviour
{
    [SerializeField] GameObject black_bg;
    [SerializeField] GameObject text1;
    [SerializeField] GameObject text2;
    [SerializeField] GameObject text3;
    [SerializeField] GameObject rank_icon;
    [SerializeField] GameObject restart_btn;
    [SerializeField] Sprite[] rank_icon_sprites;
    Image rank_icon_image;
    enum RANK_ICON_TYPE
    {
        PLATINUM,
        GOLD,  
        SILVER,
    }
    bool is_just_once;
    private void Start()
    {
        black_bg.SetActive(false);
        text1.SetActive(false);
        text2.SetActive(false);
        text3.SetActive(false);
        rank_icon.SetActive(false);
        rank_icon_image = rank_icon.GetComponent<Image>();
        restart_btn.GetComponent<CanvasGroup>().alpha = 0;
    }

    public void clearAnimation()
    {
        if (!is_just_once)
        {
            is_just_once = true;
            StartCoroutine(showScore());
        }
    }

    IEnumerator showScore()
    {
        yield return new WaitForSeconds(0.5f);
        black_bg.SetActive(true);
        SoundManager.Instance.PlaySE("blackPanel_common");
        restart_btn.GetComponent<CanvasGroup>().DOFade(1, 1f);
        yield return new WaitForSeconds(0.5f);
        text1.SetActive(true);
        SoundManager.Instance.PlaySE("showtext_common");
        yield return new WaitForSeconds(0.5f);
        text2.SetActive(true);
        SoundManager.Instance.PlaySE("showtext_common");
        yield return new WaitForSeconds(1.5f);
        text3.SetActive(true);
        SoundManager.Instance.PlaySE("showtext_common");
        yield return new WaitForSeconds(1f);
        dicideRankIcon();
        soundOfEachIcon();
        rank_icon.SetActive(true);
    }

    void dicideRankIcon()
    {
        if (SceneManager.GetActiveScene().name == "pirateShip")
        {
            int result = text3.GetComponent<text3PI>().getTotalScore();
            if(result >= 12000)
            {
                rank_icon_image.sprite = rank_icon_sprites[(int)RANK_ICON_TYPE.PLATINUM];
            }
            else if(result <= 8000)
            {
                rank_icon_image.sprite = rank_icon_sprites[(int)RANK_ICON_TYPE.SILVER];
            }
            else
            {
                rank_icon_image.sprite = rank_icon_sprites[(int)RANK_ICON_TYPE.GOLD];
            }
        }
        else if(SceneManager.GetActiveScene().name == "explosion")
        {
            int result = text3.GetComponent<text3EX>().getTotalScore();
            if (result >= 25500)
            {
                rank_icon_image.sprite = rank_icon_sprites[(int)RANK_ICON_TYPE.PLATINUM];
            }
            else if (result <= 10000)
            {
                rank_icon_image.sprite = rank_icon_sprites[(int)RANK_ICON_TYPE.SILVER];
            }
            else
            {
                rank_icon_image.sprite = rank_icon_sprites[(int)RANK_ICON_TYPE.GOLD];
            }
        }
        else if (SceneManager.GetActiveScene().name == "highSpeedRun")
        {
            int result = text3.GetComponent<text3HS>().getTotalScore();
            if (result >= 15000)
            {
                rank_icon_image.sprite = rank_icon_sprites[(int)RANK_ICON_TYPE.PLATINUM];
            }
            else if (result <= 5000)
            {
                rank_icon_image.sprite = rank_icon_sprites[(int)RANK_ICON_TYPE.SILVER];
            }
            else
            {
                rank_icon_image.sprite = rank_icon_sprites[(int)RANK_ICON_TYPE.GOLD];
            }
        }
    }
    
    void soundOfEachIcon()
    {
        if (rank_icon_image.sprite == rank_icon_sprites[(int)RANK_ICON_TYPE.PLATINUM])
        {
            SoundManager.Instance.PlaySE("platinum_common");
        }
        else if (rank_icon_image.sprite == rank_icon_sprites[(int)RANK_ICON_TYPE.GOLD])
        {
            SoundManager.Instance.PlaySE("gold_common");
        }
        else if (rank_icon_image.sprite == rank_icon_sprites[(int)RANK_ICON_TYPE.SILVER])
        {
            SoundManager.Instance.PlaySE("silver_common");
        }
    }
}
