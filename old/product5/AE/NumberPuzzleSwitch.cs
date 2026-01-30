using UnityEngine;

public class NumberPuzzleSwitch : MonoBehaviour
{
    [Header("表示・非表示を切り替えるCanvas")]
    public GameObject puzzleCanvas;
    [Header("スイッチの見た目（Renderer）")]
    public Renderer switchRenderer;
    [Header("このスイッチに対応するパズルマネージャー")]
    public NumberPuzzleManager puzzleManager;


    private bool isCleared = false; // パズルクリアしたかどうか

    private void Start()
    {
        if (puzzleCanvas != null)
        {
            puzzleCanvas.SetActive(false); // 最初は非表示
        }

        if (switchRenderer == null)
        {
            switchRenderer = GetComponent<Renderer>(); // 自分にRendererがあれば自動取得
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (isCleared)
        {
            return; // クリア済みなら無反応
        }

        if (other.CompareTag("Player"))
        {
            if (puzzleCanvas != null)
            {
                puzzleCanvas.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isCleared)
            return; // クリア済みなら無反応

        if (other.CompareTag("Player"))
        {
            if (puzzleCanvas != null)
            {
                puzzleCanvas.SetActive(false);
            }
        }
        // 離れたらパズルをリセットする！
        if (puzzleManager != null)
        {
            puzzleManager.ResetPuzzle();
        }
    }

    public void SetCleared()
    {
        isCleared = true;

        if (puzzleCanvas != null)
        {
            puzzleCanvas.SetActive(false); // 念のため消しておく
        }

        // 色を黄色に変える！
        if (switchRenderer != null)
        {
            switchRenderer.material.color = Color.yellow;
        }
    }
}
