using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class LikeMovieWalls : MonoBehaviour
{
    [SerializeField] GameObject wall_up;
    [SerializeField] GameObject wall_down;
    bool is_close;
    bool is_open;

    private void Start()
    {
        is_close = false;
        is_open = false;
    }

    //private void Update()
    //{
    //    Debug.Log(wall_up.transform.position);
    //    //Debug.Log(wall_down.transform.position);
    //}

    public void wallClose()
    {
        if (!is_close)
        {
            is_close = true;
            wall_up.transform.DOLocalMoveY(wall_up.transform.localPosition.y - 155 , 1f);
            wall_down.transform.DOLocalMoveY(wall_down.transform.localPosition.y + 155, 1f);
        }
    }

    public void wallOpen()
    {
        if (!is_open)
        {
            is_open=true;
            wall_up.transform.DOLocalMoveY(wall_up.transform.localPosition.y + 155, 1f);
            wall_down.transform.DOLocalMoveY(wall_down.transform.localPosition.y - 155, 1f);
        }
    }
}
