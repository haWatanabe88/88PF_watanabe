using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AlchemyRecipe", menuName = "Alchemy/Recipe")]
public class AlchemyRecipeSO : ScriptableObject
{
    public List<MaterialItemSO> requiredMaterials; // 必要な素材（順不同でOK）
    public int wildcardCount = 0;  // レシピ中の「なんでもいい素材の数」
    public CraftedItemSO resultItem;               // 作成されるアイテム
}
