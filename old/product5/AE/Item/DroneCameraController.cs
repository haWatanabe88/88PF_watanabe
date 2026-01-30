using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using TMPro;

public class DroneCameraController : MonoBehaviour
{
    [SerializeField] private Camera droneCamera;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float moveSpeed_beetle = 10f;
    [SerializeField] private float moveSpeed_mapping = 5f;
    [SerializeField] private float droneViewDuration = 5f; // ← 秒数をインスペクターから調整可
    private bool isActive = false;
    public bool IsDroneActive => isActive; // ←この `isActive` はカメラON時に true になるフラグ
    [SerializeField] public PlayerController player;
    /// <summary>
    /// マッピングドローンのみ
    /// </summary>
    [SerializeField] private GameObject droneCursor;//マッピングドローン時のカーソルイメージ
    [SerializeField] private TextMeshProUGUI droneManualText;//マッピングドローン時のカーソルイメージ
    private bool mappingMode = false;
    public bool IsMappingModeActive => mappingMode;
    [SerializeField] private GameObject dropPrefab; // インスペクターから設定
    [SerializeField] private float dropHeight = 10f; // 上空から落としたい高さ
    [SerializeField] private LayerMask groundLayer; // Raycastヒット対象（なくてもOK）
    [SerializeField] private int maxDropCount = 5;
    private int currentDropCount = 0;


    void Awake()
    {
        if (droneCamera != null)
        {
            droneCamera.enabled = false;
        }
        droneCamera.gameObject.SetActive(true);
    }

    private void Start()
    {
        droneCursor.SetActive(false);
        droneManualText.text = "";
    }

    void Update()
    {
        if (!isActive) return;

        float h = Input.GetAxisRaw("Horizontal"); // A / D
        float v = Input.GetAxisRaw("Vertical");   // W / S
        Vector3 move = new Vector3(h, 0, v).normalized;
        if(mappingMode)
        {
            droneCamera.transform.position += move * moveSpeed_mapping * Time.deltaTime;
        }else
        {
            droneCamera.transform.position += move * moveSpeed_beetle * Time.deltaTime;
        }
        if (mappingMode && Input.GetMouseButtonDown(0)) // ← 左クリック検知
        {
            DropPrefabAtCursor();
        }
    }

    public void StartDroneViewTimed()
    {
        currentDropCount = 0;
        StartCoroutine(DroneViewCoroutine());
    }

    private IEnumerator DroneViewCoroutine()
    {
        ActivateDroneCamera();
        yield return new WaitForSeconds(droneViewDuration);
        DeactivateDroneCamera();
    }

    public void ActivateDroneCamera()
    {
        Debug.Log("DroneCamera Activate 開始");
        droneCamera.transform.position = new Vector3(player.transform.position.x, droneCamera.transform.position.y, player.transform.position.z); 
        isActive = true;
        droneCamera.enabled = true;
        mainCamera.enabled = false;
        droneManualText.text = "wasd：移動　｜｜　左クリック：マーキング（マッピングドローン時のみ）";
    }

    public void DeactivateDroneCamera()
    {
        isActive = false;
        droneCamera.enabled = false;
        mainCamera.enabled = true;
        droneManualText.text = "";
        DisableMappingMode();
    }

    /// <summary>
    /// マッピングドローン関連
    /// </summary>
    public void EnableMappingMode()
    {
        mappingMode = true;
        droneCursor.SetActive(true);
    }

    public void DisableMappingMode()
    {
        mappingMode = false;
        droneCursor.SetActive(false);
    }
    private void DropPrefabAtCursor()
    {
        Ray ray = droneCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)); // 画面中央のRay
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f)) // 地面に当たったら
        {
            Vector3 dropPosition = hit.point + Vector3.up * dropHeight;
            Instantiate(dropPrefab, dropPosition, Quaternion.identity);

            Debug.Log("Prefabを落下させました: " + dropPosition);
            currentDropCount++;
            Debug.Log($"Prefab落下（{currentDropCount}/{maxDropCount}）");
        }
        else
        {
            Debug.Log("Raycastが何にも当たらなかった");
        }
        if (currentDropCount >= maxDropCount)
        {
            Debug.Log("最大設置回数に達しました。終了します。");
            DeactivateDroneCamera(); // 強制終了
            return;
        }
    }

}
