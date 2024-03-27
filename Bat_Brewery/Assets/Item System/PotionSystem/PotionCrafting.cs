using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the potion crafting in the game
/// </summary>
public class PotionCrafting : MonoBehaviour
{
    [SerializeField] PotionSO failedPotion;
    /// <summary>
    /// The ingredients added to the potion crafting
    /// </summary>
    public List<IngredientSO> addedIngredients = new List<IngredientSO>();
    PotionSO[] potions = new PotionSO[0];

    private void Start() 
    {
        potions = Resources.LoadAll<PotionSO>("Potions");
    }

    /// <summary>
    /// Try to craft a potion with the ingredients added to the potion crafting. The ingredients added to the crafting will also be cleared.
    /// </summary>
    /// <param name="recipePotion"></param>
    /// <returns></returns>
    public PotionSO CraftPotion()
    {   
        foreach (PotionSO recipePotion in potions)
        {
            if(recipePotion.GetIngredients().Length != addedIngredients.Count) break;
            foreach(IngredientSO ingredient in recipePotion.GetIngredients())
            {
                if(!addedIngredients.Contains(ingredient))
                {
                    break;
                }
            }

            addedIngredients = new List<IngredientSO>();
            return recipePotion;
        }
        addedIngredients = new List<IngredientSO>();
        return failedPotion;
    }
}
