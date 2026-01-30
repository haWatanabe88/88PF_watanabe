using DG.Tweening;
using UnityEngine;

public class TitleLogo : MonoBehaviour
{
    [SerializeField] GameObject press_any_btn_text;
    void Start()
    {
        Debug.Log("OK");
        transform.DOLocalMoveY(0f, 1f).SetEase(Ease.OutBounce).SetUpdate(true).OnComplete(showPressAnyBtnText);
    }

    void showPressAnyBtnText()
    {
        press_any_btn_text.SetActive(true);
    }
}