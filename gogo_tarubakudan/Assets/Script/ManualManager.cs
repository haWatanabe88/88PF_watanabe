using UnityEngine;

public class ManualManager : MonoBehaviour
{
    [SerializeField] GameObject manual_canvas;


    public void showManualCanvas()
    {
        manual_canvas.SetActive(true);
    }

    public void hideManualCanvas()
    {
        manual_canvas.SetActive(false);
    }
}
