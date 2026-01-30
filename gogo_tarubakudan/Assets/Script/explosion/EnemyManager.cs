using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    int MAX_ENEMY_NUM;
    public int number_of_defeat { get; private set; }

    void Start()
    {
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("enemy");
        MAX_ENEMY_NUM = enemyObjects.Length;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("enemy");
        number_of_defeat = MAX_ENEMY_NUM - enemyObjects.Length;
    }
}
