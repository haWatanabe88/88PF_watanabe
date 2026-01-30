using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RandomCreateMonster : MonoBehaviour
{

    [SerializeField] GameObject monster;
    [SerializeField]
    [Tooltip("¶¬‚·‚é”ÍˆÍ(MAX)")]
    Transform rangeA;
    [SerializeField]
    [Tooltip("¶¬‚·‚é”ÍˆÍ(MIN)")]
    Transform rangeB;

    int total_num;

    void Awake()
    {
        total_num = 30;
        for (int i = 1; i <= total_num; i++)
        {
            float x = Random.Range(rangeA.position.x, rangeB.position.x);
            float y = transform.position.y;
            float z = Random.Range(rangeA.position.z, rangeB.position.z);

            Instantiate(monster, new Vector3(x, y, z), monster.transform.rotation);
        }
    }
}