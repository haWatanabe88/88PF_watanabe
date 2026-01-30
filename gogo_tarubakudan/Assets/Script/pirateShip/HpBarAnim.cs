using DG.Tweening;	//DOTween‚ðŽg‚¤‚Æ‚«‚Í‚±‚Ìusing‚ð“ü‚ê‚é
using UnityEngine;
using UnityEngine.UI;

public class HpBarAnim : MonoBehaviour
{
    PirateShip pirate_ship;
    GameObject hp_bar_obj;
    Image hp_bar_image;
    bool is_scalable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        is_scalable = false;
        this.gameObject.SetActive(true);
        pirate_ship = GameObject.FindWithTag("pirate_ship").GetComponent<PirateShip>();
        hp_bar_obj = transform.GetChild(0).gameObject;
        hp_bar_image = hp_bar_obj.GetComponent<Image>();
    }

    void Update()
    {
        if (SceneFlowManager.Instance.IsPirateShipScene && !is_scalable)
        {
            is_scalable = true;
            transform.DOScale(new Vector3(18f, 0.5f, 1f), 2f)
                .OnComplete(() => AnimationManager.Instance.completeHpBarAnimation());
        }
        decreaseHpBar();
        inActive();
    }

    void decreaseHpBar()
    {
        hp_bar_image.fillAmount = pirate_ship.getHP() / 100f;
    }

    void inActive()
    {
        if(pirate_ship.getIsDefeat())
            this.gameObject.SetActive(false);
    }
}
