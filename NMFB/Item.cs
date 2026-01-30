using UnityEngine;

public class Item : MonoBehaviour
{

    float speed;

    private void Update()
    {
        speed = Random.Range(0.5f, 10.0f);
        transform.position -= new Vector3(1f, 0, 0) * Time.deltaTime * speed;
        if (transform.position.x <= Player.instance.xLimitMin - 5.0f)
        {
            Destroy(this.gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("playerと接触");
            if (this.gameObject.CompareTag("MuscleItem"))
            {
                Player.instance.MuscleItemGetCount();
            }
            else if (this.gameObject.CompareTag("FoodItem"))
            {
                Player.instance.FoodItemGetCount();
            }
            Destroy(this.gameObject);
        }
    }
}
