using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    [SerializeField] GameObject[] bullet_icons;
    [SerializeField] RelatedPirateInformText inform_text;
    [SerializeField] GameObject manualManager;
    CannonMoveAuto cannon_move_auto;
    int index;
    public int number_of_bullet { get; private set; }
    int MAX_BULLET_NUM = 3;
    bool is_anim;
    public bool is_gameover { get; private set; }
    public void setIsGameOver(bool val)
    {
        is_gameover = val;
    }
    private void Start()
    {
        index = 0;
        is_anim = false;
        is_gameover = false;
        number_of_bullet = MAX_BULLET_NUM;
        cannon_move_auto = GameObject.FindWithTag("cannon").GetComponent<CannonMoveAuto>();
    }

    private void Update()
    {
        if (SceneFlowManager.Instance.IsExplosionScene && !is_anim)
        {
            is_anim = true;
            AnimationManager.Instance.bootStartTextAnimation();
        }
    }

    public void hideBulletIcon()
    {
        bullet_icons[index].SetActive(false);
        index++;
        number_of_bullet--;
        if (AreAllObjectsInactive())
        {
            informGameOver();
        }
    }
    private bool AreAllObjectsInactive()
    {
        foreach (GameObject obj in bullet_icons)
        {
            // 1つでもアクティブなオブジェクトがあれば、falseを返してループを抜ける
            // obj が null でないことを確認する（Destroy済みの場合があるため）
            if (obj != null && obj.activeSelf)
            {
                return false;
            }
        }

        // すべてのオブジェクトをチェックし終えて、アクティブなものが1つもなければ true を返す
        return true;
    }

    public void informGameOver()
    {
        //ゲームオーバーの合図をだす
        setIsGameOver(true);
        inform_text.ActiveText();
        manualManager.GetComponent<ManualManager>().hideManualCanvas();
        cannon_move_auto.enabled = false;
    }
}
