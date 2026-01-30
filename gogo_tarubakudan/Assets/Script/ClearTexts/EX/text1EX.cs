using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class text1EX : MonoBehaviour
{
    public float text1_ex_score { get; private set; }
    TextMeshProUGUI contentText;
    [SerializeField] GameObject enemyManager;
    int number_of_defeat;


    private void Start()
    {
        contentText = GetComponent<TextMeshProUGUI>();
        text1_ex_score = 0;
    }

    void Update()
    {
        calcScore();
        contentText.text = $"åÇîjêîÅi{number_of_defeat}ëÃÅjÅF{number_of_defeat} Å~ 800 = {text1_ex_score}";
    }

    void calcScore()
    {
        number_of_defeat = enemyManager.GetComponent<EnemyManager>().number_of_defeat;
        text1_ex_score = number_of_defeat * 800;
    }
}

