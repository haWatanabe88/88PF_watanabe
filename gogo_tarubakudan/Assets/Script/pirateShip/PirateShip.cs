using UnityEngine;

public class PirateShip : MonoBehaviour
{
    public float move_speed = 10.0f;   // 移動速度（10Fぐらいが３F目にはちょうど良さそうな印象）

    Vector3 start_pos;

    [SerializeField] Transform[] waypoints_phase1; // 左右のみ
    [SerializeField] Transform[] waypoints_phase2; // X-Z平面の四角形
    [SerializeField] RelatedPirateInformText inform_text;
    [SerializeField] GameObject timer_text;
    [SerializeField] GameObject manualManager;
    Transform[] current_waypoints;
    int current_index = 0;

    bool isDefeat;
    public bool getIsDefeat() { return isDefeat;}


    int hp;
    public int getHP() { return hp;}

    /// <summary>
    /// 障害物の発射
    /// </summary>
    [SerializeField] GameObject[] obstacles;
    int obstacles_index = 0;
    float interval_time;
    //int bullet_damage = 10;
    float INISIAL_INTERVAL_TIME = 2f;
    Vector3 adjust_height = new Vector3(0f, 1f, 0f);


    void Start()
    {
        isDefeat = false;
        timer_text.SetActive(true);
        start_pos = transform.position;
        current_waypoints = waypoints_phase1;
        interval_time = INISIAL_INTERVAL_TIME;
        hp = 100;
        manualManager.GetComponent<ManualManager>().showManualCanvas();
        Debug.Log(hp);
    }

    void Update()
    {
        if (!AnimationManager.Instance.getIsCompHpBarAnim())
            return;
        move();
        changeWaypointsByHP();
        instantiateObstacle();
        defeatPirateShip();
    }


    void move()
    {
        Transform target = current_waypoints[current_index];

        transform.position = Vector3.MoveTowards(
              transform.position,
              target.position,
              move_speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            if (hp <= 50)
            {
                int tmp = Random.Range(0, 4);
                {
                    if(tmp <= 1)//仮置き:０と１の時反転する、２、３の時順天
                    {
                        //Debug.Log("反転しました");
                        current_index = (current_index - 1 + current_waypoints.Length) % current_waypoints.Length;
                    }
                    else
                    {
                        current_index = (current_index + 1 + current_waypoints.Length) % current_waypoints.Length;
                    }
                }
            }
            else
            {
                current_index = (current_index + 1 + current_waypoints.Length) % current_waypoints.Length;
            }

        }
    }

    void changeWaypointsByHP()
    {
        if (hp <= 50)
        {
            current_waypoints = waypoints_phase2;
        }
        else if (hp >= 100)
        {
            current_waypoints = waypoints_phase1;
        }
    }

    void instantiateObstacle()
    {
        if (hp <= 70)
        {
            interval_time -= Time.deltaTime;
            if (interval_time <= 0)
            {
                SoundManager.Instance.PlaySE("attacked_pi");
                interval_time = INISIAL_INTERVAL_TIME;
                Instantiate(obstacles[obstacles_index], transform.position + adjust_height, transform.rotation);
                obstacles_index = (obstacles_index + 1) % obstacles.Length;
            }
        }
    }

    public void damageByBullet(int bullet_damage)
    {
        SoundManager.Instance.PlaySE("attack_pi");
        hp -= bullet_damage;
    }

    void defeatPirateShip()
    {
        if (hp <= 0)
        {
            hp = 0;
            timer_text.SetActive(false);
            manualManager.GetComponent<ManualManager>().hideManualCanvas();
            isDefeat = true;
            this.gameObject.SetActive(false);
            inform_text.ActiveText();
            AnimationManager.Instance.activeWhiteWall(2f, 2f);
        }
    }
}
