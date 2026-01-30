using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    [SerializeField] GameObject explosionPrefab;

    private void Update()
    {
        if ((this.gameObject != null && Input.GetKey(KeyCode.Space)))
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}