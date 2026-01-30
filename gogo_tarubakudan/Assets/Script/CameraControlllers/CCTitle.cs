using UnityEngine;
using UnityEngine.SocialPlatforms;

public class CCTitle : MonoBehaviour
{
    float duration = 4f;
    float elapsed = 0f;
    public bool isForce { get; private set; }
    Quaternion targetRotation;
    public GameObject main_camera;
    [SerializeField] GameObject start_zone;
    [SerializeField] GameObject player_obj;
    [SerializeField] GameObject target_position;
    [SerializeField] GameObject title_manager_obj;
    CCTracker cc_tracker;
    PlayerZoneStatus zone_status;
    TitleManager title_manager;

    float rotate_speed = 3f;


    private void Start()
    {
        zone_status = player_obj.GetComponent<PlayerZoneStatus>();
        cc_tracker = GameObject.FindWithTag("CC").GetComponent<CCTracker>();
        title_manager = title_manager_obj.GetComponent<TitleManager>();
    }

    public void setIsForce(bool value)
    {
        isForce = value;
    }

    private void LateUpdate()
    {
        forceMoveCameraRotation();
        forceMoveCameraMove();
    }

    void forceMoveCameraRotation()
    {
        if (!zone_status.inStartPos && !zone_status.inUnCtrlZone && !zone_status.inSelectZone)
        {
            cc_tracker.setYandZinisial();
            targetRotation = Quaternion.Euler(0f, main_camera.transform.localEulerAngles.y, 0f);
        }
        else if (zone_status.inStartPos || zone_status.inUnCtrlZone)
        {
            cc_tracker.setZadjust(7f);
            targetRotation = Quaternion.Euler(40f, 0f, 0f);
        }
        main_camera.transform.localRotation = Quaternion.Slerp(main_camera.transform.localRotation,
                                                               targetRotation,
                                                               Time.deltaTime * rotate_speed);
    }

    void forceMoveCameraMove()
    {
        if (title_manager.is_press_any_key)
        {
            if(elapsed <= duration)
            {
                elapsed += Time.deltaTime;
            }
            main_camera.transform.position = Vector3.Lerp(main_camera.transform.position
                                                         , target_position.transform.position
                                                         , elapsed / duration); 
        }
    }
}
