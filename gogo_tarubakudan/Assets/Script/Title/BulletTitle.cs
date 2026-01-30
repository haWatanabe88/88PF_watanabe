using UnityEngine;

public class BulletTitle : MonoBehaviour
{
    [SerializeField] GameObject explosionPrefab;
    void Start()
    {
        adjustBulletScale(25f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("breakWall"))
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }

    void adjustBulletScale(float value)
    {
        Vector3 effect_scale = explosionPrefab.transform.localScale;
        effect_scale.x = value;
        effect_scale.y = value;
        effect_scale.z = value;
        explosionPrefab.transform.localScale = effect_scale;
    }
}
