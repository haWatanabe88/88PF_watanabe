using UnityEngine;

public class PlayerZoneStatus : MonoBehaviour
{
    public bool inStartPos {  get; private set; }
    public bool inUnCtrlZone {  get; private set; }
    public bool inEndZone {  get; private set; }
    public bool inSelectZone { get; private set; }

    public void setStartPos(bool val) {  inStartPos = val; }
    public void setUnCtrlZone(bool val) { inUnCtrlZone = val; }
    public void setEndZone(bool val) { inEndZone = val; }
    public void setSelectZone(bool val) { inSelectZone = val; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("start_pos_inTitle"))
            inStartPos = true;
        if (other.CompareTag("UnCtrlZone"))
            inUnCtrlZone = true;
        if (other.CompareTag("EndZone"))
            inEndZone = true;
        if (other.CompareTag("SelectZone"))
            inSelectZone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("start_pos_inTitle"))
            inStartPos = false;
        if (other.CompareTag("UnCtrlZone"))
            inUnCtrlZone = false;
        if (other.CompareTag("EndZone"))
            inEndZone = false;
    }
}
