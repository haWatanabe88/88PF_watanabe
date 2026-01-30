using UnityEngine;
using DG.Tweening;
using TMPro;

public class StartTextAnimation : MonoBehaviour
{
    TextMeshProUGUI startText;

    void Start()
    {
        startText = GetComponent<TextMeshProUGUI>();

        // ‰Šúó‘Ô
        startText.alpha = 1f;

        Sequence seq = DOTween.Sequence();

        seq.Append(
            startText.transform.DOScale(2.5f, 0.5f)
                .SetEase(Ease.InOutQuart)
        );
        seq.Append(
            startText.transform.DOScale(0.3f, 0.2f)
        );
        seq.Join(
            startText.DOFade(0f, 0.2f)
        );
        seq.OnComplete(() =>
        {
            startText.gameObject.SetActive(false);
        });
    }
}
