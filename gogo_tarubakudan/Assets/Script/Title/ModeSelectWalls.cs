using UnityEngine;

public class ModeSelectWalls : MonoBehaviour
{
    SlideManager slideManager;
    private void Start()
    {
        slideManager = GameObject.FindWithTag("slidemanager").GetComponent<SlideManager>();
        this.gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(this.gameObject.tag == "gameStart")
            {
                Debug.Log("ゲームスタート");
                SoundManager.Instance.PlaySE("startwall_title");
                this.gameObject.GetComponent<BoxCollider>().enabled = false;
                SceneFlowManager.Instance.touchGameStartWall();
            }
            else if(this.gameObject.tag == "howToPlay")
            {
                Debug.Log("マニュアルを見よう");
                SoundManager.Instance.PlaySE("howtoplaywall_title");
                slideManager.showSlidePannel();
                Time.timeScale = 0f;
            }
        }
    }
}
