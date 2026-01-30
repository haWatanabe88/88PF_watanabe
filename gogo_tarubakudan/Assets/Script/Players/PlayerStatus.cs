using UnityEngine;
using UnityEngine.LowLevelPhysics2D;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] PlayerPhysicsDataSO physicsData;
    Rigidbody rb;

    public enum PlayerState
    {
        Small,
        Middle,
        Big
    }
    public PlayerState currentState;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentState = PlayerState.Small;
    }

    void OnDisable()
    {
        physicsData.currentState = currentState;
        physicsData.mass = rb.mass;
        physicsData.linier_damping = rb.linearDamping;
        physicsData.scale_x = this.transform.localScale.x;
        physicsData.scale_y = this.transform.localScale.y;
        physicsData.scale_z = this.transform.localScale.z;
    }

    public void setCurrentState(PlayerState val)
    {
        currentState = val;
    }
}
