using UnityEngine;
using UnityEngine.UI;

public class SlideManager : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] GameObject slide_panel;
    [SerializeField] GameObject slide_obj;
    [SerializeField] GameObject previous_btn;
    [SerializeField] GameObject next_btn;
    [SerializeField] GameObject close_btn;
    Image slideImage;
    int slideIndex;

    void Start()
    {
        slideIndex = 0;
        slide_panel.SetActive(false);
        slideImage = slide_obj.GetComponent<Image>();
    }

    void Update()
    {
        slideImage.sprite = sprites[slideIndex];
        previous_btn.SetActive(true);
        next_btn.SetActive(true);
        if (slideIndex == 0) previous_btn.SetActive(false);
        if (slideIndex == sprites.Length - 1) next_btn.SetActive(false);

    }

    public void previousPage()
    {
        if(slideIndex > 0)
            slideIndex--;
        SoundManager.Instance.PlaySE("manualPageMove_title");
    }

    public void nextPage()
    {
        if(slideIndex < sprites.Length - 1)
            slideIndex++;
        SoundManager.Instance.PlaySE("manualPageMove_title");
    }

    public void closePage()
    {
        SoundManager.Instance.PlaySE("manualPageMove_title");
        Time.timeScale = 1f;//マニュアルを開いたときにTime.deltaTimeをTime.timescaleを０にしているので、それをマニュアルを閉じたタイミングで元に戻している
        hideSlidePannel();
        inisialize();
    }

    void inisialize()
    {
        slideIndex = 0;
    }

    public void showSlidePannel()
    {
        slide_panel.SetActive(true);
    }

    void hideSlidePannel()
    {
        slide_panel.SetActive(false);
    }
}
