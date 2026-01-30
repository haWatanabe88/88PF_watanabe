using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityChan;
using TMPro;

public class GameTimerManager : MonoBehaviour
{
    [Header("時間管理")]
    public float dayDuration = 90f;
    private float currentTime;
    private int currentDay = 1;
    private int maxDays = 7;
    
    [Header("UI")]
    public TMP_Text timerText;
    public GameObject gameOverPanel;
    public Button restartButton;

    [Header("フェード用")]
    public Image fadeImage;
    public float fadeDuration = 1.0f; // フェードにかかる時間

    [Header("プレイヤー初期情報")]
    public Transform player;
    private Vector3 startPosition;
    private Quaternion startRotation;

    [Header("カメラコントローラー")]
    public ThirdPersonCamera cameraController; // カメラ操作リセット用
    //ゲームオーバーしているかどうかの確認
    private static bool isGameOver = false;
    private static GameTimerManager instance;
    //暗転しているかどうかの確認
    private bool isFading = false;
    public static bool IsGameOverOrFading => isGameOver || (instance != null && instance.isFading);
    //recipechoice画面が表示されているかどうかの確認
    public static bool IsPauseByUI = false;

    //ダンジョン内の素材リスト
    private MaterialItemPickup[] allMaterials;

    //スキップに関するもの
    [Header("スキップ確認UI")]
    public GameObject skipConfirmPanel;
    public Button yesButton;
    public Button noButton;

    public static bool isSkipConfirming = false; // スキップ確認中かどうか

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        skipConfirmPanel.SetActive(false);
        currentTime = dayDuration;

        if (player != null)
        {
            startPosition = player.position;
            startRotation = player.rotation;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (restartButton != null)
        {
            isGameOver = false;
            restartButton.onClick.AddListener(RestartGame);
        }

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            fadeImage.color = new Color(c.r, c.g, c.b, 0f); // 最初は透明
        }

        // ★ここで全素材をリストアップ
        allMaterials = FindObjectsOfType<MaterialItemPickup>();
    }

    private void Update()
    {
        if (IsGameOverOrFading || IsPauseByUI || isSkipConfirming)
            return;
        if (GameClearManager.Instance.isGameClear)
            return;
        currentTime -= Time.deltaTime;
        UpdateTimerUI();

        if (currentTime <= 0f)
        {
            StartCoroutine(FadeToNextDay());
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            if(currentDay != maxDays)
            {
                timerText.text = $"{(maxDays + 1) - currentDay} Days Left - {Mathf.CeilToInt(currentTime)}s";
            }else
            {
                timerText.text = $"{(maxDays + 1) - currentDay} Day Left - {Mathf.CeilToInt(currentTime)}s";
            }
        }
    }

    private IEnumerator FadeToNextDay()
    {
        isFading = true;

        // フェードイン（黒くなる）
        yield return StartCoroutine(Fade(0f, 1f));

        currentDay++;

        if (currentDay > maxDays)
        {
            GameOver();
            yield break;
        }

        currentTime = dayDuration;

        // タイマーUI更新（暗転中に表示更新）
        UpdateTimerUI();

        // プレイヤー位置・向きリセット
        if (player != null)
        {
            player.position = startPosition;
            player.rotation = startRotation;
        }

        if (cameraController != null)
        {
            cameraController.ResetCameraRotation();
        }

        //素材を全部復活
        ResetAllMaterials();
        // 少し待ってからフェードアウト（自然な間）
        yield return new WaitForSeconds(0.5f);

        // フェードアウト（明るくなる）
        yield return StartCoroutine(Fade(1f, 0f));

        isFading = false;
    }

    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, to);
    }

    //１日経過した後に呼ぶ関数→素材を再取得可能にする
    private void ResetAllMaterials()
    {
        if (allMaterials == null) return;

        foreach (var mat in allMaterials)
        {
            if (mat != null && mat.respawnable)//現状、hopehurtだけ復活しないようにするため
            {
                mat.gameObject.SetActive(true);
            }
        }
    }

    //スキップボタンを押した時に呼ぶ関数
    public void OnSkipButtonPressed()
    {
        isSkipConfirming = true;
        if (skipConfirmPanel != null)
        {
            skipConfirmPanel.SetActive(true);
        }
    }
    //「はい」を押した時に呼ばれる関数
    public void OnConfirmSkip()
    {
        if (skipConfirmPanel != null)
        {
            skipConfirmPanel.SetActive(false);
        }

        isSkipConfirming = false;

        StartCoroutine(FadeToNextDay()); // すぐ次の日へ！
    }
    //「いいえ」を押した時に呼ばれる関数
    public void OnCancelSkip()
    {
        if (skipConfirmPanel != null)
        {
            skipConfirmPanel.SetActive(false);
        }

        isSkipConfirming = false;
    }


    private void GameOver()
    {
        isGameOver = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
