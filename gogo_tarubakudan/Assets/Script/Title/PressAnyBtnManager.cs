using DG.Tweening;
using TMPro;
using UnityEngine;

public class PressAnyBtnManager : MonoBehaviour
{
    CanvasGroup canvasGroup;
    [SerializeField]  GameObject titleManager;

    void OnDisable()
    {
        GetComponent<CanvasGroup>().DOKill();
    }

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        transform.GetComponent<CanvasGroup>().DOFade(1f, 1f).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
    }

    void Update()
    {
        if (titleManager.GetComponent<TitleManager>().is_press_any_key)
        {
            this.gameObject.SetActive(false);
        }   
    }
}
