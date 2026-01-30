using UnityEngine;
using UnityEngine.LowLevelPhysics2D;
using UnityEngine.SceneManagement;

public class PlayerItemPickup : MonoBehaviour
{
    int obtain_positive_item_num = 0;
    public int MAX_OBTAIN_ITEM_NUM { get; private set; }
    int MIDDLE_OBTAIN_ITEM_NUM;
    int MIN_OBTAIN_ITEM_NUM = 0;
    [SerializeField] GameObject energy_ui_obj;
    EnergyUI energy_ui_component;
    PlayerStatus player_status;
    Rigidbody rb;
    
    private void Start()
    {
        energy_ui_component = energy_ui_obj.GetComponent<EnergyUI>();
        player_status = GetComponent<PlayerStatus>();
        rb = GetComponent<Rigidbody>();
    }

    //private void Update()
    //{
    //    Debug.Log(obtain_positive_item_num);
    //}
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnTriggerEnter(Collider other_collision)
    {
        obtainItemNumChange(other_collision);
        parameterChange();
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "title")
        {
            settingThresholdItemNumInTitle();
        }
        else if(scene.name == "grow")
        {
            settingThresholdItemNum();
        }
    }

    public void obtainItemNumChange(Collider other_collision)
    {
        if (other_collision.gameObject.CompareTag("mini_bomb_positive"))
        {
            if(obtain_positive_item_num < MAX_OBTAIN_ITEM_NUM)
            {
                obtain_positive_item_num++;
            }
            SoundManager.Instance.PlaySE("posi_grow");
            energy_ui_component.addFillAmount();
        }
        else if (other_collision.gameObject.CompareTag("mini_bomb_negative"))
        {
            if(obtain_positive_item_num > MIN_OBTAIN_ITEM_NUM)
            {
                obtain_positive_item_num--;
            }
            SoundManager.Instance.PlaySE("nega_grow");
            energy_ui_component.decreseFillAmount();
        }
    }

    public int getObtainPositiveItemnum()
    {
        return obtain_positive_item_num;
    }

    void parameterChange()
    {
        if (obtain_positive_item_num < MIDDLE_OBTAIN_ITEM_NUM)//MIDDLE未満だったら、小（デフォルトスケール）
        {
            player_status.currentState = PlayerStatus.PlayerState.Small;
            scaleChange(1.0f);
            mass_linierDampChange(1.0f, 1.0f);
        }
        else if (obtain_positive_item_num >= MAX_OBTAIN_ITEM_NUM)//大スケール
        {
            player_status.currentState = PlayerStatus.PlayerState.Big;
            scaleChange(2.0f);
            mass_linierDampChange(2.0f, 0.6f);
        }
        else if (obtain_positive_item_num >= MIDDLE_OBTAIN_ITEM_NUM)//中スケール
        {
            player_status.currentState = PlayerStatus.PlayerState.Middle;
            scaleChange(1.5f);
            mass_linierDampChange(1.5f, 0.8f);
        }
    }

    void settingThresholdItemNumInTitle()
    {
        MAX_OBTAIN_ITEM_NUM = 6;
        MIDDLE_OBTAIN_ITEM_NUM = 3;
    }

    void settingThresholdItemNum()
    {
        MAX_OBTAIN_ITEM_NUM = 22;
        MIDDLE_OBTAIN_ITEM_NUM = 10;
    }

    //ヘルパー関数
    void scaleChange(float value)
    {
        //今のローカルスケールのx（なんでもいい）がvalueの値よりおおきければ、powerupsound,
        //小さければ、powerdownsoundをならす
        if(this.gameObject.transform.localScale.x < value)
        {
            SoundManager.Instance.PlaySE("powerup_grow");
        }else if (this.gameObject.transform.localScale.x > value)
        {
            SoundManager.Instance.PlaySE("powerdown_grow");
        }
        transform.localScale = new Vector3(value, value, value);
    }
    void mass_linierDampChange(float new_mass, float new_linier_damp)
    {
        rb.mass = new_mass;
        rb.linearDamping = new_linier_damp;
    }
}
