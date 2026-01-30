using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Transform tf;
    private Animator anim;

    public float moveSpeed = 0.01f;
    private Vector3 moveDirection;

    public Transform cameraTransform; // Main CameraのTransformをここに設定
    public GameObject alchemyPanel;
    public DroneCameraController droneCameraController;
    public GameObject recipeChoicePanel;
    [SerializeField] private Transform PlayerStartPos;
    void Start()
    {
        transform.position = PlayerStartPos.position;
        rb = GetComponent<Rigidbody>();
        tf = GetComponent<Transform>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (GameTimerManager.IsGameOverOrFading)
        {
            moveDirection = Vector3.zero;
            Rigidbody rb = GetComponent<Rigidbody>();
            anim.SetBool("walking", false);
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
            }
            return;
        }
        if (GameClearManager.Instance.isGameClear)
            return;
        // ドローン中は操作無効
        if (droneCameraController.IsDroneActive)
        {
            anim.SetBool("walking", false); // アニメ止める
            return;
        }
        // 特定のUIパネル上にマウスがあるときだけ操作無効
        if ((alchemyPanel.activeSelf &&
            RectTransformUtility.RectangleContainsScreenPoint(
            alchemyPanel.GetComponent<RectTransform>(),
            Input.mousePosition,null)) || recipeChoicePanel.activeSelf || GameTimerManager.isSkipConfirming)
        {
            moveDirection = Vector3.zero;
            anim.SetBool("walking", false);
            GameTimerManager.IsPauseByUI = true; // ←タイマーも停止
            return; 
        }
        GameTimerManager.IsPauseByUI = false;
        // 入力取得
        float h = Input.GetAxisRaw("Horizontal"); // A: -1, D: 1
        float v = Input.GetAxisRaw("Vertical");   // W: 1, S: -1

        // カメラの向きを基準に移動方向を決定
        Vector3 inputDirection = new Vector3(h, 0, v).normalized;

        if (cameraTransform != null)
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            moveDirection = camForward * inputDirection.z + camRight * inputDirection.x;
        }
        else
        {
            moveDirection = inputDirection; // カメラが未設定の場合はそのまま
        }

        // 向きを移動方向に合わせる（移動してるときだけ）
        if (moveDirection != Vector3.zero)
        {
            tf.forward = moveDirection;
        }

        bool isWalking = moveDirection.magnitude > 0f;
        anim.SetBool("walking", isWalking);
    }

    void FixedUpdate()
    {
        // Rigidbodyを使って移動処理（位置更新）
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }
}
