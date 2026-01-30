using UnityEngine;
using DG.Tweening;

public class CannonDamage : MonoBehaviour
{
    public int damage_count { get; private set; } //最終的にスコアに反映する予定
    [SerializeField] GameObject main_camera;

    private void Start()
    {
        damage_count = 0;
        main_camera = GameObject.FindWithTag("MainCamera");
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("obstacle"))
        {
            SoundManager.Instance.PlaySE("damage_pi");
            Destroy(collider.gameObject);
            main_camera.transform.DOShakePosition(1f, 0.3f, 15, 1, false, true);
            addDamageCount();
        }
    }

    void addDamageCount()
    {
        damage_count+=1;

    }
}
