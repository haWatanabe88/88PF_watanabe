using UnityEngine;

public class PlayerRestrict : MonoBehaviour
{
    public void setEnable(Behaviour b, bool value)
    {
        if (b == null) return;
        b.enabled = value;
    }

    public void setEnable(Collider c, bool value)
    {
        if (c == null) return;
        c.enabled = value;
    }

}
