using UnityEngine;

public class PIPEIN : MonoBehaviour
{
    [SerializeField] GameObject pipe_out;

    void Start()
    {
        this.transform.position = pipe_out.transform.position;
    }
}
