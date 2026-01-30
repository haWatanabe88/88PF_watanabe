using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class JsonLoader : MonoBehaviour
{
    public TextAsset stagePatternJsonData;
    string jsonText;
    JArray jsonArray;
    JObject enemy;
    public int index;
    float spawnCounter;
    public float startPosX;
    public float startPosY;
    public float endPosX;
    public float endPosY;
    public int hp;
    public int score;
    public EnemyManager enemyManager;
    public int squadCount;


    public static JsonLoader instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        Application.targetFrameRate = 60;
        jsonText = stagePatternJsonData.ToString();
        jsonArray = JArray.Parse(jsonText);
        index = 0;
        enemy = (JObject)jsonArray[index];
    }

    private void Update()
    {
        enemy = (JObject)jsonArray[index];
        spawnCounter = (float)enemy["SpawnCounter"];
        if ((float)Player.instance.spawnTimeSecond == spawnCounter)//Time.frameCount == spawnCounter
        {
            startPosX = (float)enemy["StartPosX"];
            startPosY = (float)enemy["StartPosY"];
            endPosX = (float)enemy["EndPosX"];
            endPosY = (float)enemy["EndPosY"];
            //敵生成
            string enemyTypeStr = (string)enemy["Type"];
            //敵の種類をenemy["Type"]によって分岐
            switch (enemyTypeStr)
            {//敵の種類のよる「HP」や「Score」を決定している
                //弾一発当たりのダメージ：50
                case "Normal":
                    enemyManager.SpawnEnemy(0);
                    hp = 50;
                    score = 100;
                    break;
                case "Hidden":
                    enemyManager.SpawnEnemy(1);
                    hp = 150;
                    score = 200;
                    break;
                case "Rare":
                    enemyManager.SpawnEnemy(2);
                    hp = 50;
                    score = 3000;
                    break;
                case "Wave":
                    enemyManager.SpawnEnemy(3);
                    hp = 100;
                    score = 150;
                    break;
                case "Surround":
                    enemyManager.SpawnEnemy(4);
                    hp = 250;
                    score = 250;
                    break;
                case "Bee":
                    enemyManager.SpawnEnemy(5);
                    hp = 200;
                    score = 500;
                    break;
                case "Missile":
                    enemyManager.SpawnEnemy(6);
                    hp = 300;
                    score = 200;
                    break;
                case "Squad":
                    enemyManager.SpawnEnemy(7);
                    squadCount = (int)enemy["SquadCount"];
                    hp = 200;
                    score = 500;
                    break;
                case "Boss":
                    enemyManager.SpawnEnemy(8);
                    hp = 5000;
                    score = 10000;
                    break;
            }
            if (jsonArray.Count - 1 > index)
            {
                index++;
            }
        }
    }
}