using TMPro;
using UnityEngine;

public class text2PI : MonoBehaviour
{
    public float text2_pi_score { get; private set; }
    TextMeshProUGUI contentText;
    CannonDamage cannon_damage_component;
    int hitCount;

    private void Start()
    {
        hitCount = 0;
        contentText = GetComponent<TextMeshProUGUI>();
        cannon_damage_component = GameObject.FindWithTag("cannon").GetComponent<CannonDamage>();
    }

    void Update()
    {
        calcScore();
        contentText.text = $"è·äQï®Ç…ìñÇΩÇ¡ÇΩÅi{hitCount}âÒÅjÅF{text2_pi_score}";
    }

    void calcScore()
    {
        hitCount = cannon_damage_component.damage_count;
        text2_pi_score = hitCount * -800;
    }
}
