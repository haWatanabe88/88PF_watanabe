using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class GameClearManager : MonoBehaviour
{
    public static GameClearManager Instance { get; private set; }

    [SerializeField] private PlayableDirector[] timelines;
    [SerializeField] private CinemachineVirtualCamera[] virtualCameras;
    [SerializeField] private GameObject AlchemyWindowMoveIcon;
    [SerializeField] private GameObject skipPanel;
    [SerializeField] private GameObject Timer;

    public bool isGameClear = false;


    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        isGameClear = false;
    }

    public void OnGameClear()
    {
        isGameClear = true;
        AlchemyWindowMoveIcon.SetActive(false);
        skipPanel.SetActive(false);
        Timer.SetActive(false);
        // ① VirtualCamera をすべて有効化
        foreach (var vcam in virtualCameras)
        {
            if (vcam != null)
                vcam.gameObject.SetActive(true);
        }

        // ② Timeline を順に再生
        foreach (var timeline in timelines)
        {
            if (timeline != null)
                timeline.Play();
        }
    }
}
