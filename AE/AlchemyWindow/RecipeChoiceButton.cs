using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeChoiceButton : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    private AlchemyRecipeSO currentRecipe;

    public void Set(AlchemyRecipeSO recipe)
    {
        currentRecipe = recipe;
        iconImage.sprite = recipe.resultItem.icon;
        nameText.text = recipe.resultItem.itemName;

        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() =>
        {
            AlchemyManager.Instance.ConfirmRecipe(currentRecipe);
            RecipeChoicePanel.Instance.Hide();
        });
    }
}
