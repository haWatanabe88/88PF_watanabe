using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RestartBtn : MonoBehaviour
{
    [SerializeField] PlayerPhysicsDataSO playerPhysicsData;
    private void Start()
    {
        transform.DOScale(new Vector3(0.15f, 0.6f, 0.6f), 1f).SetLoops(-1, LoopType.Yoyo);
    }

    public void selectSceneLoad()
    {
        StartCoroutine(selectSceneLoadFlow());
    }

    IEnumerator selectSceneLoadFlow()
    {
        SoundManager.Instance.PlaySE("restart_common");
        yield return AnimationManager.Instance.activeWhiteWall(0f, 2f);
        inisialize();
        SceneManager.LoadScene("select");
        transform.DOKill();
    }

    void inisialize()
    {
        SceneFlowManager.Instance.inisialize();
        AnimationManager.Instance.inisialize();
        playerPhysicsData.clear();
    }

}
