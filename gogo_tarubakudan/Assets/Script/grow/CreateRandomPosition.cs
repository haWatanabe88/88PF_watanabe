using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CreateRangeRandomPosition : MonoBehaviour
{

    [SerializeField] GameObject item_posi;
    [SerializeField] GameObject item_nega;
    [SerializeField]
    [Tooltip("¶¬‚·‚é”ÍˆÍ(MAX)")]
    Transform rangeA;
    [SerializeField]
    [Tooltip("¶¬‚·‚é”ÍˆÍ(MIN)")]
    Transform rangeB;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float checkRadius;

    int total_num;
    
    void Start()
    {
        total_num = 50;
        for (int i = 1; i <= total_num; i++) 
        {
            Vector3 spawnPos;
            do
            {
                float x = Random.Range(rangeA.position.x, rangeB.position.x);
                float z = Random.Range(rangeA.position.z, rangeB.position.z);
                spawnPos = new Vector3(x, 1.4f, z);

            } while (Physics.CheckSphere(spawnPos, checkRadius, playerLayer));

            if (i <= 40)
            {
                Instantiate(item_posi, spawnPos, item_posi.transform.rotation);
            }
            else
            {
                Instantiate(item_nega, spawnPos, item_nega.transform.rotation);
            }
        }
    }
}