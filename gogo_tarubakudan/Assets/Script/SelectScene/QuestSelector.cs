using UnityEngine;

/*
 add_move_num＝＞乱数にして（３ー6）
何秒ルーレットするか＝＞乱数にして（２or4秒）
 */


public class QuestSelector : MonoBehaviour
{
    [SerializeField] GameObject[] cursors;
    float roulette_time;
    float interval_time;
    float INITIAL_INTERVAL_TIME_DEFAULT = 0.3f;
    float INITIAL_INTERVAL_TIME_SLOW = 1.0f;
    int add_move_num;
    int quest_select_index = 0;
    bool is_play_roulette = false;

    void Start()
    {
        add_move_num = Random.Range(3,6);
        roulette_time = Random.Range(2.0f, 4.0f);
        is_play_roulette = true;//仮置き
        interval_time = INITIAL_INTERVAL_TIME_DEFAULT;
        cursors[quest_select_index].gameObject.SetActive(true);
    }

    void Update()
    {
        playRouletteChooseQuest();
    }


    void playRouletteChooseQuest()
    {
        if (is_play_roulette)
        {
            roulette_time -= Time.deltaTime;
            interval_time -= Time.deltaTime;

            if (roulette_time <= 0f)
            {
                roulette_time = 0f;
            }

            if (interval_time <= 0)
            {
                if (add_move_num > 0)
                {
                    if (roulette_time <= 0)
                    {
                        interval_time = INITIAL_INTERVAL_TIME_SLOW;
                        add_move_num--;
                    }
                    else
                    {
                        interval_time = INITIAL_INTERVAL_TIME_DEFAULT;
                    }
                    
                    if (add_move_num > 0)
                    {
                        SoundManager.Instance.PlaySE("movecursor_select");
                    }
                    else if (add_move_num == 0)
                    {
                        SoundManager.Instance.PlaySE("decide_select");
                    }
                    
                    cursors[quest_select_index].gameObject.SetActive(false);
                    quest_select_index++;
                    if (quest_select_index > 2)
                    {
                        quest_select_index = 0;
                    }
                }
                else
                {
                    is_play_roulette = false;
                    QuestManager.Instance.setQuestIndex(quest_select_index);//quest_select_index
                    SceneFlowManager.Instance.doneRoulette();
                }
            }
            cursors[quest_select_index].gameObject.SetActive(true);
        }
    }
    
}

