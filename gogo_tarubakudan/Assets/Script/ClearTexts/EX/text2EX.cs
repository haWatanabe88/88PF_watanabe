using TMPro;
using UnityEngine;

public class text2EX : MonoBehaviour
{
    public float text2_ex_score { get; private set; }
    TextMeshProUGUI contentText;
    [SerializeField] GameObject ex_manager_obj;
    ExplosionManager ex_manager;
    int number_of_bullet;

    private void Start()
    {
        contentText = GetComponent<TextMeshProUGUI>();
        ex_manager = ex_manager_obj.GetComponent<ExplosionManager>();
    }

    void Update()
    {
        calcScore();
        contentText.text = $"残りの弾数（{number_of_bullet}個）：{number_of_bullet} × 1500 = {text2_ex_score}";
    }

    void calcScore()
    {
        number_of_bullet = ex_manager.number_of_bullet;
        text2_ex_score = number_of_bullet * 1500;
    }
}
