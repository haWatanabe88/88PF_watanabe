using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFlowManager : MonoBehaviour
{
    public static SceneFlowManager Instance { get; private set; }

    public bool IstouchGameStartWall {  get; private set; }
    public bool IsdoneRoulette {  get; private set; }

    public bool IsPirateShipScene { get; private set; }
    public bool IsExplosionScene { get; private set; }
    public bool IsHighSpeedScene { get; private set; }

    [SerializeField] PlayerPhysicsDataSO player_data;
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "title")
        {
            SoundManager.Instance.PlayBGM("bgm_title_window");//ほかのbgmは別の個所：DestroyBrealWall：afterPressAnyBtn
        }
        else if(scene.name == "grow")
        {
            Debug.Log("growシーン");
            SoundManager.Instance.PlayBGM("bgm_grow");
            StartCoroutine(growFlow());
        }
        else if(scene.name == "select")
        {
            Debug.Log("selectシーン");
            SoundManager.Instance.PlayBGM("bgm_select");
            StartCoroutine(selectFlow());
        }
        else if (scene.name == "pirateShip")
        {
            Debug.Log("pirateシーン");
            SoundManager.Instance.PlayBGM("bgm_pi");
            StartCoroutine(pirateShipFlow());
        }else if(scene.name == "explosion")
        {
            Debug.Log("explosionシーン");
            SoundManager.Instance.PlayBGM("bgm_ex");
            StartCoroutine(explosionFlow());
        }
        else if (scene.name == "highSpeedRun")
        {
            Debug.Log("highSpeedRunシーン");
            SoundManager.Instance.PlayBGM("bgm_hs");
            StartCoroutine(highSpeedRunFlow());
        }
    }

    IEnumerator selectFlow()
    {
        player_data.clear();
        yield return AnimationManager.Instance.inactiveWhiteWall(0f, 1f);
    }

    IEnumerator growFlow()
    {
        yield return AnimationManager.Instance.inactiveWhiteWall(0f, 1f);
    }

    IEnumerator pirateShipFlow()
    {
       yield return AnimationManager.Instance.inactiveWhiteWall(0f, 2f);
       IsPirateShipScene = true;
    }

    IEnumerator explosionFlow()
    {
        yield return AnimationManager.Instance.inactiveWhiteWall(0f, 2f);
        IsExplosionScene = true;
    }
    IEnumerator highSpeedRunFlow()
    {
        yield return AnimationManager.Instance.inactiveWhiteWall(0f, 2f);
        IsHighSpeedScene = true;
    }



    public IEnumerator questLoador()
    {
        int quest_index = QuestManager.Instance.QuestIndex;

        yield return AnimationManager.Instance.activeWhiteWall(2f, 2f);
        switch (quest_index)
        {
            case 0:
                SceneManager.LoadScene("pirateShip");
                break;
            case 1:
                SceneManager.LoadScene("explosion");
                break;
            case 2:
                SceneManager.LoadScene("highSpeedRun");
                break;
        }
    }

    void simpleSceneLoad(string scene_name)
    {
        SceneManager.LoadScene(scene_name);
    }
    
    public void doneRoulette()
    {
        if (!IsdoneRoulette)
        {
            IsdoneRoulette = true;
            StartCoroutine(doneRouletteFlow());
        }
    }

    IEnumerator doneRouletteFlow()
    {
        yield return AnimationManager.Instance.activeWhiteWall(2f, 2f);
        simpleSceneLoad("grow");
    }

    public void touchGameStartWall()
    {
        if (!IstouchGameStartWall)
        {
            IstouchGameStartWall = true;
            StartCoroutine(touchGameStartWallFlow());
        }
    }

    IEnumerator touchGameStartWallFlow()
    {
        yield return AnimationManager.Instance.activeWhiteWall(0f, 2f);
        simpleSceneLoad("select");
    }

    public void inisialize()
    {
        IsdoneRoulette = false;
        IstouchGameStartWall = false;
        IsPirateShipScene = false;
        IsExplosionScene = false;
        IsHighSpeedScene = false;
    }
}
