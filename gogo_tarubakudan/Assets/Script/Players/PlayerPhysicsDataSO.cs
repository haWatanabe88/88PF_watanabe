using UnityEngine;
using static PlayerStatus;

[CreateAssetMenu(fileName = "PlayerPhysicsDataSO", menuName = "Scriptable Objects/PlayerPhysicsDataSO")]
public class PlayerPhysicsDataSO : ScriptableObject
{
    public float mass;
    public float linier_damping;
    public float scale_x;
    public float scale_y;
    public float scale_z;
    public PlayerState currentState;


    public void clear()
    {
        currentState = PlayerState.Small;
        mass = 1f;
        linier_damping = 1f;
        scale_x = 1f;
        scale_y = 1f;
        scale_z = 1f;
    }
}
