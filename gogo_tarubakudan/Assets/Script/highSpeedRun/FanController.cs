using UnityEngine;

public class FanController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("xŽ²‚Ì‰ñ“]Šp“x")]
    private float rotateZ = 0;
    [SerializeField] GameObject pipe_in;


    private void Start()
    {
        transform.position = new Vector3(pipe_in.transform.position.x, pipe_in.transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        // X,Y,ZŽ²‚É‘Î‚µ‚Ä‚»‚ê‚¼‚êAŽw’è‚µ‚½Šp“x‚¸‚Â‰ñ“]‚³‚¹‚Ä‚¢‚éB
        // deltaTime‚ð‚©‚¯‚é‚±‚Æ‚ÅAƒtƒŒ[ƒ€‚²‚Æ‚Å‚Í‚È‚­A1•b‚²‚Æ‚É‰ñ“]‚·‚é‚æ‚¤‚É‚µ‚Ä‚¢‚éB
        this.gameObject.transform.Rotate(new Vector3(0f, 0f, rotateZ) * Time.deltaTime);
        if(rotateZ >= 360f)
        {
            rotateZ -= 360f;
        }
    }
}
