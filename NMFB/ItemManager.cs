using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] public GameObject muscleItemPrefab;
    [SerializeField] public GameObject foodItemPrefab;

    float time;
    float interval;
    int th_muscleItem;//muscleアイテムの出現頻度→高ければ高いほど、マッスルアイテムが出やすい
    int generateNum;

    private void Start()
    {
        interval = 4.0f;//本来は今の所8を予定している→（変更：FATの時、4.0 MUSCLEの時、8.0）
        time = interval;
        th_muscleItem = 7;//ファットモードの時：8,マッスルモードの時、6（変更した）F:7M:5
        generateNum = 5;
    }

    private void Update()
    {
        if(!Player.instance.isClear)
        {
            time -= Time.deltaTime;
            if (Player.instance.currentMode == Player.Mode.MUSCLE)
            {
                th_muscleItem = 6;
                interval = 8.0f;
            }
            else if (Player.instance.currentMode == Player.Mode.FAT)
            {
                th_muscleItem = 8;
                interval = 4.0f;
            }
            if (Player.instance.isAllOut == true)
            {
                th_muscleItem = 2;
            }
            if (time < 0)
            {
                for (int i = 0; i < generateNum; i++)
                {
                    int rnd = Random.Range(1, 11);
                    float generate_y = Random.Range(Player.instance.yLimitMin, Player.instance.yLimitMax);
                    Vector3 generatePos = new Vector3(Player.instance.xLimitMax + 5.0f, generate_y, Player.instance.transform.position.z);
                    if (rnd <= th_muscleItem)
                    {
                        //Debug.Log("muscle生成");
                        GenerateMuscleItem(generatePos);
                    }
                    else
                    {
                        //Debug.Log("food生成");
                        GeneratefoodItem(generatePos);
                    }
                }
                time = interval;
            }
        }
    }

    void GenerateMuscleItem(Vector3 generatePosition)
    {
        Instantiate(muscleItemPrefab, generatePosition, Quaternion.identity);
    }

    void GeneratefoodItem(Vector3 generatePosition)
    {
        Instantiate(foodItemPrefab, generatePosition, Quaternion.identity);
    }
}
