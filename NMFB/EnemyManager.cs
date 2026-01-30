using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static bool created = false;

    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
        
    }

    public GameObject[] EnemyList;
    /*
     0:normal
     1:hidden
     2:rare
     3:wave
     4:surround
     5:bee
     6:missile
     7:squad
     */
    public void SpawnEnemy(int enemyIndex)
    {
        //Debug.Log("spawnされた");
        Instantiate(EnemyList[enemyIndex], transform.position, Quaternion.identity);
    }
}
