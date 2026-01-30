using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class RecipeChoicePanel : MonoBehaviour
{
    public static RecipeChoicePanel Instance { get; private set; }

    [SerializeField] private GameObject panelRoot; // パネル本体
    [SerializeField] private List<RecipeChoiceButton> buttons; // ボタン3つ

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;

        panelRoot.SetActive(false); // 最初は非表示
    }

    public void ShowChoices(List<AlchemyRecipeSO> recipes)
    {
        panelRoot.SetActive(true);

        for (int i = 0; i < buttons.Count; i++)
        {
            if (i < recipes.Count)
            {
                buttons[i].gameObject.SetActive(true);
                buttons[i].Set(recipes[i]);
            }
            else
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
    }

    public void Hide()
    {
        panelRoot.SetActive(false);
    }
}
