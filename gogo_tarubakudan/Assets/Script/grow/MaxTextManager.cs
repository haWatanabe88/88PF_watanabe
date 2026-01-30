using DG.Tweening;
using UnityEngine;

public class MaxTextManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.DOScale(new Vector3(0.42f, 0.42f, 0.42f), 1f).SetLoops(-1, LoopType.Yoyo);
        transform.GetComponent<CanvasGroup>().DOFade(1f, 1f).SetLoops(-1, LoopType.Yoyo);
    }

    public void maxTexttweenKill()
    {
        transform.DOKill();
        GetComponent<CanvasGroup>().DOKill();
    }
}
