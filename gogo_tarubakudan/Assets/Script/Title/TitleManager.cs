using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] GameObject title_image;
    [SerializeField] GameObject zone_manager;
    [SerializeField] GameObject front_wall;
    [SerializeField] GameObject press_any_btn;


    PlayerZoneStatus player_zonestatus;
    [SerializeField] PlayerPhysicsDataSO player_data;
    bool is_destroy_break_wall;
    public bool is_press_any_key { get; private set; }
    public void DestroyBreakWall()
    {
        title_image.SetActive(true);
        SoundManager.Instance.PlaySE("bakuhatu_title");
        SoundManager.Instance.PlayBGM("bgm_title");
        is_destroy_break_wall = true;
    }

    public void ActiveFrontWall()
    {
        front_wall.SetActive(true);
    }

    private void Start()
    {
        player_data.clear();
        player_zonestatus = GameObject.FindWithTag("Player").GetComponent<PlayerZoneStatus>();
    }

    private void Update()
    {
        if (is_destroy_break_wall && !is_press_any_key)//壁が破壊された、かつ、何かのボタンが押されてない
        {
            //Debug.Log("壁壊れた");
            Time.timeScale = 0f;
            if (Input.anyKeyDown)
            {
                //Debug.Log("なんかボタン押された");
                Time.timeScale = 1f;
                zone_manager.GetComponent<ZoneManager>().afterPressAnyBtn();
                title_image.SetActive(false);
                is_destroy_break_wall = false;
                is_press_any_key = true;
            }
        }
        if (player_zonestatus.inSelectZone)
        {
            front_wall.SetActive (true);
        }
    }
}