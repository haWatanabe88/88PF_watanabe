using UnityEngine;

public class SwitchButton : MonoBehaviour
{
    public SwitchPuzzleManager puzzleManager; // このスイッチが属するマネージャー
    private bool isPlayerInRange = false; // プレイヤーが範囲内か
    private MeshRenderer cubeRenderer;
    private Color originalColor;
    public Color highlightColor = Color.yellow;
    private bool isPressed = false; //（押されたかどうか）
    public bool isReset = false;

    void Start()
    {
        cubeRenderer = GetComponent<MeshRenderer>();
        if (cubeRenderer != null)
        {
            originalColor = cubeRenderer.sharedMaterial.color;
        }
    }

    private void Update()
    {
        //Debug.Log(isPressed);
        if(isReset)
        {
            isPressed = false;
            isReset = false;
        }
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (puzzleManager != null)
            {
                puzzleManager.PressSwitch(this);
                SetPressedState(true); // 🎯 押したら色を変える
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            PickupPromptUI.Instance.Show("Eキー：押す");
            if (cubeRenderer != null)
            {
                cubeRenderer.material.color = highlightColor;
            }
            else
            {
                cubeRenderer.material.color = originalColor;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            PickupPromptUI.Instance.Hide();
            if (!isPressed && cubeRenderer != null)
            {
                cubeRenderer.material.color = originalColor;
            }
        }
    }
    public void SetPressedState(bool pressed)
    {
        isPressed = pressed;
        if (pressed)
        {
            cubeRenderer.material.color = highlightColor;
        }
        else
        {
            cubeRenderer.material.color = originalColor;
            //Debug.LogWarning("リセットしたよん：" + isPressed);
        }
    }
}
