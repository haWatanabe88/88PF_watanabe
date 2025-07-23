/*
 ゲームスタートするために、tapするためのオブジェクトにアタッチされているもの 
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class Tap : MonoBehaviour
{
    GameObject gd;
    GameObject hitinfo;
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D raycasthit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
            if(raycasthit == true)
            {
                //Debug.Log("ヒットした");
                hitinfo = raycasthit.transform.gameObject;
                if(hitinfo.tag == "tapbox")
                {
                    GameDirector gd_class = gd.GetComponent<GameDirector>();
                    if (gd_class._isStart == false && gd_class._isEnd == false)//ゲームがまだスタートしていない時
                    {
                        //Debug.Log("tapboxだった");
                        //Destroy(hitinfo);
                        GameObject[] tapboxs = GameObject.FindGameObjectsWithTag("tapbox");
                        foreach (GameObject tapbox in tapboxs)
                        {
                            //Destroy(tapbox);
                            tapbox.SetActive(false);
                        }
                        gd_class._isStart = true;
                    }
                    else if(gd_class._isStart == false && gd_class._isEnd == true)//ゲームが終了している際に、再開する時
                    {
                        //Debug.Log("ゲーム終了後にタップされた");
                        gd_class.ReStart();
                        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    }
                }
            }     
        }

    }
}
