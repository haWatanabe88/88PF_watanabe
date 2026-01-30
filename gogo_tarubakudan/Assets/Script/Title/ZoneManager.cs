using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    [SerializeField] GameObject start_zone;
    [SerializeField] GameObject unctrl_zone;
    [SerializeField] GameObject end_zone;
    [SerializeField] GameObject select_zone;
    [SerializeField] GameObject slope;
    [SerializeField] GameObject ground_under;
    [SerializeField] GameObject manualManager;
    [SerializeField] GameObject leftclick_icon_ui;
    [SerializeField] GameObject camera_icon_ui;
    Rigidbody rb;
    float add_force;
    PlayerMovement player_movement;
    CCMovement cc_movement;
    CCTracker cc_tracker;
    PlayerZoneStatus zone_status;
    PlayerStatus player_status;
    PlayerVelocityControlle player_v;
    LikeMovieWalls like_movie_wall;

    private void Start()
    {
        rb = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
        player_movement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        player_v = GameObject.FindWithTag("Player").GetComponent<PlayerVelocityControlle>();
        player_status = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();
        cc_movement = GameObject.FindWithTag("CC").GetComponent<CCMovement>();
        cc_tracker = GameObject.FindWithTag("CC").GetComponent<CCTracker>();
        zone_status = GameObject.FindWithTag("Player").GetComponent<PlayerZoneStatus>();
        like_movie_wall = GameObject.FindWithTag("likeMovieWall").GetComponent<LikeMovieWalls>();
    }

    private void Update()
    {
        //どれかのゾーンにいるとき、カメラが動ないようにしている
        if (zone_status.inStartPos || zone_status.inUnCtrlZone || zone_status.inEndZone
                                                               || zone_status.inSelectZone)
        {
            cc_movement.enabled = false;
        }
        else
        {
            cc_movement.enabled = true;
        }
        if (zone_status.inUnCtrlZone)
        {
            player_v.velosityZero();
            like_movie_wall.wallClose();
            player_movement.enabled = false;// player_movement.enabled = true;はpressanybtnのあと
            manualManager.GetComponent<ManualManager>().hideManualCanvas();
        }
        if (zone_status.inEndZone)
        {
            cc_tracker.enabled = false;
            leftclick_icon_ui.SetActive(false);
            camera_icon_ui.SetActive(false);
            like_movie_wall.wallOpen();
        }
    }

    private void FixedUpdate()
    {
        if (zone_status.inEndZone)
        {
            add_force = 20f;
            rb.AddForce(ground_under.transform.forward * add_force, ForceMode.Acceleration);
        }
        else if(zone_status.inUnCtrlZone)
        {
            if (player_status.currentState == PlayerStatus.PlayerState.Small)
            {
                add_force = 1f;
            }
            else if (player_status.currentState == PlayerStatus.PlayerState.Middle)
            {
                //Debug.Log("M");
                add_force = 5f;
            }
            else if (player_status.currentState == PlayerStatus.PlayerState.Big)
            {
                //Debug.Log("B");
                add_force = 10f;
            }
            rb.AddForce(slope.transform.forward * add_force, ForceMode.Acceleration);
        }
    }

    public void afterPressAnyBtn()//何かのボタンを押してタイトルがきえたあとに
    {
        SoundManager.Instance.PlayBGM("bgm_title_modeselect");
        player_movement.enabled = true;
        player_movement.setMoveForce(100f);
        manualManager.GetComponent<ManualManager>().showManualCanvas();
        //cc_tracker.enabled = true;
    }
}
