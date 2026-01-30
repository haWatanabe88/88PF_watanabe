/*
 ゲームの進行状況の管理をしているもの
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{   
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI startText;
    [SerializeField] TextMeshProUGUI EndText;
    [SerializeField] TextMeshProUGUI tapText;

    //バルーンの生成位置を決めるための変数
    [SerializeField] float max_range_x = -9.0f;
    [SerializeField] float min_range_x = 9.0f;
    [SerializeField] float max_range_y = 3.0f;
    [SerializeField] float min_range_y = -2.5f;

    //スコア表示のための変数
    [SerializeField] float count = 0.0f;
    public float defo_score = 0.0f;
    public GameObject[] ballonPrefab;

    //ゲームがスタートしているのか、終了しているのかをみるためのブール値
    public bool _isStart = false;
    public bool _isEnd = false;

    public GameObject tapplate;



    // Start is called before the first frame update
    void Start()
    {
        //GameObject tapplate = GameObject.Find("tapplate");
    }

    // Update is called once per frame
    void Update()
    {
        if(_isStart == false && _isEnd == false)
        {
            //開始前
            startText.text = "Start!!";
            tapText.text = "tap";
            EndText.text = "";
        }
        else if(_isStart == false && _isEnd == true)
        {
            //ゲーム終了時
            tapplate.SetActive(true);
            EndText.text = "GameOver!";
            tapText.text = "";
        }
        else
        {
            //ゲーム中
            startText.text = "";
            tapText.text = "";
            count += Time.deltaTime;
            var pos = transform.position;
            pos.x = Random.Range(min_range_x, max_range_x);
            pos.y = Random.Range(min_range_y, max_range_y);
            if (count >= 1.0f)
            {
                count = 0.0f;
                GameObject ballon = Instantiate(SelectBallon(), pos, Quaternion.identity);
            }
        }
    }

    //バルーンをタップした時に得点を加算するための関数
    public void getScore(float score)
    {
        //Debug.Log("ok");
        defo_score += score;
        scoreText.text = "SCORE:" + defo_score.ToString("N0");
    }

    //いろんな色のバルーンを生成するための関数
    GameObject SelectBallon()
    {
        int index = Random.Range(0, ballonPrefab.Length);
        return ballonPrefab[index];
    }

    //ゲーム終了時にゲームを最初に戻すための関数
    public void ReStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
