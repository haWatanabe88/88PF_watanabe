using UnityEngine;
using UnityEngine.EventSystems;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;          // プレイヤーなど追いかける対象
    public float distance = 5.0f;     // ターゲットとの距離（インスペクターで調整可）
    public float height = 2.0f;       // 少し上から見る高さ（視点の持ち上げ）
    public float rotationSpeed = 5.0f; // カメラ回転のスピード

    private float currentRotationAngle = 0f; // 左右回転用の角度
    public GameObject alchemyPanel;
    private float startRotationAngle; // 初期回転角を保存

    // ★追加：Raycastする時に使うレイヤーマスク
    public LayerMask obstacleLayers;

    private void Start()
    {
        startRotationAngle = currentRotationAngle;
    }

    void LateUpdate()
    {
        if (alchemyPanel.activeSelf &&
            RectTransformUtility.RectangleContainsScreenPoint(
                alchemyPanel.GetComponent<RectTransform>(),
                Input.mousePosition,
                null))
        {
            return;
        }

        if (!target) return;

        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");
            currentRotationAngle += mouseX * rotationSpeed;
        }

        Quaternion rotation = Quaternion.Euler(0, currentRotationAngle, 0);

        Vector3 offset = rotation * new Vector3(0, 0, -distance);
        Vector3 desiredPosition = target.position + offset + Vector3.up * height;

        // ★プレイヤーとカメラの間をRaycastして、壁があれば寄せる
        Vector3 direction = desiredPosition - (target.position + Vector3.up * height * 0.5f);
        float distanceToTarget = direction.magnitude;
        direction.Normalize();

        RaycastHit hit;
        if (Physics.Raycast(target.position + Vector3.up * height * 0.5f, direction, out hit, distanceToTarget, obstacleLayers))
        {
            // 壁にぶつかったら、ぶつかったポイントにカメラを置く
            transform.position = hit.point;
        }
        else
        {
            // 壁がなければ普通に追従
            transform.position = desiredPosition;
        }

        transform.LookAt(target.position + Vector3.up * height * 0.5f);
    }


    public void ResetCameraRotation()
    {
        currentRotationAngle = startRotationAngle;
    }

}