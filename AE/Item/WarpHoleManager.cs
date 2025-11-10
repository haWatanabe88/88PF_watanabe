using System.Collections.Generic;
using UnityEngine;

public class WarpHoleManager : MonoBehaviour
{
    public static WarpHoleManager Instance { get; private set; }

    private WarpHole waitingHole = null; // 待機中のホール

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // ワープホールを登録する（ペアリング処理も行う）
    public void RegisterHole(WarpHole newHole)
    {
        if (waitingHole == null)
        {
            // 最初のホール → ペア待ちに登録
            waitingHole = newHole;
        }
        else
        {
            // 2つ目のホール → ペア確定
            waitingHole.pairedHole = newHole;
            newHole.pairedHole = waitingHole;

            // ログとリセット
            Debug.Log("ワープホールペアを確立しました！");
            WarpMessageUI.Instance?.ShowMessage("ワープホールが接続されました！");
            waitingHole = null;
        }
    }
}
