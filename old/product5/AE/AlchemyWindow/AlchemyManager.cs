//調合に関する処理。調合できるかどうか判定し、調合成功時スロットを空にする
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AlchemyManager : MonoBehaviour
{
    public DropSlot inputSlot1;
    public DropSlot inputSlot2;
    public DropSlot inputSlot3;
    public CraftedItemSO sampleResult; // 仮：生成するアイテム
    public DropSlot resultSlot;
    public List<AlchemyRecipeSO> recipeList; // レシピ一覧（Inspectorでアサイン）
    public List<CraftedItemSO> randomCraftedCandidates;  // ランダム調合の候補
    public CraftedItemSO excludeFromRandom;              // 除外アイテム
    public Image resultBannerImage;
    public Sprite dafaultSprite;  //デフォルトのスプライト
    public Sprite randomSprite;   //ランダム調合時の画像
    public Sprite successSprite;  //成功調合時の画像
    public static AlchemyManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }


        private void Start()
    {
        resultBannerImage.sprite = dafaultSprite;//デフォルトへ設定
    }

    public void OnClickCraft()
    {
        var inputItems = new List<MaterialItemSO>();

        if (inputSlot1.itemData is MaterialItemSO mat1) inputItems.Add(mat1);
        if (inputSlot2.itemData is MaterialItemSO mat2) inputItems.Add(mat2);
        if (inputSlot3.itemData is MaterialItemSO mat3) inputItems.Add(mat3);

        // 3個入ってなければ不可
        if (inputItems.Count != 3)
        {
            Debug.Log("素材が3つ必要です！");
            return;
        }
        // レシピを探す
        if (resultSlot.itemData != null)
        {
            Debug.Log("素材を「入手」してください");
            return;
        }
        List<AlchemyRecipeSO> matchedRecipes = new List<AlchemyRecipeSO>();

        foreach (var recipe in recipeList)
        {
            if (MatchRecipe(inputItems, recipe))
            {
                matchedRecipes.Add(recipe);
            }
        }

        if (matchedRecipes.Count == 0)
        {
            // ランダム生成（今まで通り）
            var filtered = randomCraftedCandidates
                .Where(item => item != excludeFromRandom)
                .ToList();

            if (filtered.Count == 0)
            {
                Debug.LogWarning("ランダム生成可能なアイテムがありません！");
                return;
            }

            var randomItem = filtered[Random.Range(0, filtered.Count)];
            resultSlot.DisplayResult(randomItem);
            resultBannerImage.sprite = randomSprite;
            resultBannerImage.color = Color.white;
            Debug.Log($"ランダム生成：{randomItem.itemName} を生成！");
            ClearInputSlots();
        }
        else if (matchedRecipes.Count == 1)
        {
            // 1件だけなら自動決定
            ConfirmRecipe(matchedRecipes[0]);
        }
        else
        {
            // 複数候補 → プレイヤーに選択してもらう
            RecipeChoicePanel.Instance.ShowChoices(matchedRecipes);
        }

        //foreach (var recipe in recipeList)
        //{
        //    if (MatchRecipe(inputItems, recipe))
        //    {
        //        resultBannerImage.sprite = successSprite;
        //        resultBannerImage.color = Color.white;
        //        Debug.Log($"調合成功！{recipe.resultItem.itemName} を生成");
        //        resultSlot.DisplayResult(recipe.resultItem);
        //        ClearInputSlots();
        //        return;
        //    }
        //    else// レシピにマッチしなかったら、ランダム生成
        //    {
        //        var filtered = randomCraftedCandidates
        //            .Where(item => item != excludeFromRandom)
        //            .ToList();

        //        if (filtered.Count == 0)
        //        {
        //            Debug.LogWarning("ランダム生成可能なアイテムがありません！");
        //            return;
        //        }

        //        var randomItem = filtered[Random.Range(0, filtered.Count)];

        //        resultSlot.DisplayResult(randomItem);
        //        ClearInputSlots();
        //        resultBannerImage.sprite = randomSprite;
        //        resultBannerImage.color = Color.white;
        //        Debug.Log($"ランダム生成：{randomItem.itemName} を生成！");
        //    }
        //}
    }
    public void OnClickReceiveResult()
    {
        if (resultSlot.itemData != null)
        {
            // インベントリに追加（1個）
            InventoryManager.Instance.AddCraftedItem(resultSlot.itemData);

            // スロットを空にする
            resultSlot.Clear();
            //視覚的フィードバックをデフォルトへ戻す
            resultBannerImage.sprite = dafaultSprite;//デフォルトへ戻す
        }
        else
        {
            Debug.Log("ResultSlot は空です！");
        }
    }

    void ClearInputSlots()
    {
        inputSlot1.Clear();
        inputSlot2.Clear();
        inputSlot3.Clear();
    }

    private bool MatchRecipe(List<MaterialItemSO> input, AlchemyRecipeSO recipe)
    {
        var required = recipe.requiredMaterials;
        if (input.Count != required.Count + recipe.wildcardCount) return false;

        var inputCopy = new List<MaterialItemSO>(input);

        foreach (var req in required)
        {
            var match = inputCopy.Find(item => item == req);
            if (match != null)
            {
                inputCopy.Remove(match);
            }
            else
            {
                return false; // 必要な素材が見つからない
            }
        }

        // 残り素材の数 = 入力数 - required分
        if (inputCopy.Count != recipe.wildcardCount)
        {
            return false; // ワイルドカード数と一致しない
        }
        return true;
    }

    public void ConfirmRecipe(AlchemyRecipeSO selectedRecipe)
    {
        resultSlot.DisplayResult(selectedRecipe.resultItem);
        resultBannerImage.sprite = successSprite;
        resultBannerImage.color = Color.white;
        //Debug.Log($"選択生成：{selectedRecipe.resultItem.itemName}");
        ClearInputSlots();
    }

}
