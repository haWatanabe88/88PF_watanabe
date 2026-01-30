using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RelatedPirateInformText : MonoBehaviour
{
    [SerializeField] GameObject endMarker;
    [SerializeField] GameObject clearAnimationManager;
    // スピード
    public float speed = 1.0F;
    bool game_clear;
    //二点間の距離を入れる
    private float distance_two;
    void Start()
    {
        this.gameObject.SetActive(false);
        distance_two = Vector3.Distance(transform.position, endMarker.transform.position);
    }

    private void Update()
    {
        if (game_clear)
        {
            StartCoroutine(moveClearText());
        }
    }

    public void ActiveText()
    {
        this.gameObject.SetActive(true);
        game_clear = true;
    }

    IEnumerator moveClearText()
    {
        yield return new WaitForSeconds(2f);
        float present_Location = (Time.time * speed) / distance_two;
        transform.position =
            Vector3.Lerp(transform.position
                        , endMarker.transform.position
                        , present_Location);
        clearAnimationManager.GetComponent<ClearAnimationManager>().clearAnimation();
    }
}
