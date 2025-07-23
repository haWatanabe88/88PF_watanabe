using System.Collections;
/*
 バルーンに対するソース
 */

using System.Collections.Generic;
using UnityEngine;
//using gd;

public class Balloon : MonoBehaviour
{
    GameObject gd;

    public float balloon_score = 50.0f;//とりあえず50点
    GameObject clickedGameObject;//Raycastのヒット情報を格納する
    // Start is called before the first frame update
    void Start()
    {
        gd = GameObject.Find("GameDirector");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("クリックした");
            clickedGameObject = null;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hitSprite = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            if (hitSprite == true)
            {
                //Debug.Log("ヒットした");
                clickedGameObject = hitSprite.transform.gameObject;
                if (clickedGameObject.tag == "balloon")
                {
                    //Debug.Log("バルーンだった");
                    Destroy(clickedGameObject);
                    GameDirector gd_script = gd.GetComponent<GameDirector>();//スコアを加算する関数→G.D.内に作る
                    gd_script.getScore(balloon_score);
                }
            }
        }
    }
}

/*
         if (Input.GetMouseButtonDown(0))
        {

            clickedGameObject = null;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hitSprite = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            if (hitSprite == true)
            {
                clickedGameObject = hitSprite.transform.gameObject;
                if (clickedGameObject.tag == "balloon")
                {
                    Destroy(clickedGameObject);
                    GameDirector gd_script = gd.GetComponent<GameDirector>();//スコアを加算する関数→G.D.内に作る
                    gd_script.getScore(balloon_score);
                }
            }
        }

////以下最初にやっていたこと
         if (Input.GetMouseButtonDown(0))//クリックしたら削除
        {
            GameDirector gd_script = gd.GetComponent<GameDirector>();//スコアを加算する関数→G.D.内に作る
            gd_script.getScore(balloon_score);
            Destroy(this.gameObject);
        }
 */