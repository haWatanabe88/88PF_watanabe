using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AlchemyWindowToggle : MonoBehaviour
{
    public GameObject alchemyCanvas;
    public GameObject alchemyWindowMoveIcon;


    private void Start()
    {
        HideAlchemyCanvas();
    }

    void Update()
    {
        // マウスクリックした時
        if (Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUI())
            {
                HideAlchemyCanvas();
            }
        }
    }

    public void ShowAlchemyCanvas()
    {
        alchemyCanvas.SetActive(true);
        alchemyWindowMoveIcon.SetActive(false);
    }

    public void HideAlchemyCanvas()
    {
        alchemyCanvas.SetActive(false);
        alchemyWindowMoveIcon.SetActive(true);
    }

    bool IsPointerOverUI()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        return results.Count > 0;
    }
}
