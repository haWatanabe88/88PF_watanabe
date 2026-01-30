using DG.Tweening;
using System.Collections;
using TMPro;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class AnimationManager : MonoBehaviour
{

    [SerializeField] GameObject whiteWall;
    [SerializeField] TextMeshProUGUI startText;
    Image whiteOutWall_image;
    bool is_complete_hpbar_anim;
    public bool getIsCompHpBarAnim() { return is_complete_hpbar_anim; }

    public static AnimationManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        is_complete_hpbar_anim = false;
        whiteOutWall_image = whiteWall.GetComponent<Image>();
        startText.alpha = 0f;
    }

    public void completeHpBarAnimation()
    {
        Debug.Log("スタートテキストアニメ");
        Invoke(nameof(startTextAnimation), 1f);
    }

    void isCompleteAnim()
    {
        is_complete_hpbar_anim = true;
    }

    void startTextAnimation()
    {
        // 初期状態
        startText.alpha = 1f;
        SoundManager.Instance.PlaySE("start_common");
        Sequence seq = DOTween.Sequence();

        seq.Append(
            startText.transform.DOScale(6f, 0.5f)
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
            isCompleteAnim();
        });
    }

    public void bootStartTextAnimation()
    {
        startTextAnimation();
    }


    public IEnumerator activeWhiteWall(float start_time, float duration)
    {
        yield return new WaitForSeconds(start_time);
        whiteOutWall_image.DOFade(1f, duration);
        yield return new WaitForSeconds(duration); //完了を待つ
    }

    public IEnumerator inactiveWhiteWall(float start_time, float duration)
    {
        yield return new WaitForSeconds(start_time);
        whiteOutWall_image.DOFade(0f, duration);
        yield return new WaitForSeconds(duration); //完了を待つ
    }

    public void inisialize()
    {
        is_complete_hpbar_anim = false;
        startText.alpha = 0f;
        startText.gameObject.SetActive(true);
    }
}
